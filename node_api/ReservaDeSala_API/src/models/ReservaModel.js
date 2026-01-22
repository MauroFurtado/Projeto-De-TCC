import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';

const Reserva = sequelize.define('Reserva', {
  id: { type: DataTypes.INTEGER, primaryKey: true, autoIncrement: true, field: 'id' },
  usuario_id: { type: DataTypes.INTEGER, allowNull: false, field: 'usuario_id' },
  sala_id: { type: DataTypes.INTEGER, allowNull: false, field: 'sala_id' },
  data_reserva: { type: DataTypes.DATEONLY, allowNull: false, field: 'data_reserva' },
  hora_inicio: { type: DataTypes.TIME, allowNull: false, field: 'hora_inicio' },
  hora_fim: { type: DataTypes.TIME, allowNull: false, field: 'hora_fim' },
  criado_em: { type: DataTypes.DATE, defaultValue: DataTypes.NOW, field: 'criado_em' }
}, {
  tableName: 'reservas',
  timestamps: false
});

// associações
Reserva.associate = (models) => {
  if (models.Usuario) {
    Reserva.belongsTo(models.Usuario, { foreignKey: 'usuario_id', as: 'Usuario' });
  }
  if (models.Sala) {
    Reserva.belongsTo(models.Sala, { foreignKey: 'sala_id', as: 'Sala' });
  }
};

export default Reserva;