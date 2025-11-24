const express = require("express");
const http = require("http");
const socketIo = require("socket.io");

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

app.use(express.static("public"));

//mendeskripsikan state game
let gameState = {
  board: Array(8).fill().map(()=>
  Array(8).fill(null)
  ),
  currentPlayer: 'black',
  players: {}
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
}
//panggil initialisasi
initializeBoard();

//menambahkan event listener untuk event 'cellClick' yang dikirim dari client.

io.on("connection", (socket) => {
  console.log("User Connected:", socket.id);

  //ngirim state game ke client yg baru connect
  socket.emit('gameState', gameState);

  //terima event cellClick dari client
  socket.on("cellClick", (data) => {
    console.log("cell clicked", data);

    const {row, col} = data;

    //validasi move sederhana
    if (gameState.board[row][col] === null)
    {
      //tambahkan piece
      gameState.board[row][col] = gameState.currentPlayer;

      //ganti player
      gameState.currentPlayer = gameState.currentPlayer === 'black' ? 'white' : 'black';

      io.emit('boardUpdate', {
        board: gameState.board,
        currentPlayer: gameState.currentPlayer
      });

      console.log(`Piece ${gameState.board[row][col]} ditempatkan di [${row}, ${col}]`);


    }else {
      console.log(`Cell [${row}, ${col}] sudah terisi`);
      socket.emit('invalidMove', {row, col});
    }

  });
  socket.on('disconnect', ()=> {
    console.log('User disconnected:', socket.id);
  });
});

server.listen(3000, () => {
  console.log("Server running on port 3000");
});
