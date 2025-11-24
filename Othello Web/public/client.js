document.addEventListener("DOMContentLoaded", () => {
  const board = document.getElementById('board');
  const currentPlayerSpan = document.getElementById('currentPlayerSpan');
  const debugDiv = document.getElementById('debug');
  const socket = io(); // Terhubung ke server


  function updateBoard(boardState)
  {
    const cells = document.querySelectorAll('.cell');
    cells.forEach (cell => {
      const row = parseInt(cell.dataset.row);
      const col = parseInt(cell.dataset.col);
      const piece = boardState[row][col];

      const existingPiece = cell.querySelector('.piece');
      if (existingPiece) {
        existingPiece.remove();
      }

      if (piece) {
        const pieceDiv = document.createElement('div');
        pieceDiv.className = `piece ${piece}`;
        cell.appendChild(pieceDiv);
      }

    });
  }






  for (let row = 0; row < 8; row++) {
    for (let col = 0; col < 8; col++) {
      const cell = document.createElement("div");
      cell.className = "cell";
      cell.dataset.row = row;
      cell.dataset.col = col;
      //tambahkan event listener untuk klik
      cell.addEventListener("click", () => {
        debugDiv.innerHTML = `Terakhir di klik: row: ${row}, col: ${col} `;
        //kirim koordinat ke server
        socket.emit("cellClick", { row: row, col: col });
      });

      board.appendChild(cell);
    }
  }

  socket.on('boardUpdate', (data) => {
    console.log('menerima Update dari server:', data);
    updateBoard(data.board);
    currentPlayerSpan.textContent = data.currentPlayer;
    currentPlayerSpan.className = data.currentPlayer;
  });

  socket.on('gameState', (state)=> {
    updateBoard(state.board);
    currentPlayerSpan.textContent = state.currentPlayer;
    currentPlayerSpan.className = state.currentPlayer;
    console.log('State game:', state);
  });

  socket.on('invalidMove', (data) => {
    debugDiv.innerHTML = `INVALID MOVE: Cell [${data.row}, ${data.col}] sudah terisi!`;
  });

});
