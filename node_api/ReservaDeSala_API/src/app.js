import express from 'express';
import dotenv from 'dotenv';
import UsuarioRoute from './routes/UsuarioRoutes.js';
import SalaRoute from './routes/SalaRoutes.js';
import ReservaRoute from './routes/ReservaRoutes.js';


dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes (mount)
app.use('/usuarios', UsuarioRoute);
app.use('/salas', SalaRoute);
app.use('/reservas', ReservaRoute);

// Start the server
app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});