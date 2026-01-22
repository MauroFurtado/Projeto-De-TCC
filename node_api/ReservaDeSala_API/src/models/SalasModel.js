import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Sala = sequelize.define('Sala', {
  id: { type: DataTypes.INTEGER, primaryKey: true, autoIncrement: true, field: 'id' },
  nome: { type: DataTypes.STRING, allowNull: false, field: 'nome' },
  tipo: { type: DataTypes.ENUM('laboratorio','aula','reuniao','auditório'), allowNull: false, field: 'tipo' },
  capacidade: { type: DataTypes.INTEGER, allowNull: false, field: 'capacidade' },
  localizacao: { type: DataTypes.STRING, field: 'localizacao' },
  disponivel: { type: DataTypes.BOOLEAN, defaultValue: true, field: 'disponivel' },
  criado_em: { type: DataTypes.DATE, defaultValue: DataTypes.NOW, field: 'criado_em' }
}, {
  tableName: 'salas',
  timestamps: false
});

Sala.associate = (models) => {
  if (models.Reserva) {
    Sala.hasMany(models.Reserva, { foreignKey: 'sala_id', as: 'Reservas' });
  }
};

export default Sala;