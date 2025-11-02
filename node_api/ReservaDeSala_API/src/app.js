import express from 'express';
import dotenv from 'dotenv';
import UsuarioRoute from './routes/UsuarioRoutes.js';
import SalaRoute from './routes/SalaRoutes.js';
import ReservaRoute from './routes/ReservaRoutes.js';
import sequelize from './config/db.js';


dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.use('/usuarios', UsuarioRoute);
app.use('/salas', SalaRoute);
app.use('/reservas', ReservaRoute);

// Health endpoint que verifica conexão com o banco
app.get('/health', async (req, res) => {
    try {
        await sequelize.authenticate();
        return res.status(200).json({ status: 'ok', db: 'connected' });
    } catch (err) {
        console.error('Healthcheck DB failed:', err.message || err);
        return res.status(503).json({ status: 'error', db: 'disconnected', message: err.message });
    }
});

// Start the server somente após confirmar conexão com o DB (tentativas com backoff)
const start = async () => {
    const maxAttempts = 8;
    const delay = ms => new Promise(r => setTimeout(r, ms));

    for (let attempt = 1; attempt <= maxAttempts; attempt++) {
        try {
            await sequelize.authenticate();
            console.log('Conectado ao banco de dados');

            if (process.env.DB_SYNC === 'true') {
                await sequelize.sync({ alter: true });
                console.log('Modelos sincronizados (sequelize.sync)');
            }

            app.listen(PORT, () => {
                console.log(`Server is running on port ${PORT}`);
            });
            return;
        } catch (err) {
            console.error(`Tentativa ${attempt} - erro conectando ao DB:`, err.message || err);
            if (attempt === maxAttempts) {
                console.error('Não foi possível conectar ao banco após várias tentativas. Encerrando.');
                process.exit(1);
            }
            await delay(2000 * attempt);
        }
    }
};

start();