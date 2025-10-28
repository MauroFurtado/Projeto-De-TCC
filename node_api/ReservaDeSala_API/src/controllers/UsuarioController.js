import Usuario from '../models/UsuarioModel.js';

export const criarUsuario = async (req, res) => {
    try {
        const { nome, email, senha, perfil } = req.body;
        if (!nome || !email || !senha) return res.status(400).json({ error: 'Campos obrigatórios: nome, email, senha' });
        const novoUsuario = await Usuario.create({ nome, email, senha, perfil });
        return res.status(201).location(`/usuarios/${novoUsuario.id}`).json(novoUsuario);
    } catch (error) {
        console.error('Erro ao criar usuário:', error);
        if (error.name === 'SequelizeUniqueConstraintError') {
            return res.status(409).json({ error: 'Email já cadastrado' });
        }
        return res.status(500).json({ error: 'Erro ao criar usuário' });
    }
};

export const obterUsuarios = async (req, res) => {
    try {
        const usuarios = await Usuario.findAll();
        return res.status(200).json(usuarios);
    } catch (error) {
        console.error('Erro ao obter usuários:', error);
        return res.status(500).json({ error: 'Erro ao obter usuários' });
    }
};

export const obterUsuarioPorId = async (req, res) => {
    try {
        const { id } = req.params;
        const usuario = await Usuario.findByPk(id);
        if (!usuario) return res.status(404).json({ error: 'Usuário não encontrado' });
        return res.status(200).json(usuario);
    } catch (error) {
        console.error('Erro ao obter usuário por ID:', error);
        return res.status(500).json({ error: 'Erro ao obter usuário por ID' });
    }
};

export const atualizarUsuario = async (req, res) => {
    try {
        const { id } = req.params;
        const { nome, email, senha, perfil } = req.body;
        const usuario = await Usuario.findByPk(id);
        if (!usuario) return res.status(404).json({ error: 'Usuário não encontrado' });
        await usuario.update({ nome, email, senha, perfil });
        return res.status(200).json(usuario);
    } catch (error) {
        console.error('Erro ao atualizar usuário:', error);
        if (error.name === 'SequelizeUniqueConstraintError') {
            return res.status(409).json({ error: 'Email já cadastrado' });
        }
        return res.status(500).json({ error: 'Erro ao atualizar usuário' });
    }
};

export const deletarUsuario = async (req, res) => {
    try {
        const { id } = req.params;
        const linhasDeletadas = await Usuario.destroy({ where: { id } });
        if (!linhasDeletadas) return res.status(404).json({ error: 'Usuário não encontrado' });
        return res.sendStatus(204);
    } catch (error) {
        console.error('Erro ao deletar usuário:', error);
        return res.status(500).json({ error: 'Erro ao deletar usuário' });
    }
};
