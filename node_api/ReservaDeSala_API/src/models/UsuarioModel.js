import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Usuario = sequelize.define('Usuario', {
  id: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true,
    field: 'id'
  },
  nome: {
    type: DataTypes.STRING,
    allowNull: false,
    field: 'nome'
  },
  email: {
    type: DataTypes.STRING,
    allowNull: false,
    unique: true,
    field: 'email'
  },
  senha: {
    type: DataTypes.STRING,
    allowNull: false,
    field: 'senha'
  },
  perfil: {
    type: DataTypes.ENUM('admin', 'comum'),
    allowNull: false,
    defaultValue: 'comum',
    field: 'perfil'
  },
  criado_em: {
    type: DataTypes.DATE,
    allowNull: false,
    defaultValue: DataTypes.NOW,
    field: 'criado_em'
  }
}, {
  tableName: 'usuarios', 
  timestamps: false
});

// associações
Usuario.associate = (models) => {
  if (models.Reserva) {
    Usuario.hasMany(models.Reserva, { foreignKey: 'usuario_id', as: 'Reservas' });
  }
};

export default Usuario;