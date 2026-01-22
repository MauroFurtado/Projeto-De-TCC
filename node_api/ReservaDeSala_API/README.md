# API Node.js - Sistema de Reserva de Salas

## 📝 Descrição

Implementação da API de Reserva de Salas utilizando **Node.js 20+** com Express.js e Sequelize ORM, seguindo padrões MVC e princípios de código limpo.

## 🏗️ Arquitetura

### Estrutura do Projeto

```
ReservaDeSala_API/
├── src/
│   ├── app.js                    # Aplicação Express principal
│   ├── config/
│   │   └── db.js                 # Configuração Sequelize
│   ├── controllers/              # Controladores (Lógica de negócio)
│   │   ├── ReservaController.js
│   │   ├── SalaController.js
│   │   └── UsuarioController.js
│   ├── models/                   # Modelos Sequelize (ORM)
│   │   ├── ReservaModel.js
│   │   ├── SalasModel.js
│   │   └── UsuarioModel.js
│   ├── routes/                   # Rotas da API
│   │   ├── ReservaRoutes.js
│   │   ├── SalaRoutes.js
│   │   └── UsuarioRoutes.js
│   └── middleware/               # Middlewares
│       └── auth.js               # Autenticação JWT
├── Dockerfile
├── package.json
└── README.md
```

## 🛠️ Tecnologias e Pacotes

### Dependências (package.json)

```json
{
  "dependencies": {
    "bcryptjs": "^3.0.3",
    "dotenv": "^10.0.0",
    "express": "^4.17.1",
    "jsonwebtoken": "^9.0.3",
    "pg": "^8.10.0",
    "pg-hstore": "^2.3.4",
    "sequelize": "^6.37.7"
  },
  "devDependencies": {
    "nodemon": "^2.0.7"
  }
}
```

### Frameworks e Bibliotecas

- **Node.js 20+**: Runtime JavaScript
- **Express.js 4**: Framework web minimalista
- **Sequelize 6**: ORM para Node.js
- **PostgreSQL (pg)**: Driver PostgreSQL
- **bcryptjs**: Hash de senhas
- **jsonwebtoken**: Autenticação JWT
- **dotenv**: Variáveis de ambiente

## ⚙️ Configuração

### Arquivo .env

Crie um arquivo `.env` na raiz do projeto:

```env
# Server
PORT=3030
NODE_ENV=development

# Database
DB_HOST=localhost
DB_PORT=5432
DB_NAME=ReservaSalas
DB_USER=MauroFurtado
DB_PASSWORD=

# JWT
JWT_SECRET=seu-super-secreto-jwt-aqui-mudar-em-producao
JWT_EXPIRES_IN=24h

# Application
API_VERSION=v1
```

### Configuração do Banco (src/config/db.js)

```javascript
import { Sequelize } from 'sequelize';
import dotenv from 'dotenv';

dotenv.config();

const sequelize = new Sequelize(
  process.env.DB_NAME,
  process.env.DB_USER,
  process.env.DB_PASSWORD,
  {
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    dialect: 'postgres',
    logging: process.env.NODE_ENV === 'development' ? console.log : false,
    pool: {
      max: 5,
      min: 0,
      acquire: 30000,
      idle: 10000
    }
  }
);

export default sequelize;
```

## 🚀 Executando o Projeto

### Requisitos

- Node.js 20+ (recomendado usar nvm)
- PostgreSQL 16+
- npm ou yarn

### Modo Desenvolvimento

```bash
# Instalar dependências
npm install

# Executar em modo desenvolvimento (com nodemon)
npm run dev

# Executar em modo produção
npm start
```

### Docker

```bash
# Build da imagem
docker build -t reserva-node-api .

# Executar container
docker run -p 3001:3030 \
  -e DB_HOST=postgres \
  -e DB_NAME=ReservaSalas \
  -e DB_USER=MauroFurtado \
  -e DB_PASSWORD=MGFurtad0 \
  reserva-node-api
```

### Com Docker Compose (Recomendado)

```bash
# Do diretório raiz do projeto
docker-compose up -d node_api
```

## 📡 Endpoints

### Base URL
```
http://localhost:3001/api
```

### Autenticação

#### POST /api/auth/login
```javascript
// Request
{
  "email": "usuario@exemplo.com",
  "senha": "senha123"
}

// Response 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "usuario@exemplo.com",
    "perfil": "comum"
  }
}
```

#### POST /api/auth/register
```javascript
// Request
{
  "nome": "Novo Usuário",
  "email": "novo@exemplo.com",
  "senha": "senha123",
  "perfil": "comum"
}

// Response 201 Created
{
  "message": "Usuário criado com sucesso",
  "usuario": {
    "id": 5,
    "nome": "Novo Usuário",
    "email": "novo@exemplo.com",
    "perfil": "comum"
  }
}
```

### Usuários

#### GET /api/usuarios
```javascript
// Headers
Authorization: Bearer {token}

// Response 200 OK
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "perfil": "admin",
    "criadoEm": "2026-01-20T10:00:00.000Z"
  }
]
```

#### GET /api/usuarios/:id
```javascript
// Response 200 OK
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "perfil": "admin",
  "criadoEm": "2026-01-20T10:00:00.000Z"
}
```

#### POST /api/usuarios
```javascript
// Request (Requer perfil admin)
{
  "nome": "Maria Santos",
  "email": "maria@exemplo.com",
  "senha": "senha123",
  "perfil": "comum"
}

// Response 201 Created
{
  "id": 2,
  "nome": "Maria Santos",
  "email": "maria@exemplo.com",
  "perfil": "comum",
  "criadoEm": "2026-01-22T14:30:00.000Z"
}
```

#### PUT /api/usuarios/:id
```javascript
// Request
{
  "nome": "Maria Santos Silva",
  "email": "maria.silva@exemplo.com"
}

// Response 200 OK
{
  "id": 2,
  "nome": "Maria Santos Silva",
  "email": "maria.silva@exemplo.com",
  "perfil": "comum"
}
```

#### DELETE /api/usuarios/:id
```javascript
// Response 200 OK
{
  "message": "Usuário deletado com sucesso"
}
```

### Salas

#### GET /api/salas
```javascript
// Response 200 OK
[
  {
    "id": 1,
    "nome": "Sala 101",
    "tipo": "aula",
    "capacidade": 30,
    "localizacao": "Bloco A",
    "disponivel": true,
    "criadoEm": "2026-01-15T08:00:00.000Z"
  }
]
```

#### POST /api/salas
```javascript
// Request
{
  "nome": "Laboratório 205",
  "tipo": "laboratorio",
  "capacidade": 25,
  "localizacao": "Bloco B"
}

// Response 201 Created
{
  "id": 5,
  "nome": "Laboratório 205",
  "tipo": "laboratorio",
  "capacidade": 25,
  "localizacao": "Bloco B",
  "disponivel": true
}
```

### Reservas

#### GET /api/reservas
```javascript
// Response 200 OK
[
  {
    "id": 1,
    "usuarioId": 1,
    "salaId": 5,
    "dataReserva": "2026-02-10",
    "horaInicio": "10:00:00",
    "horaFim": "12:00:00",
    "criadoEm": "2026-01-22T14:40:00.000Z",
    "usuario": {
      "id": 1,
      "nome": "João Silva"
    },
    "sala": {
      "id": 5,
      "nome": "Laboratório 205"
    }
  }
]
```

#### POST /api/reservas
```javascript
// Request
{
  "usuarioId": 1,
  "salaId": 5,
  "dataReserva": "2026-02-10",
  "horaInicio": "10:00",
  "horaFim": "12:00"
}

// Response 201 Created
{
  "id": 10,
  "usuarioId": 1,
  "salaId": 5,
  "dataReserva": "2026-02-10",
  "horaInicio": "10:00:00",
  "horaFim": "12:00:00"
}
```

## 🧪 Testes

### Configurar Testes

```bash
# Instalar dependências de teste
npm install --save-dev jest supertest

# Adicionar script no package.json
"scripts": {
  "test": "jest",
  "test:watch": "jest --watch",
  "test:coverage": "jest --coverage"
}
```

### Exemplo de Teste

```javascript
// tests/usuario.test.js
import request from 'supertest';
import app from '../src/app';

describe('Usuario API', () => {
  it('deve listar todos os usuários', async () => {
    const response = await request(app)
      .get('/api/usuarios')
      .set('Authorization', `Bearer ${token}`);
    
    expect(response.status).toBe(200);
    expect(Array.isArray(response.body)).toBe(true);
  });

  it('deve criar novo usuário', async () => {
    const novoUsuario = {
      nome: 'Teste',
      email: 'teste@exemplo.com',
      senha: 'senha123',
      perfil: 'comum'
    };

    const response = await request(app)
      .post('/api/usuarios')
      .set('Authorization', `Bearer ${adminToken}`)
      .send(novoUsuario);
    
    expect(response.status).toBe(201);
    expect(response.body).toHaveProperty('id');
  });
});
```

## 🔧 Desenvolvimento

### Estrutura de um Modelo Sequelize

```javascript
// src/models/UsuarioModel.js
import { DataTypes } from 'sequelize';
import sequelize from '../config/db.js';
import bcrypt from 'bcryptjs';

const Usuario = sequelize.define('Usuario', {
  id: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true
  },
  nome: {
    type: DataTypes.STRING(255),
    allowNull: false
  },
  email: {
    type: DataTypes.STRING(255),
    allowNull: false,
    unique: true,
    validate: {
      isEmail: true
    }
  },
  senha: {
    type: DataTypes.TEXT,
    allowNull: false
  },
  perfil: {
    type: DataTypes.ENUM('admin', 'comum'),
    defaultValue: 'comum'
  },
  criadoEm: {
    type: DataTypes.DATE,
    defaultValue: DataTypes.NOW,
    field: 'criado_em'
  }
}, {
  tableName: 'usuarios',
  timestamps: false,
  hooks: {
    beforeCreate: async (usuario) => {
      if (usuario.senha) {
        usuario.senha = await bcrypt.hash(usuario.senha, 10);
      }
    },
    beforeUpdate: async (usuario) => {
      if (usuario.changed('senha')) {
        usuario.senha = await bcrypt.hash(usuario.senha, 10);
      }
    }
  }
});

// Métodos de instância
Usuario.prototype.verificarSenha = async function(senha) {
  return await bcrypt.compare(senha, this.senha);
};

export default Usuario;
```

### Estrutura de um Controller

```javascript
// src/controllers/UsuarioController.js
import Usuario from '../models/UsuarioModel.js';

class UsuarioController {
  // GET /api/usuarios
  async index(req, res) {
    try {
      const usuarios = await Usuario.findAll({
        attributes: { exclude: ['senha'] }
      });
      return res.json(usuarios);
    } catch (error) {
      return res.status(500).json({ error: error.message });
    }
  }

  // GET /api/usuarios/:id
  async show(req, res) {
    try {
      const { id } = req.params;
      const usuario = await Usuario.findByPk(id, {
        attributes: { exclude: ['senha'] }
      });
      
      if (!usuario) {
        return res.status(404).json({ error: 'Usuário não encontrado' });
      }
      
      return res.json(usuario);
    } catch (error) {
      return res.status(500).json({ error: error.message });
    }
  }

  // POST /api/usuarios
  async store(req, res) {
    try {
      const usuario = await Usuario.create(req.body);
      const { senha, ...usuarioSemSenha } = usuario.toJSON();
      return res.status(201).json(usuarioSemSenha);
    } catch (error) {
      return res.status(400).json({ error: error.message });
    }
  }

  // PUT /api/usuarios/:id
  async update(req, res) {
    try {
      const { id } = req.params;
      const usuario = await Usuario.findByPk(id);
      
      if (!usuario) {
        return res.status(404).json({ error: 'Usuário não encontrado' });
      }
      
      await usuario.update(req.body);
      const { senha, ...usuarioSemSenha } = usuario.toJSON();
      return res.json(usuarioSemSenha);
    } catch (error) {
      return res.status(400).json({ error: error.message });
    }
  }

  // DELETE /api/usuarios/:id
  async destroy(req, res) {
    try {
      const { id } = req.params;
      const usuario = await Usuario.findByPk(id);
      
      if (!usuario) {
        return res.status(404).json({ error: 'Usuário não encontrado' });
      }
      
      await usuario.destroy();
      return res.json({ message: 'Usuário deletado com sucesso' });
    } catch (error) {
      return res.status(500).json({ error: error.message });
    }
  }
}

export default new UsuarioController();
```

### Middleware de Autenticação

```javascript
// src/middleware/auth.js
import jwt from 'jsonwebtoken';

export const authMiddleware = (req, res, next) => {
  const authHeader = req.headers.authorization;

  if (!authHeader) {
    return res.status(401).json({ error: 'Token não fornecido' });
  }

  const [, token] = authHeader.split(' ');

  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET);
    req.userId = decoded.id;
    req.userPerfil = decoded.perfil;
    return next();
  } catch (error) {
    return res.status(401).json({ error: 'Token inválido' });
  }
};

export const adminMiddleware = (req, res, next) => {
  if (req.userPerfil !== 'admin') {
    return res.status(403).json({ error: 'Acesso negado. Requer perfil admin' });
  }
  return next();
};
```

### Rotas

```javascript
// src/routes/UsuarioRoutes.js
import { Router } from 'express';
import UsuarioController from '../controllers/UsuarioController.js';
import { authMiddleware, adminMiddleware } from '../middleware/auth.js';

const router = Router();

router.get('/usuarios', authMiddleware, UsuarioController.index);
router.get('/usuarios/:id', authMiddleware, UsuarioController.show);
router.post('/usuarios', authMiddleware, adminMiddleware, UsuarioController.store);
router.put('/usuarios/:id', authMiddleware, adminMiddleware, UsuarioController.update);
router.delete('/usuarios/:id', authMiddleware, adminMiddleware, UsuarioController.destroy);

export default router;
```

## 📊 Performance

### Otimizações Implementadas

```javascript
// Connection Pooling
const sequelize = new Sequelize(database, username, password, {
  pool: {
    max: 5,        // Máximo de conexões no pool
    min: 0,        // Mínimo de conexões no pool
    acquire: 30000, // Tempo máximo para adquirir conexão
    idle: 10000    // Tempo máximo de conexão ociosa
  }
});

// Eager Loading (Include)
const reservas = await Reserva.findAll({
  include: [
    { model: Usuario, attributes: ['id', 'nome'] },
    { model: Sala, attributes: ['id', 'nome'] }
  ]
});

// Atributos específicos (reduz dados transferidos)
const usuarios = await Usuario.findAll({
  attributes: ['id', 'nome', 'email']
});

// Paginação
const { page = 1, limit = 10 } = req.query;
const offset = (page - 1) * limit;

const usuarios = await Usuario.findAndCountAll({
  limit,
  offset
});
```

## 🔒 Segurança

### Boas Práticas Implementadas

```javascript
// Hash de senha com bcrypt
import bcrypt from 'bcryptjs';
const hashedPassword = await bcrypt.hash(senha, 10);

// JWT com expiração
const token = jwt.sign(
  { id: usuario.id, perfil: usuario.perfil },
  process.env.JWT_SECRET,
  { expiresIn: process.env.JWT_EXPIRES_IN }
);

// Helmet para security headers
import helmet from 'helmet';
app.use(helmet());

// Rate limiting
import rateLimit from 'express-rate-limit';
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutos
  max: 100 // 100 requests por IP
});
app.use('/api/', limiter);

// Validação de entrada
import { body, validationResult } from 'express-validator';

router.post('/usuarios',
  body('email').isEmail(),
  body('senha').isLength({ min: 6 }),
  (req, res, next) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }
    next();
  },
  UsuarioController.store
);
```

## 🐛 Troubleshooting

### Erro: "Cannot find module"
```bash
# Verificar se está usando type: "module" no package.json
# Usar import/export em vez de require
```

### Erro: "Connection refused"
```bash
# Verificar variáveis de ambiente
echo $DB_HOST

# Verificar se PostgreSQL está rodando
docker-compose ps postgres
```

### Erro: "Port already in use"
```bash
# Usar porta diferente no .env
PORT=3031
```

## 📚 Recursos Adicionais

- [Express.js Documentation](https://expressjs.com/)
- [Sequelize ORM](https://sequelize.org/)
- [Node.js Best Practices](https://github.com/goldbergyoni/nodebestpractices)

---

**Autor**: Mauro Furtado  
**Versão**: 1.0.0  
**Última Atualização**: Janeiro 2026