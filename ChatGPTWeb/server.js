// モジュール準備
const express = require('express');
const multer = require('multer');
const fs = require('fs');

const app = express();
app.use(multer().none());

// 静的コンテンツ（HTMLファイル）の返却準備
app.use(express.static('public'));

// サーバーを起動する
const port = 3000;
app.listen(port, function () {
  console.log('Node.js Server Started: http://localhost:' + port);
});
