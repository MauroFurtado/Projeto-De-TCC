import { Op } from 'sequelize';
import Reserva from "../models/ReservaModel.js";

export const criarReserva = async (req, res) => {
    try {
        const sala_id = req.body.salaId ?? req.body.sala_id;
        const usuario_id = req.body.usuarioId ?? req.body.usuario_id;
        const data_reserva = req.body.dataReserva ?? req.body.data_reserva;
        const hora_inicio = req.body.horaInicio ?? req.body.hora_inicio;
        const hora_fim = req.body.horaFim ?? req.body.hora_fim;

        if (!sala_id || !usuario_id || !data_reserva || !hora_inicio || !hora_fim) {
            return res.status(400).json({ error: 'Campos obrigatórios ausentes' });
        }

        const conflito = await Reserva.findOne({
            where: {
                sala_id,
                data_reserva,
                [Op.and]: [
                    { hora_inicio: { [Op.lt]: hora_fim } },
                    { hora_fim: { [Op.gt]: hora_inicio } }
                ]
            }
        });

        if (conflito) {
            return res.status(409).json({ error: 'Conflito de horário: já existe uma reserva nesse intervalo' });
        }

        const novaReserva = await Reserva.create({ sala_id, usuario_id, data_reserva, hora_inicio, hora_fim });
        return res.status(201).location(`/reservas/${novaReserva.id}`).json(novaReserva);
    } catch (error) {
        console.error('Erro ao criar reserva:', error);
        return res.status(500).json({ error: 'Erro ao criar reserva' });
    }
};

export const listarReservas = async (req, res) => {
    try {
        const reservas = await Reserva.findAll();
        return res.status(200).json(reservas);
    } catch (error) {
        console.error('Erro ao obter reservas', error);
        return res.status(500).json({ error: 'Erro ao obter reservas' });
    }
};

export const obterReservaPorId = async (req, res) => {
    try {
        const { id } = req.params;
        const reserva = await Reserva.findByPk(id);
        if (!reserva) return res.status(404).json({ error: 'Reserva não encontrada' });
        return res.status(200).json(reserva);
    } catch (error) {
        console.error('Erro ao obter reserva por ID:', error);
        return res.status(500).json({ error: 'Erro ao obter reserva por ID' });
    }
};
export const atualizarReserva = async (req, res) => {
    try {
        const { id } = req.params;
        const reserva = await Reserva.findByPk(id);
        if (!reserva) return res.status(404).json({ error: 'Reserva não encontrada' });
        const { sala_id, usuario_id, data_reserva, hora_inicio, hora_fim } = req.body;
        await reserva.update({ sala_id, usuario_id, data_reserva, hora_inicio, hora_fim });
        return res.status(200).json(reserva);
    } catch (error) {
        console.error('Erro ao atualizar reserva:', error);
        return res.status(500).json({ error: 'Erro ao atualizar reserva' });
    }
};
export const listarMinhasReservas = async (req, res) => {
    try {
        const usuario_id = req.params.usuarioId ?? req.params.usuario_id;
        if (!usuario_id) return res.status(400).json({ error: 'usuario_id é obrigatório' });
        const reservas = await Reserva.findAll({ where: { usuario_id } });
        return res.status(200).json(reservas);
    } catch (error) {
        console.error('Erro ao obter reservas do usuário:', error);
        return res.status(500).json({ error: 'Erro ao obter reservas do usuário' });
    }
};

export const deletarReserva = async (req, res) => {
    try {
        const { id } = req.params;
        const reserva = await Reserva.findByPk(id);
        if (!reserva) return res.status(404).json({ error: 'Reserva não encontrada' });
        await reserva.destroy();
        return res.sendStatus(204);
    } catch (error) {
        console.error('Erro ao deletar reserva:', error);
        return res.status(500).json({ error: 'Erro ao deletar reserva' });
    }
};
