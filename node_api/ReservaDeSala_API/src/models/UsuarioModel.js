import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Usuario = sequelize.define('Usuario', {
    id: {
        type: DataTypes.INTEGER,
        primaryKey: true,
        autoIncrement: true
    },
    nome: {
        type: DataTypes.STRING,
        allowNull: false
    },
    email: {
        type: DataTypes.STRING,
        allowNull: false,
        unique: true
    },
    senha: {
        type: DataTypes.STRING,
        allowNull: false
    },
    perfil: {
        type: DataTypes.ENUM('admin', 'comum'),
        allowNull: false,
        defaultValue: 'comum'
    },
    criado_em: {
        type: DataTypes.DATE,
        allowNull: false,
        defaultValue: DataTypes.NOW
    }
}, {
    tableName: 'usuarios',
    timestamps: false
});

// associações (chamar Usuario.associate(models) após carregar todos os modelos)
Usuario.associate = (models) => {
    if (models.Reserva) {
        Usuario.hasMany(models.Reserva, { foreignKey: 'usuario_id', as: 'reservas' });
    }
};

export default Usuario;