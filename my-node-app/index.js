require("dotenv").config();
const express = require("express");
const axios = require("axios");

const app = express();
const PORT = process.env.PORT || 3000;


const RENDER_API_URL = "https://api.render.com/v1/services";
const API_KEY = process.env.RENDER_API_KEY


app.get("/", async (req, res) =>{
  try {
    const response = await axios.get(RENDER_API_URL, {
      headers: {
        Authorization: `Bearer ${API_KEY}`,
      },
    });

    res.json(response.data);
  } catch (error) {
    console.error("Error fetching data from Render API:", error);
    res.status(500).json({ error: "Failed to fetch data from Render API" });
  }
});

// הפעלת השרת
app.listen(PORT, () => {
  console.log(`🚀 Server running on http://localhost:${PORT}`);
});

