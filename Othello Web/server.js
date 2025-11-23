const express = require("express");
const http = require("http");
const socketIo = require("socket.io");

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

app.use(express.static("public"));

//menambahkan event listener untuk event 'cellClick' yang dikirim dari client.

io.on("connection", (socket) => {
  console.log("User Connected:", socket.id);
  //terima event cellClick dari client
  socket.on("cellClick", (data) => {
    console.log("cell clicked", data);
  });
});

server.listen(3000, () => {
  console.log("Server running on port 3000");
});
