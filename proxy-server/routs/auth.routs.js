const express = require("express");
const router = express.Router();
const apiClient = require("../services/apiClient");

// login
router.post("/login", async (req, res) => {
  try {
    const response = await apiClient.post("/api/auth/login", req.body);
    res.json(response.data);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;