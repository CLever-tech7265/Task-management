const axios = require("axios");

const BASE_URL = process.env.BASE_URL;

// GET all
async function getTasks(skip = 0, take = 10) {
    const res = await axios.get(BASE_URL, {
        params: { skip, take }
    });

    return res.data;
}

// GET by id
async function getTaskById(id) {
    const res = await axios.get(`${BASE_URL}/${id}`);
    return res.data;
}

// CREATE
async function createTask(data) {
    const res = await axios.post(BASE_URL, data);
    return res.data;
}

// UPDATE
async function updateTask(id, data) {
    const res = await axios.put(`${BASE_URL}/${id}`, data);
    return res.data;
}

// DELETE
async function deleteTask(id) {
    const res = await axios.delete(`${BASE_URL}/${id}`);
    return res.data;
}

module.exports = {
    getTasks,
    getTaskById,
    createTask,
    updateTask,
    deleteTask
};