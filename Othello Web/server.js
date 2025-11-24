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



//mendeskripsikan state game
let gameState = {
  board: Array(8).fill().map(()=>
  Array(8).fill(null)
  ),
  currentPlayer: 'black',
  scores: {black: 2, white: 2},
  gameOver: false,
  validMoves: []
};

function initializeBoard() {
  //kosongkan board
  gameState.board = Array(8).fill().map(() => Array(8).fill(null));

  //posisi awal othello

  gameState.board[3][3] = 'white';
  gameState.board[3][4] = 'black';
  gameState.board[4][3] = 'black';
  gameState.board[4][4] = 'white';

  gameState.currentPlayer = 'black';
  gameState.scores = {black: 2, white: 2};
  gameState.gameOver = false;
  gameState.validMoves = calculateValidMoves('black');
}

//cek posisi dalam lingkup papan gk
function isValidPosition(row, col) {
  return row >= 0 && row < 8 && col >= 0 && col < 8;
}

//function buat check apakah move valid
function getFlippedPiece(board, row, col, player) {
    if (board[row][col] !== null) return [];
    
    const opponent = player === 'black' ? 'white' : 'black';
    const flipped = [];
    
    for (const [dr, dc] of DIRECTIONS) {
        const directionFlipped = [];
        let r = row + dr;
        let c = col + dc;
        
        // Cari pieces lawan yang berurutan
        while (isValidPosition(r, c) && board[r][c] === opponent) {
            directionFlipped.push([r, c]);
            r += dr;
            c += dc;
        }
        
        // Jika setelah pieces lawan ada pieces sendiri, maka valid
        if (isValidPosition(r, c) && board[r][c] === player && directionFlipped.length > 0) {
            flipped.push(...directionFlipped);
        }
    }
    
    return flipped;
}

function calculateValidMoves(player){
    const validMoves = [];

    for (let row = 0; row < 8; row++){
      for (let col = 0; col < 8; col++){
        const flipped = getFlippedPiece(gameState.board, row, col, player);
        if (flipped.length > 0){
          validMoves.push({row, col, flipped});
        }
      }
    }
    return validMoves;
} 
//eksekusi move dan flip piece
function makeMove(row, col, player) 
{
  const flipped = getFlippedPiece(gameState, row, col, player);

  if(flipped.length === 0) return false;
  
  //tempatkan piece pemain
  gameState.board[row][col] = player;

  //flip pieces lawan
  flipped.forEach((r, c) => {
    gameState.board[r][c] = player;
  });

  //upadate scores

  updateScores();
  return true;

}

function updateScores() {
  let black = 0, white = 0;
  for(let row = 0; row < 8; row++){
    for(let col = 0; col < 8; col++) 
    {
      if(gameState.board[ro][col] === 'black') black++
      if(gameState.board[row][col] === 'white') white++
    }
  }
  gameState.scores = {black, white};
}

//ngecheck game
function checkGameOver() {
  const blackMoves = calculateValidMoves('black');
  const whiteMoves = calculateValidMoves('white');

  if(blackMoves.length === 0 && whiteMoves.length === 0)
  {
    gameState.gameOver = true;
    return true;
  }
  return false;
}


function switchPlayer() {
  gameState.currentPlayer = gameState.currentPlayer === 'black' ? 'white' : 'black';
  gameState.validMoves = calculateValidMoves(gameState.currentPlayer);

  //jika tidak ada valid moved, skip turn

  if (gameState.validMoves.length === 0 && !checkGameOver())
  {
    switchPlayer();
  }
}


















//panggil initialisasi
initializeBoard();

//menambahkan event listener untuk event 'cellClick' yang dikirim dari client.
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

    const isValidMove = gameState.validMoves.some(move =>
      move.row === row && move.col === col
    );

    if (isValidMove) {
      makeMove(row, col, player);
      checkGameOver();

      if (!gameState.gameOver) {
        switchPlayer();
      }
      
      io.emit('gameState', gameState);
      console.log(`Valid move: ${player} at [${row}, ${col}]`);
    } else {
      console.log(`Invalid Move: ${player} at [${row}, ${col}] sudah terisi`);
      socket.emit('invalidMove', { 
        row, col,
        message: 'Invalid move!'
      });
    }
  });

  socket.on('resetGame', () => {
    initializeBoard();
    io.emit('gameState', gameState);
    console.log('Game reset');
  })

  socket.on('disconnect', () => {
    console.log('User disconnected:', socket.id);
  });
});



server.listen(3000, () => {
  console.log("Server running on port 3000");
});
