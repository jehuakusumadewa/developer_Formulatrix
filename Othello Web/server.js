const express = require("express");
const http = require("http");
const socketIo = require("socket.io");

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

app.use(express.static("public"));

const DIRECTIONS = [
  [-1, -1], [-1, 0], [-1, 1],
  [0, -1],           [0, 1],
  [1, -1],   [1, 0],  [1, 1] 
];

// mendeskripsikan state game
let gameState = {
  board: [],
  currentPlayer: 'black',
  scores: {black: 2, white: 2},
  gameOver: false,
  validMoves: []
};

function initializeBoard() {
  // kosongkan board dan buat dengan benar
  gameState.board = [];
  for (let i = 0; i < 8; i++) {
    gameState.board[i] = [];
    for (let j = 0; j < 8; j++) {
      gameState.board[i][j] = null;
    }
  }

  // posisi awal othello
  gameState.board[3][3] = 'white';
  gameState.board[3][4] = 'black';
  gameState.board[4][3] = 'black';
  gameState.board[4][4] = 'white';

  gameState.currentPlayer = 'black';
  gameState.scores = {black: 2, white: 2};
  gameState.gameOver = false;
  gameState.validMoves = calculateValidMoves('black');
  
  console.log("Board initialized:", gameState.board);
}

// cek posisi dalam lingkup papan
function isValidPosition(row, col) {
  return row >= 0 && row < 8 && col >= 0 && col < 8;
}

// function buat check apakah move valid
function getFlippedPiece(board, row, col, player) {
    // Perbaikan: Cek validitas posisi dan pastikan board ada
    if (!isValidPosition(row, col) || !board[row] || board[row][col] !== null) {
        return [];
    }
    
    const opponent = player === 'black' ? 'white' : 'black';
    const flipped = [];
    
    for (const [dr, dc] of DIRECTIONS) {
        const directionFlipped = [];
        let r = row + dr;
        let c = col + dc;
        
        // Cari pieces lawan yang berurutan
        while (isValidPosition(r, c) && board[r] && board[r][c] === opponent) {
            directionFlipped.push([r, c]);
            r += dr;
            c += dc;
        }
        
        // Jika setelah pieces lawan ada pieces sendiri, maka valid
        if (isValidPosition(r, c) && board[r] && board[r][c] === player && directionFlipped.length > 0) {
            flipped.push(...directionFlipped);
        }
    }
    
    return flipped;
}

function calculateValidMoves(player){
    const validMoves = [];

    for (let row = 0; row < 8; row++){
      for (let col = 0; col < 8; col++){
        // Perbaikan: Pastikan board[row] ada sebelum mengakses
        if (gameState.board[row]) {
            const flipped = getFlippedPiece(gameState.board, row, col, player);
            if (flipped.length > 0){
              validMoves.push({row, col, flipped});
            }
        }
      }
    }
    return validMoves;
} 

// eksekusi move dan flip piece
function makeMove(row, col, player) {
  // Perbaikan: Validasi tambahan sebelum memproses move
  if (!isValidPosition(row, col) || !gameState.board[row]) {
      return false;
  }

  const flipped = getFlippedPiece(gameState.board, row, col, player);

  if(flipped.length === 0) return false;
  
  // tempatkan piece pemain
  gameState.board[row][col] = player;

  // flip pieces lawan
  flipped.forEach(([r, c]) => {
    if (isValidPosition(r, c) && gameState.board[r]) {
        gameState.board[r][c] = player;
    }
  });

  // update scores
  updateScores();
  return true;
}

function updateScores() {
  let black = 0, white = 0;
  for(let row = 0; row < 8; row++){
    if (gameState.board[row]) {
        for(let col = 0; col < 8; col++) {
          if(gameState.board[row][col] === 'black') black++;
          if(gameState.board[row][col] === 'white') white++;
        }
    }
  }
  gameState.scores = {black, white};
}

// ngecheck game
function checkGameOver() {
  const blackMoves = calculateValidMoves('black');
  const whiteMoves = calculateValidMoves('white');

  if(blackMoves.length === 0 && whiteMoves.length === 0) {
    gameState.gameOver = true;
    return true;
  }
  return false;
}

function switchPlayer() {
  gameState.currentPlayer = gameState.currentPlayer === 'black' ? 'white' : 'black';
  gameState.validMoves = calculateValidMoves(gameState.currentPlayer);

  // jika tidak ada valid moves, skip turn
  if (gameState.validMoves.length === 0 && !checkGameOver()) {
    switchPlayer();
  }
}

// panggil initialisasi
initializeBoard();

// Debug: Cek struktur board
console.log("Board structure check:");
for (let i = 0; i < 8; i++) {
    console.log(`Row ${i}:`, gameState.board[i] ? `Length: ${gameState.board[i].length}` : 'UNDEFINED');
}

// menambahkan event listener untuk event 'cellClick' yang dikirim dari client.
io.on("connection", (socket) => {
  console.log("User Connected:", socket.id);

  // ngirim state game ke client yg baru connect
  socket.emit('gameState', gameState);

  // terima event cellClick dari client
  socket.on("cellClick", (data) => {
    if (gameState.gameOver) return;
    
    const { row, col } = data; 
    const player = gameState.currentPlayer;

    console.log(`cell clicked by ${socket.id}: [${row}, ${col}] by ${player}`);

    // Validasi bahwa row dan col berada dalam rentang yang benar
    if (!isValidPosition(row, col)) {
      console.log(`Invalid position: [${row}, ${col}]`);
      socket.emit('invalidMove', { 
        row, col,
        message: 'Invalid position!'
      });
      return;
    }

    // Validasi tambahan: pastikan baris ada di board
    if (!gameState.board[row]) {
      console.log(`Invalid row: [${row}]`);
      socket.emit('invalidMove', { 
        row, col,
        message: 'Invalid board position!'
      });
      return;
    }

    const isValidMove = gameState.validMoves.some(move =>
      move.row === row && move.col === col
    );

    if (isValidMove) {
      const success = makeMove(row, col, player);
      if (success) {
        checkGameOver();

        if (!gameState.gameOver) {
          switchPlayer();
        }
        
        io.emit('gameState', gameState);
        console.log(`Valid move: ${player} at [${row}, ${col}]`);
      } else {
        console.log(`Move failed: ${player} at [${row}, ${col}]`);
        socket.emit('invalidMove', { 
          row, col,
          message: 'Move failed!'
        });
      }
    } else {
      console.log(`Invalid Move: ${player} at [${row}, ${col}]`);
      socket.emit('invalidMove', { 
        row, col,
        message: 'Invalid move!'
      });
    }

  socket.on('resetGame', () => {
    console.log("Game reset by", socket.id);

    initializeBoard();  // panggil reset board

    io.emit('gameState', gameState); // kirim state baru ke semua client
  });


  });

  socket.on('disconnect', () => {
    console.log('User disconnected:', socket.id);
  });
});

server.listen(3000, () => {
  console.log("Server running on port 3000");
});