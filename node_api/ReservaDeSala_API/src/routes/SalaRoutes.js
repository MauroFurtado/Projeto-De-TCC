import express from 'express';
import {
  cadastrarSala,
  listarSalas,
  obterSalaPorId,
  atualizarSala,
  deletarSala
} from '../controllers/SalaController.js';

const router = express.Router();

router.post('/', cadastrarSala);
router.get('/', listarSalas);
router.get('/:id', obterSalaPorId);
router.put('/:id', atualizarSala);
router.delete('/:id', deletarSala);

export default router;