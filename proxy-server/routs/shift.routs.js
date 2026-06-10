const express = require("express");
const router = express.Router();
const apiClient = require("../services/apiClient");

// קבלת משמרות
router.get("/", async (req, res) => {
  try {
    const response = await apiClient.get("/api/shift");
    res.json(response.data);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// יצירה
router.post("/", async (req, res) => {
  try {
    const response = await apiClient.post("/api/shift", req.body);
    res.json(response.data);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;