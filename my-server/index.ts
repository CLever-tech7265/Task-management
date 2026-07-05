import express from 'express';
import fetch from 'node-fetch';

const app = express();
const PORT = 3000;

app.use(express.json());

app.get('/api/employees', async (req, res) => {
  try {
    // קריאה ל-C# API
    const response = await fetch('https://localhost:3000/api/employees');
    const data = await response.json();
    res.json(data);
  } catch (err) {
    res.status(500).json({ error: 'Failed to fetch from C# API', details: err });
  }
});

app.listen(PORT, () => {
  console.log(`Node server running on http://localhost:${PORT}`);
});