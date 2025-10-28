import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Sala = sequelize.define('Sala', {
    id: {
        type: DataTypes.INTEGER,
        primaryKey: true,
        autoIncrement: true
    },
    nome: {
        type: DataTypes.STRING,
        allowNull: false
    },
    tipo: {
        type: DataTypes.ENUM('laboratorio', 'aula', 'reuniao', 'auditório'),
        allowNull: false
    },
    capacidade: {
        type: DataTypes.INTEGER,
        allowNull: false,
        validate: { min: 1 }
    },
    localizacao: {
        type: DataTypes.STRING,
        allowNull: true
    },
    disponivel: {
        type: DataTypes.BOOLEAN,
        allowNull: false,
        defaultValue: true
    },
    criado_em: {
        type: DataTypes.DATE,
        allowNull: false,
        defaultValue: DataTypes.NOW
    }
}, {
    tableName: 'salas',
    timestamps: false
});
// associações
Sala.associate = (models) => {
    if (models.Reserva) {
        Sala.hasMany(models.Reserva, { foreignKey: 'sala_id', as: 'reservas' });
    }
};

export default Sala;