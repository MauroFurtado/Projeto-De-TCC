import express from 'express';
import {
    criarUsuario,
    obterUsuarios,
    obterUsuarioPorId,
    atualizarUsuario,
    deletarUsuario
} from '../controllers/UsuarioController.js';

const router = express.Router();

// Definição das rotas relativas — o prefixo '/usuarios' vem do app.js
router.post('/', criarUsuario); 
router.get('/', obterUsuarios);
router.get('/:id', obterUsuarioPorId);
router.put('/:id', atualizarUsuario);
router.delete('/:id', deletarUsuario);

export default router;