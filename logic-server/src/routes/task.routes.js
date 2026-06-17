const express = require("express");
const router = express.Router();

const taskService = require("../services/task.service");

// GET all tasks
router.get("/", async (req, res) => {
    try {
        const skip = Number(req.query.skip || 0);
        const take = Number(req.query.take || 10);

        const data = await taskService.getTasks(skip, take);
        res.json(data);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// GET by id
router.get("/:id", async (req, res) => {
    try {
        const data = await taskService.getTaskById(req.params.id);
        res.json(data);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// CREATE
router.post("/", async (req, res) => {
    try {
        const result = await taskService.createTask(req.body);
        res.json(result);
    } catch (err) {
        res.status(400).json({ error: err.message });
    }
});

// UPDATE
router.put("/:id", async (req, res) => {
    try {
        const result = await taskService.updateTask(req.params.id, req.body);
        res.json(result);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// DELETE
router.delete("/:id", async (req, res) => {
    try {
        const result = await taskService.deleteTask(req.params.id);
        res.json(result);
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

module.exports = router;