import Sala from "../models/SalasModel.js";
import Reserva from "../models/ReservaModel.js";

export const cadastrarSala = async (req, res) => {
    try {
        const { nome, tipo, capacidade, localizacao, disponivel } = req.body;
        if (!nome || !tipo || !capacidade) {
            return res.status(400).json({ error: 'Campos obrigatórios: nome, tipo, capacidade' });
        }
        const novaSala = await Sala.create({ nome, tipo, capacidade, localizacao, disponivel });
        return res.status(201).location(`/salas/${novaSala.id}`).json(novaSala);
    } catch (error) {
        console.error('Erro ao criar sala:', error);
        return res.status(500).json({ error: 'Erro ao criar sala' });
    }
};

export const listarSalas = async (req, res) => {
    try {
        const salas = await Sala.findAll();
        return res.status(200).json(salas);
    } catch (error) {
        console.error('Erro ao obter salas:', error);
        return res.status(500).json({ error: 'Erro ao obter salas' });
    }
};

export const obterSalaPorId = async (req, res) => {
    try {
        const { id } = req.params;
        const sala = await Sala.findByPk(id);
        if (!sala) return res.status(404).json({ error: 'Sala não encontrada' });
        return res.status(200).json(sala);
    } catch (error) {
        console.error('Erro ao obter sala por ID:', error);
        return res.status(500).json({ error: 'Erro ao obter sala por ID' });
    }
};

export const atualizarSala = async (req, res) => {
    try {
        const { id } = req.params;
        const { nome, tipo, capacidade, localizacao, disponivel } = req.body;
        const sala = await Sala.findByPk(id);
        if (!sala) return res.status(404).json({ error: 'Sala não encontrada' });
        await sala.update({ nome, tipo, capacidade, localizacao, disponivel });
        return res.status(200).json(sala);
    } catch (error) {
        console.error('Erro ao atualizar sala:', error);
        return res.status(500).json({ error: 'Erro ao atualizar sala' });
    }
};

export const deletarSala = async (req, res) => {
    try {
        const { id } = req.params;
        const sala = await Sala.findByPk(id);

        if (!sala) return res.status(404).json({ error: 'Sala não encontrada' });
        const temReservas = await Reserva.findOne({ where: { sala_id: id } });
        if (temReservas) {
            return res.status(409).json({ error: 'Não é possível deletar sala com reservas existentes' });
        }

        await sala.destroy();
        return res.sendStatus(204);
    } catch (error) {
        console.error('Erro ao deletar sala:', error);
        return res.status(500).json({ error: 'Erro ao deletar sala' });
    }
};
