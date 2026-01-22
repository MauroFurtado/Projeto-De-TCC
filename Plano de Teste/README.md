# Guia de Testes de Performance - JMeter

## 📊 Visão Geral

Este documento descreve o plano de testes de performance implementado para avaliar as três implementações da API de Reserva de Salas (ASP.NET, Django e Node.js).

## 🎯 Objetivos dos Testes

- Avaliar **desempenho** sob diferentes cargas
- Identificar **limites de escalabilidade**
- Medir **tempo de resposta** em cenários realistas
- Detectar **vazamentos de memória** e degradação ao longo do tempo
- Comparar **throughput** entre as tecnologias

## 📁 Localização

```
Plano de Teste/
└── Plano de Teste.jmx
```

## 🔧 Pré-requisitos

### Instalar Apache JMeter

#### Windows
```powershell
# Download do site oficial
https://jmeter.apache.org/download_jmeter.cgi

# Ou via Chocolatey
choco install jmeter
```

#### Linux/Mac
```bash
# Via Homebrew (Mac)
brew install jmeter

# Via apt (Ubuntu/Debian)
sudo apt-get install jmeter

# Manualmente
wget https://downloads.apache.org/jmeter/binaries/apache-jmeter-5.6.3.tgz
tar -xf apache-jmeter-5.6.3.tgz
```

### Verificar Instalação

```bash
jmeter --version
# Deve exibir: Apache JMeter 5.6.3 ou superior
```

## 🏗️ Estrutura do Plano de Teste

### Variáveis Globais

| Variável | Valor Padrão | Descrição |
|----------|--------------|-----------|
| `protocol` | http | Protocolo (http/https) |
| `host` | localhost | Endereço do servidor |
| `port` | 5000 | Porta da API |
| `basePath` | /api | Caminho base da API |
| `framework` | ASP | Framework a testar (ASP/Django/Node) |

### Cenários de Teste

O plano inclui 3 grupos de testes principais:

## 📈 1. Teste de Estresse

Avalia o comportamento do sistema sob carga crescente gradual.

### Configuração

| Fase | Usuários | Ramp-Up (s) | Duração (s) | Ramp-Down (s) |
|------|----------|-------------|-------------|---------------|
| 1 | 100 | 30 | 60 | 10 |
| 2 | 200 | 30 | 60 | 10 |
| 3 | 300 | 30 | 60 | 10 |
| 4 | 650 | 30 | 60 | 10 |

### Objetivo

- Identificar ponto de saturação
- Observar degradação gradual de performance
- Medir tempo de resposta sob diferentes cargas
- Avaliar taxa de erro em cada fase



## ⚡ 2. Teste de Pico

Simula picos súbitos de tráfego (como em horários de pico ou eventos especiais).

### Configuração

| Fase | Usuários | Início (s) | Ramp-Up (s) | Duração (s) | Ramp-Down (s) |
|------|----------|------------|-------------|-------------|---------------|
| Baseline | 50 | 0 | 1 | 199 | 1 |
| **Pico** | 400 | 200 | 1 | 60 | 1 |
| Recovery | 50 | 261 | 1 | 200 | 1 |

### Objetivo

- Avaliar comportamento em picos súbitos
- Medir tempo de recuperação após pico
- Identificar bottlenecks sob carga extrema
- Verificar estabilidade após pico


## ⏱️ 3. Teste de Resistência (Soak Test)

Verifica estabilidade do sistema sob carga moderada por período prolongado.

### Configuração

| Usuários | Ramp-Up (s) | Duração (s) | Ramp-Down (s) | Duração Total |
|----------|-------------|-------------|---------------|---------------|
| 40 | 60 | 3600 | 60 | **1 hora** |

### Objetivo

- Detectar vazamentos de memória
- Identificar degradação de performance ao longo do tempo
- Verificar estabilidade de conexões de longo prazo
- Avaliar gestão de recursos (conexões DB, threads)



## 🔄 Operações Testadas

Cada thread de usuário executa o seguinte fluxo completo:

### 1. POST /api/usuarios
Criar novo usuário com dados dinâmicos
```json
{
  "nome": "teste_<counter>",
  "email": "<email_gerado>@teste.com",
  "senha": "senha123",
  "perfil": "admin"
}
```

### 2. POST /api/salas
Criar nova sala
```json
{
  "nome": "sala_<counter>",
  "tipo": "laboratorio",
  "capacidade": 100,
  "localizacao": "bloco A"
}
```

### 3. POST /api/reservas
Criar reserva usando IDs extraídos
```json
{
  "usuario_id": "${userId}",
  "sala_id": "${salaId}",
  "data_reserva": "2026-10-07",
  "hora_inicio": "10:00",
  "hora_fim": "12:00"
}
```

### 4-6. GET Endpoints
- GET /api/usuarios
- GET /api/salas
- GET /api/reservas

### 7-9. PUT Endpoints
- PUT /api/usuarios/${userId}
- PUT /api/salas/${salaId}
- PUT /api/reservas/${reservaId}

### 10-12. DELETE Endpoints
- DELETE /api/reservas/${reservaId}
- DELETE /api/usuarios/${userId}
- DELETE /api/salas/${salaId}

## 🚀 Como Executar

### Preparar Ambiente

```bash
# 1. Iniciar o banco de dados e API a testar
cd /caminho/do/projeto

# Para ASP.NET
docker-compose up -d postgres asp_api

# Para Django
docker-compose up -d postgres django_api

# Para Node.js
docker-compose up -d postgres node_api

# 2. Aguardar serviços iniciarem
docker-compose logs -f [nome_servico]
```

### Executar Testes - Modo GUI (Desenvolvimento)

```bash
# Abrir JMeter GUI
jmeter

# Na GUI:
# File > Open > Selecionar "Plano de Teste/Plano de Teste.jmx"
# Configurar variáveis conforme necessário
# Run > Start (Ctrl+R)
```

### Executar Testes - Modo CLI (Produção/CI)

#### Teste Completo (Todos os Cenários)

```bash
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -l resultados/resultado-completo.jtl \
  -e -o resultados/relatorio-html
```

#### Teste Individual por Cenário

```bash
# Apenas Teste de Estresse
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -JtestType=estresse \
  -l resultados/estresse-asp.jtl \
  -e -o resultados/relatorio-estresse-asp

# Apenas Teste de Pico
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -JtestType=pico \
  -l resultados/pico-django.jtl \
  -e -o resultados/relatorio-pico-django

# Apenas Teste de Resistência
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -JtestType=resistencia \
  -l resultados/resistencia-node.jtl \
  -e -o resultados/relatorio-resistencia-node
```

#### Testar APIs Diferentes

```bash
# ASP.NET (porta 5000)
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -Jport=5000 \
  -Jframework=ASP \
  -l resultados/asp-results.jtl \
  -e -o resultados/relatorio-asp

# Django (porta 8000)
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -Jport=8000 \
  -Jframework=Django \
  -l resultados/django-results.jtl \
  -e -o resultados/relatorio-django

# Node.js (porta 3001)
jmeter -n \
  -t "Plano de Teste/Plano de Teste.jmx" \
  -Jport=3001 \
  -Jframework=Node \
  -l resultados/node-results.jtl \
  -e -o resultados/relatorio-node
```

### Parâmetros CLI Importantes

| Parâmetro | Descrição |
|-----------|-----------|
| `-n` | Modo não-GUI (CLI) |
| `-t <arquivo>` | Arquivo do plano de teste |
| `-l <arquivo>` | Arquivo de log dos resultados (.jtl) |
| `-e` | Gerar relatório HTML após execução |
| `-o <diretório>` | Diretório de saída do relatório |
| `-J<variavel>=<valor>` | Definir propriedade JMeter |

## 📊 Analisar Resultados

### Relatório HTML

O JMeter gera automaticamente um dashboard HTML com:

- **Statistics**: Estatísticas gerais
- **Response Times Over Time**: Gráfico de tempo de resposta
- **Throughput**: Requisições por segundo
- **Error Rate**: Taxa de erro ao longo do tempo
- **Response Times Percentiles**: P50, P90, P95, P99

Abrir relatório:
```bash
# Windows
start resultados/relatorio-html/index.html

# Linux/Mac
open resultados/relatorio-html/index.html
```

### Métricas-Chave para Análise

#### 1. Tempo de Resposta
```
- Média (Mean): Tempo médio de resposta
- Mediana (P50): 50% das requisições abaixo deste valor
- P90: 90% das requisições abaixo deste valor
- P95: 95% das requisições abaixo deste valor
- P99: 99% das requisições abaixo deste valor
- Mínimo/Máximo: Extremos
```

#### 2. Throughput
```
- Requisições/segundo
- Transações/segundo
- Dados transferidos (KB/s)
```

#### 3. Taxa de Erro
```
- Percentual de requisições com erro
- Tipos de erro (timeout, 500, etc)
```

#### 4. Latência de Rede
```
- Tempo de conexão
- Latência média
```

### Comparar Resultados Entre APIs

```bash
# Criar script de comparação
cat > comparar-resultados.sh << 'EOF'
#!/bin/bash

echo "Comparação de Performance - APIs"
echo "================================="
echo

for api in asp django node; do
  echo "### $api ###"
  grep -A 1 "summary =" resultados/${api}-results.jtl | tail -1
  echo
done
EOF

chmod +x comparar-resultados.sh
./comparar-resultados.sh
```

## 📋 Checklist de Execução

### Antes dos Testes

- [ ] Banco de dados limpo ou com dados consistentes
- [ ] API rodando e respondendo
- [ ] Recursos do sistema monitorados (CPU, RAM)
- [ ] Logs habilitados para análise posterior
- [ ] Nenhum outro processo pesado rodando

### Durante os Testes

- [ ] Monitorar uso de recursos do servidor
- [ ] Observar logs de erro da aplicação
- [ ] Verificar conexões com banco de dados
- [ ] Anotar comportamentos anormais

### Após os Testes

- [ ] Salvar resultados (.jtl)
- [ ] Gerar e revisar relatórios HTML
- [ ] Documentar observações
- [ ] Limpar banco de dados se necessário
- [ ] Comparar com execuções anteriores

## 🎨 Personalizar Testes

### Modificar Variáveis no JMeter GUI

1. Abrir `Plano de Teste.jmx` no JMeter
2. Selecionar "User Defined Variables"
3. Modificar valores conforme necessário
4. Salvar arquivo

### Adicionar Novos Endpoints

1. Clicar direito no Thread Group
2. Add > Sampler > HTTP Request
3. Configurar:
   - Path: `${basePath}/novo-endpoint`
   - Method: GET/POST/PUT/DELETE
   - Body (se necessário)
4. Adicionar JSON Extractor se precisar capturar dados

### Configurar Listeners

Adicionar listeners para visualização em tempo real:

- View Results Tree
- Summary Report
- Graph Results
- Response Time Graph

**Nota**: Remover listeners ao executar em CLI (impacta performance).

## ⚠️ Troubleshooting

### Erro: "Connection refused"

```bash
# Verificar se API está rodando
curl http://localhost:5000/api/usuarios

# Verificar logs
docker-compose logs asp_api
```

### Erro: "Too many open files"

```bash
# Linux: Aumentar limite
ulimit -n 65536

# Verificar limite atual
ulimit -n
```

### Erro: "Out of Memory"

```bash
# Aumentar memória do JMeter
export HEAP="-Xms1g -Xmx4g"
jmeter -n -t "Plano de Teste.jmx" ...
```

### Performance Degradada

```bash
# Limpar cache do JMeter
rm -rf ~/jmeter.log

# Executar com menos threads
# Modificar valores no plano de teste
```

## 📚 Recursos Adicionais

- [JMeter User Manual](https://jmeter.apache.org/usermanual/)
- [JMeter Best Practices](https://jmeter.apache.org/usermanual/best-practices.html)
- [Performance Testing Guidance](https://martinfowler.com/articles/practical-test-pyramid.html)


**Versão do Plano**: 1.0.0  
**JMeter Versão**: 5.6.3  
**Última Atualização**: Janeiro 2026
