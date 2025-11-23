document.addEventListener("DOMContentLoaded", () => {
  const board = document.getElementById("board");
  const socket = io(); // Terhubung ke server

  for (let row = 0; row < 8; row++) {
    for (let col = 0; col < 8; col++) {
      const cell = document.createElement("div");
      cell.className = "cell";
      cell.dataset.row = row;
      cell.dataset.col = col;
      //tambahkan event listener untuk klik
      cell.addEventListener("click", () => {
        //kirim koordinat ke server
        socket.emit("cellClick", { row: row, col: col });
      });

      board.appendChild(cell);
    }
  }
});
