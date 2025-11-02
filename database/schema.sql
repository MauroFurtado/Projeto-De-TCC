DO
$$
BEGIN
   IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'MauroFurtado') THEN
      CREATE ROLE "MauroFurtado" WITH LOGIN PASSWORD 'trocar_senha' CREATEDB;
   END IF;
END
$$;

CREATE DATABASE ReservaSalas
    WITH
    OWNER = "MauroFurtado"
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    IS_TEMPLATE = False;
-- ==========================
-- ENUMS
-- ==========================
CREATE TYPE perfil_usuario AS ENUM ('admin', 'comum');
CREATE TYPE tipo_sala AS ENUM ('laboratorio', 'aula', 'reuniao', 'auditório');

-- ==========================
-- USUÁRIOS
-- ==========================
CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    senha TEXT NOT NULL,
    perfil perfil_usuario NOT NULL DEFAULT 'comum',
    criado_em TIMESTAMP DEFAULT NOW()
);

-- ==========================
-- SALAS
-- ==========================
CREATE TABLE salas (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    tipo tipo_sala NOT NULL,
    capacidade INT NOT NULL CHECK (capacidade > 0),
    localizacao VARCHAR(255),
    disponivel BOOLEAN DEFAULT TRUE,
    criado_em TIMESTAMP DEFAULT NOW()
);

-- ==========================
-- RESERVAS
-- ==========================
CREATE TABLE reservas (
    id SERIAL PRIMARY KEY,
    usuario_id INT NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    sala_id INT NOT NULL REFERENCES salas(id) ON DELETE CASCADE,
    data_reserva DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fim TIME NOT NULL,
    criado_em TIMESTAMP DEFAULT NOW(),
    CONSTRAINT chk_horario CHECK (hora_fim > hora_inicio),
    CONSTRAINT reserva_unica UNIQUE (sala_id, data_reserva, hora_inicio, hora_fim)
);

-- ==========================
-- ÍNDICES
-- ==========================
CREATE INDEX idx_reservas_usuario ON reservas(usuario_id);
CREATE INDEX idx_reservas_sala ON reservas(sala_id);
