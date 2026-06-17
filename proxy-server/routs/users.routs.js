const express = require("express");
const router = express.Router();
const apiClient = require("../services/apiClient");

// קבלת משתמשים
router.get("/", async (req, res) => {
  try {
    const response = await apiClient.get("/api/auth/users");
    res.json(response.data);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// מחיקת משתמש
router.delete("/:id", async (req, res) => {
  try {
    const response = await apiClient.delete(`/api/auth/users/${req.params.id}`);
    res.json(response.data);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;