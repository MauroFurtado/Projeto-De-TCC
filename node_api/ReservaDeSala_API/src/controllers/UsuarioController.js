import Usuario from '../models/UsuarioModel.js';

export const criarUsuario=async(req, res) => {
    try {
        const { email, senha } = req.body;
        const novoUsuario = await Usuario.create({ email, senha });
        res.status(201).json(novoUsuario);
    } catch (error) {
        console.error('Erro ao criar usuário:', error);
        res.status(500).json({ error: 'Erro ao criar usuário' });
    }
};

export const obterUsuarios = async(req, res) => {
    try {
        const usuarios = await Usuario.findAll();
        res.json(usuarios); 
    } catch (error) {
        console.error('Erro ao obter usuários:', error);
        res.status(500).json({ error: 'Erro ao obter usuários' });
    }
};

export const obterUsuarioPorId = async(req, res) => {
    try {
        const { id } = req.params;
        const usuario = await Usuario.findByPk(id);
        if (usuario) {
            res.json(usuario);
        } else {
            res.status(404).json({ error: 'Usuário não encontrado' });
        }
    } catch (error) {
        console.error('Erro ao obter usuário por ID:', error);
        res.status(500).json({ error: 'Erro ao obter usuário por ID' });
    }
};
export const atualizarUsuario = async(req, res) => {
    try {
        const { id } = req.params;
        const { email, senha } = req.body;
        const [linhasAtualizadas] = await Usuario.update({ email, senha }, { where: { id } });
        if (linhasAtualizadas) {
            const usuarioAtualizado = await Usuario.findByPk(id);
            res.json(usuarioAtualizado);
        }   else {
            res.status(404).json({ error: 'Usuário não encontrado' });
        }
    } catch (error) {
        console.error('Erro ao atualizar usuário:', error);
        res.status(500).json({ error: 'Erro ao atualizar usuário' });
    }
};
export const deletarUsuario = async(req, res) => {
    try {
        const { id } = req.params;
        const linhasDeletadas = await Usuario.destroy({ where: { id } });
        if (linhasDeletadas) {
            res.status(204).send();
        } else {
            res.status(404).json({ error: 'Usuário não encontrado' });
        }
    } catch (error) {
        console.error('Erro ao deletar usuário:', error);
        res.status(500).json({ error: 'Erro ao deletar usuário' });
    }
};
