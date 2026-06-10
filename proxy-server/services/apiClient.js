const axios = require("axios");

const API = process.env.BACKEND_URL;

const apiClient = axios.create({
  baseURL: API,
  headers: {
    "Content-Type": "application/json",
  },
});

module.exports = apiClient;