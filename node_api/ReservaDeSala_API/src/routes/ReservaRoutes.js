import express from 'express';
import {
    criarReserva,
    listarReservas,
    obterReservaPorId,
    listarMinhasReservas,
    deletarReserva
} from '../controllers/ReservaController.js';

const router = express.Router();

router.post('/', criarReserva);
router.get('/', listarReservas);
router.get('/minhas', listarMinhasReservas);
router.get('/:id', obterReservaPorId);
router.delete('/:id', deletarReserva);

export default router;