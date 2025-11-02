import express from 'express';
import {
    criarReserva,
    listarReservas,
    obterReservaPorId,
    listarMinhasReservas,
    atualizarReserva,
    deletarReserva
} from '../controllers/ReservaController.js';

const router = express.Router();

router.post('/', criarReserva);
router.get('/', listarReservas);
router.put('/:id', atualizarReserva);
router.get('/minhas/:usuarioId', listarMinhasReservas);

router.get('/:id', obterReservaPorId);
router.delete('/:id', deletarReserva);

export default router;