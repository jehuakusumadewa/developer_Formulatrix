document.addEventListener("DOMContentLoaded", () => {
  const board = document.getElementById('board');
  const currentPlayerSpan = document.getElementById('currentPlayerSpan');
  const debugDiv = document.getElementById('debug');
  const socket = io(); // Terhubung ke server
  let currentValidMoves = [];

  //function untuk update tampilan papan
  function updateBoard(boardState, validMoves = [])
  {
    const cells = document.querySelectorAll('.cell');
    currentValidMoves = validMoves;
    cells.forEach (cell => {
      const row = parseInt(cell.dataset.row);
      const col = parseInt(cell.dataset.col);
      const piece = boardState[row][col];

      //reset cell styling ?
      cell.classList.remove('valid-move');

      const existingPiece = cell.querySelector('.piece');
      if (existingPiece) {
        existingPiece.remove();
      }

      if (piece) {
        const pieceDiv = document.createElement('div');
        pieceDiv.className = `piece ${piece}`;
        cell.appendChild(pieceDiv);
      }
      if(validMoves.some(move =>
        move.row === row && move.col === col))
      { 
        cell.classList.add('valid-move');
      }

    });
  }

  //function untuk update game info

  function updateGameInfo(state) {
    currentPlayerSpan.textContent = state.currentPlayer;
    currentPlayerSpan.className = state.currentPlayer;

    const scores = `Black: ${state.scores.black} | White ${state.scores.white}`;
    document.getElementById('scores').textContent = scores;

    if (state.gameOver) 
    {
      let winner = 'Draw!';
      if (state.scores.black > state.scores.white) winner = 'Black Wins!';
      if (state.scores.white > state.scores.black) winner = 'White Wins!';

      document.getElementById('gameStatus').innerHTML = `<strong> Game Over! ${winner} </strong>`;

    }else {
      document.getElementById('gameStatus').textContent = 
      `${state.currentPlayer.toUpperCase()}'s turn`;
    }
  }






  for (let row = 0; row < 8; row++) {
    for (let col = 0; col < 8; col++) {
      const cell = document.createElement("div");
      cell.className = "cell";
      cell.dataset.row = row;
      cell.dataset.col = col;
      //tambahkan event listener untuk klik
      cell.addEventListener("click", () => {
        if(currentValidMoves.some(move => move.row === row 
          && move.col === col)) 
        {
          debugDiv.innerHTML = `Valid move: [${row}, ${col}]`;
          //kirim koordinat ke server
          socket.emit("cellClick", { row: row, col: col });
        } else {
          debugDiv.innerHTML = `Invalid move: [${row},${col}] - not a valid move!`;
        }
      });

      board.appendChild(cell);
    }
  }

  socket.on('gameState', (state)=> {
    updateBoard(state.board, state.validMoves);
    updateGameInfo(state);
    console.log('State game:', state);
  });

  socket.on('invalidMove', (data) => {
    debugDiv.innerHTML = `INVALID MOVE: Cell [${data.row}, ${data.col}] sudah terisi!`;
  });

  document.getElementById('resetButton').addEventListener('click', () => {
    socket.emit('resetGame');
    debugDiv.innerHTML = 'Game reset!';
  });

});
