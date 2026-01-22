# Sistema de Reserva de Salas - TCC

## 📋 Descrição do Projeto

Sistema completo de gerenciamento de reservas de salas desenvolvido como Trabalho de Conclusão de Curso (TCC). O projeto implementa a mesma API REST em três diferentes tecnologias (ASP.NET Core, Django REST Framework e Node.js) para fins de análise comparativa de desempenho, escalabilidade e manutenibilidade.

## 🎯 Objetivos

- Comparar diferentes tecnologias de backend para desenvolvimento de APIs REST
- Analisar desempenho sob diferentes cenários de carga
- Implementar testes de estresse, pico e resistência


## 🏗️ Arquitetura

O sistema é composto por:

- **3 APIs REST** implementadas em diferentes tecnologias
- **Banco de Dados PostgreSQL** compartilhado
- **Docker/Docker Compose** para orquestração de containers
- **JMeter** para testes de carga e performance

### Estrutura de Diretórios

```
Gestor-De-Projetos/
├── ASP/                          # API em ASP.NET Core 8.0
│   ├── Dockerfile
│   └── ResrvaDeSala_API/
│       ├── Controllers/          # Controladores da API
│       ├── Data/                 # Contexto do banco e mapeamentos
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Models/               # Modelos de dados
│       └── Services/             # Serviços (JWT, etc)
│
├── Django/                       # API em Django 5.2
│   ├── Dockerfile
│   ├── core/                     # App principal
│   │   ├── models.py             # Modelos Django
│   │   ├── serializers.py        # Serializers DRF
│   │   ├── views.py              # Views da API
│   │   └── permissions.py        # Permissões customizadas
│   └── ReservaDeSalas/           # Configurações do projeto
│
├── node_api/                     # API em Node.js
│   └── ReservaDeSala_API/
│       ├── Dockerfile
│       ├── src/
│       │   ├── controllers/      # Controladores
│       │   ├── models/           # Modelos Sequelize
│       │   ├── routes/           # Rotas da API
│       │   ├── middleware/       # Middlewares (Auth)
│       │   └── config/           # Configurações
│       └── package.json
│
├── database/                     # Scripts do banco de dados
│   ├── schema.sql                # Schema inicial
│   └── TRUNCATE TABLE...sql     # Scripts de manutenção
│
├── Plano de Teste/              # Testes de performance
│   └── Plano de Teste.jmx       # Plano JMeter
│
├── prometheus/                   # Monitoramento
│   └── prometheus.yml
│
└── docker-compose.yml           # Orquestração dos serviços
```

## 🛠️ Tecnologias Utilizadas

### Backend APIs

| Tecnologia | Versão | Framework Web | ORM/Database |
|------------|--------|---------------|--------------|
| **ASP.NET Core** | 8.0 | ASP.NET Core Web API | Entity Framework Core + Npgsql |
| **Django** | 5.2 | Django REST Framework | Django ORM |
| **Node.js** | 20+ | Express.js | Sequelize |

### Infraestrutura

- **PostgreSQL** 16 (Alpine)
- **Docker** & **Docker Compose**
- **Apache JMeter** 5.6.3 (Testes de Carga)
- **Prometheus** (Monitoramento)

### Autenticação

- **JWT (JSON Web Tokens)** implementado nas três APIs

## 📊 Modelo de Dados

### Entidades

#### Usuários
```sql
CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    senha TEXT NOT NULL,
    perfil perfil_usuario NOT NULL DEFAULT 'comum',
    criado_em TIMESTAMP DEFAULT NOW()
);
```

#### Salas
```sql
CREATE TABLE salas (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    tipo tipo_sala NOT NULL,
    capacidade INT NOT NULL CHECK (capacidade > 0),
    localizacao VARCHAR(255),
    disponivel BOOLEAN DEFAULT TRUE,
    criado_em TIMESTAMP DEFAULT NOW()
);
```

#### Reservas
```sql
CREATE TABLE reservas (
    id SERIAL PRIMARY KEY,
    usuario_id INT REFERENCES usuarios(id) ON DELETE CASCADE,
    sala_id INT REFERENCES salas(id) ON DELETE CASCADE,
    data_reserva DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fim TIME NOT NULL,
    criado_em TIMESTAMP DEFAULT NOW(),
    CONSTRAINT horario_valido CHECK (hora_inicio < hora_fim)
);
```

### Tipos Enumerados

- **perfil_usuario**: `admin`, `comum`
- **tipo_sala**: `laboratorio`, `aula`, `reuniao`, `auditorio`

## 🚀 Como Executar

### Pré-requisitos

- Docker 24+
- Docker Compose 2+
- (Opcional) JMeter 5.6+ para executar os testes

### 1. Clonar o Repositório

```bash
git clone https://github.com/MauroFurtado/Projeto-De-TCC.git
cd Projeto-De-TCC
```

### 2. Configurar Variáveis de Ambiente (Opcional)

Crie um arquivo `.env` na raiz do projeto:

```env
# Banco de Dados
POSTGRES_USER=
POSTGRES_PASSWORD=
POSTGRES_DB=
POSTGRES_PORT=

# JWT
JWT_SECRET=seu-super-secreto-aqui
JWT_ISSUER=ReservaSalasAPI
JWT_AUDIENCE=ReservaSalasClients
```

### 3. Iniciar os Serviços

#### Iniciar todas as APIs:
```bash
docker-compose up -d
```

#### Iniciar apenas uma API específica:
```bash
# ASP.NET
docker-compose up -d postgres asp_api

# Django
docker-compose up -d postgres django_api

# Node.js
docker-compose up -d postgres node_api
```

### 4. Verificar Status dos Containers

```bash
docker-compose ps
```

### 5. Acessar as APIs

- **ASP.NET API**: http://localhost:5000/api
- **Django API**: http://localhost:8000/api
- **Node.js API**: http://localhost:3001/api

## 📡 Endpoints da API

Todas as três implementações seguem a mesma especificação de API:

### Autenticação

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/auth/login` | Login de usuário | Não |
| POST | `/api/auth/register` | Registro de novo usuário | Não |

### Usuários

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/usuarios` | Listar todos os usuários | Sim |
| GET | `/api/usuarios/{id}` | Obter usuário por ID | Sim |
| POST | `/api/usuarios` | Criar novo usuário | Admin |
| PUT | `/api/usuarios/{id}` | Atualizar usuário | Admin |
| DELETE | `/api/usuarios/{id}` | Deletar usuário | Admin |

### Salas

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/salas` | Listar todas as salas | Sim |
| GET | `/api/salas/{id}` | Obter sala por ID | Sim |
| POST | `/api/salas` | Criar nova sala | Admin |
| PUT | `/api/salas/{id}` | Atualizar sala | Admin |
| DELETE | `/api/salas/{id}` | Deletar sala | Admin |

### Reservas

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/reservas` | Listar todas as reservas | Sim |
| GET | `/api/reservas/{id}` | Obter reserva por ID | Sim |
| POST | `/api/reservas` | Criar nova reserva | Sim |
| PUT | `/api/reservas/{id}` | Atualizar reserva | Sim |
| DELETE | `/api/reservas/{id}` | Deletar reserva | Sim/Admin |

### Exemplo de Requisição

#### Criar Usuário
```http
POST /api/usuarios HTTP/1.1
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "senha": "senha123",
  "perfil": "comum"
}
```

#### Criar Reserva
```http
POST /api/reservas HTTP/1.1
Authorization: Bearer {token}
Content-Type: application/json

{
  "usuario_id": 1,
  "sala_id": 5,
  "data_reserva": "2026-10-07",
  "hora_inicio": "10:00",
  "hora_fim": "12:00"
}
```

## 🧪 Testes de Performance

O projeto inclui um plano completo de testes JMeter localizado em `Plano de Teste/Plano de Teste.jmx`.

### Cenários de Teste

#### 1. Teste de Estresse
Avalia o comportamento do sistema sob carga crescente:
- **Fase 1**: 100 usuários (ramp-up: 30s, duração: 60s)
- **Fase 2**: 200 usuários (ramp-up: 30s, duração: 60s)
- **Fase 3**: 300 usuários (ramp-up: 30s, duração: 60s)
- **Fase 4**: 650 usuários (ramp-up: 30s, duração: 60s)

#### 2. Teste de Pico
Simula picos súbitos de tráfego:
- **Fase 1**: 50 usuários (duração: 199s)
- **Fase 2**: 400 usuários (pico imediato, duração: 60s)
- **Fase 3**: 50 usuários (duração: 200s)

#### 3. Teste de Resistência
Verifica estabilidade em uso prolongado:
- **40 usuários** contínuos por **1 hora**

### Executar Testes JMeter

```bash
# Modo GUI (desenvolvimento)
jmeter -t "Plano de Teste/Plano de Teste.jmx"

# Modo CLI (produção)
jmeter -n -t "Plano de Teste/Plano de Teste.jmx" -l resultados.jtl -e -o ./relatorio
```

### Operações Testadas

Cada cenário executa o seguinte fluxo completo:
1. Criar usuário (POST)
2. Criar sala (POST)
3. Criar reserva (POST)
4. Listar usuários (GET)
5. Listar salas (GET)
6. Listar reservas (GET)
7. Atualizar usuário (PUT)
8. Atualizar sala (PUT)
9. Atualizar reserva (PUT)
10. Deletar reserva (DELETE)
11. Deletar usuário (DELETE)
12. Deletar sala (DELETE)

## 📈 Monitoramento

O projeto inclui integração com Prometheus para monitoramento de métricas.

```bash
# Acessar Prometheus
http://localhost:9090
```

## 🔒 Segurança

### Implementações de Segurança

- **Autenticação JWT**: Tokens com expiração configurável
- **Hashing de Senhas**: BCrypt para todas as implementações
- **Validação de Entrada**: Validação de dados em todas as camadas
- **CORS**: Configurado para ambientes específicos
- **SQL Injection Protection**: ORMs previnem injeção SQL
- **Rate Limiting**: Recomendado implementar em produção

### Variáveis de Ambiente Sensíveis

⚠️ **IMPORTANTE**: Nunca commitar credenciais reais!

- `JWT_SECRET`: Chave secreta para assinatura de tokens
- `POSTGRES_PASSWORD`: Senha do banco de dados
- Trocar valores padrão em produção

## 📚 Documentação Adicional

- [ASP.NET API - Documentação Específica](./ASP/README.md)
- [Django API - Documentação Específica](./Django/README.md)
- [Node.js API - Documentação Específica](./node_api/ReservaDeSala_API/README.md)

## 🛠️ Desenvolvimento

### Estrutura de Commits

O projeto segue a convenção de commits semânticos:

- `feat:` Nova funcionalidade
- `fix:` Correção de bug
- `docs:` Documentação
- `test:` Testes
- `refactor:` Refatoração de código
- `perf:` Melhorias de performance
- `chore:` Tarefas de manutenção

### Boas Práticas Implementadas

- **Clean Architecture**: Separação de camadas
- **SOLID Principles**: Aplicados nas três implementações
- **RESTful Design**: APIs seguem princípios REST
- **DRY**: Evitar repetição de código
- **Documentação**: Código bem documentado

## 🐛 Troubleshooting

### Problema: Container não inicia

```bash
# Ver logs
docker-compose logs [nome_servico]

# Reiniciar serviços
docker-compose restart

# Reconstruir imagens
docker-compose up -d --build
```

### Problema: Erro de conexão com banco de dados

```bash
# Verificar se o PostgreSQL está rodando
docker-compose ps postgres

# Verificar logs do PostgreSQL
docker-compose logs postgres

# Resetar banco de dados
docker-compose down -v
docker-compose up -d
```

### Problema: Porta já em uso

Edite `docker-compose.yml` e altere as portas mapeadas:

```yaml
ports:
  - "5001:5000"  # Em vez de 5000:5000
```

## 📊 Resultados Esperados

Este TCC visa demonstrar:

1. **Comparação de Performance**: Benchmarks entre as três tecnologias
2. **Análise de Escalabilidade**: Comportamento sob diferentes cargas
3. **Facilidade de Manutenção**: Complexidade e legibilidade do código
4. **Curva de Aprendizado**: Tempo para implementação equivalente
5. **Ecossistema**: Bibliotecas e ferramentas disponíveis

## 👥 Autor

**Mauro Furtado**
- GitHub: [@MauroFurtado](https://github.com/MauroFurtado)

## 📄 Licença

Este projeto é desenvolvido para fins acadêmicos como Trabalho de Conclusão de Curso.

## 🙏 Agradecimentos

- Orientadores do TCC
- Comunidades open-source das tecnologias utilizadas
- Colegas que contribuíram com feedback e testes

---

**Data**: Janeiro 2026  
**Versão**: 1.0.0  
**Status**: Em Desenvolvimento
