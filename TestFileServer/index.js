const express = require("express");
const fs = require("fs");
const app = express();

app.get("/api/models/:userId/:fileId",(req,res)=>{
    
    let filePath = `./files/${req.params.userId}/${req.params.fileId}.smdl`;
    let stat = fs.statSync(filePath);
    console.log(filePath);
    res.writeHead(200, {
        'Content-Type': 'application/octet-stream',
        'Content-Length': stat.size
    });

    let fileStream = fs.createReadStream(filePath);
    fileStream.pipe(res);
})

app.listen(5000, () => {
});