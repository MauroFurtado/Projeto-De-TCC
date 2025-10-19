import express from 'express';
const router = express.Router();

import {
    criarUsuario,
    obterUsuarios,
    obterUsuarioPorId,
    atualizarUsuario,
    deletarUsuario
} from '../controllers/UsuarioController.js';

// Definição as rotas para os usuários
router.post('/usuarios', criarUsuario);
router.get('/usuarios', obterUsuarios);
router.get('/usuarios/:id', obterUsuarioPorId);
router.put('/usuarios/:id', atualizarUsuario);
router.delete('/usuarios/:id', deletarUsuario);

export default router;