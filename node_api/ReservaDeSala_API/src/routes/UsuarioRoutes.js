const express = require('express');
const {
    criarUsuario,
    obterUsuarios,
    obterUsuarioPorId,
    atualizarUsuario,
    deletarUsuario
} = require('../controllers/UsuarioController');

const router = express.Router();

router.post('/usuarios', criarUsuario);
router.get('/usuarios', obterUsuarios);
router.get('/usuarios/:id', obterUsuarioPorId);
router.put('/usuarios/:id', atualizarUsuario);
router.delete('/usuarios/:id', deletarUsuario);


module.exports = router;