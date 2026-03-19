import express from 'express';

const app = express();
const port = 3000;

const windDirections = ["N", "NE", "E", "SE", "S", "SW", "W", "NW"];

app.get('/temperature', (req, res) => {
    const randomTemp = (Math.random() * (40 - (-10)) + (-10)).toFixed(1);
    res.json({ temperature: randomTemp });
});

app.get('/wind-direction', (req, res) => {
    const randomDirection = windDirections[Math.floor(Math.random() * windDirections.length)];
    res.json({ windDirection: randomDirection });
});

app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});