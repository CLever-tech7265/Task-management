require("dotenv").config();

const express = require("express");

const app = express();

app.use(express.json());

// routes
app.use("/api/task", require("./routes/task.routes"));

const PORT = process.env.PORT;

app.listen(PORT, () => {
    console.log(`Node server running on port ${PORT}`);
});