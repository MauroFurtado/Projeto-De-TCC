import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Reserva = sequelize.define('Reserva', {
    id: {
        type: DataTypes.INTEGER,
        primaryKey: true,
        autoIncrement: true
    },
    usuario_id: {
        type: DataTypes.INTEGER,
        allowNull: false,
        references: { model: 'usuarios', key: 'id' }
    },
    sala_id: {
        type: DataTypes.INTEGER,
        allowNull: false,
        references: { model: 'salas', key: 'id' }
    },
    data_reserva: {
        type: DataTypes.DATEONLY,
        allowNull: false
    },
    hora_inicio: {
        type: DataTypes.TIME,
        allowNull: false
    },
    hora_fim: {
        type: DataTypes.TIME,
        allowNull: false
    },
    criado_em: {
        type: DataTypes.DATE,
        allowNull: false,
        defaultValue: DataTypes.NOW
    }
}, {
    tableName: 'reservas',
    timestamps: false,
    indexes: [
        {
            name: 'reserva_unica',
            unique: true,
            fields: ['sala_id', 'data_reserva', 'hora_inicio', 'hora_fim']
        },
        { name: 'idx_reservas_usuario', fields: ['usuario_id'] },
        { name: 'idx_reservas_sala', fields: ['sala_id'] }
    ],
    validate: {
        horaFimMaiorQueInicio() {
            const toSeconds = (t) => {
                if (!t) return null;
                const parts = t.split(':').map(Number);
                return (parts[0] || 0) * 3600 + (parts[1] || 0) * 60 + (parts[2] || 0);
            };
            const hi = toSeconds(this.hora_inicio?.toString());
            const hf = toSeconds(this.hora_fim?.toString());
            if (hi != null && hf != null && hf <= hi) {
                throw new Error('hora_fim deve ser maior que hora_inicio');
            }
        }
    }
});

// associações
Reserva.associate = (models) => {
    if (models.Usuario) {
        Reserva.belongsTo(models.Usuario, { foreignKey: 'usuario_id', as: 'usuario' });
    }
    if (models.Sala) {
        Reserva.belongsTo(models.Sala, { foreignKey: 'sala_id', as: 'sala' });
    }
};

export default Reserva;