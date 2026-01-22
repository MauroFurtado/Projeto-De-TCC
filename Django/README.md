# API Django - Sistema de Reserva de Salas

## 📝 Descrição

Implementação da API de Reserva de Salas utilizando **Django 5.2** com Django REST Framework, seguindo padrões pythônicos e best practices da comunidade Django.

## 🏗️ Arquitetura

### Estrutura do Projeto Django

```
Django/
├── core/                      # App principal
│   ├── __init__.py
│   ├── admin.py              # Interface administrativa
│   ├── apps.py               # Configuração do app
│   ├── models.py             # Modelos de dados (ORM)
│   ├── serializers.py        # Serializers DRF
│   ├── views.py              # Views da API (ViewSets)
│   ├── urls.py               # Rotas do app
│   ├── permissions.py        # Permissões customizadas
│   ├── tests.py              # Testes
│   └── migrations/           # Migrações do banco
│
└── ReservaDeSalas/           # Configurações do projeto
    ├── __init__.py
    ├── settings.py           # Configurações principais
    ├── urls.py               # URLs raiz
    ├── wsgi.py               # WSGI application
    └── asgi.py               # ASGI application
```

## 🛠️ Tecnologias e Pacotes

### Dependências (requirements.txt)

```txt
Django==5.2.8
djangorestframework==3.15.2
psycopg2-binary==2.9.9
python-dotenv==1.0.0
PyJWT==2.8.0
django-cors-headers==4.3.1
djangorestframework-simplejwt==5.3.1
```

### Frameworks e Bibliotecas

- **Django 5.2**: Framework web Python
- **Django REST Framework**: Toolkit para APIs REST
- **psycopg2**: Adaptador PostgreSQL
- **python-dotenv**: Gerenciamento de variáveis de ambiente
- **PyJWT**: Autenticação JWT
- **django-cors-headers**: Configuração CORS

## ⚙️ Configuração

### settings.py (Principais Configurações)

```python
# Database
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql',
        'NAME': os.getenv('DB_NAME', 'ReservaSalas'),
        'USER': os.getenv('DB_USER', 'MauroFurtado'),
        'PASSWORD': os.getenv('DB_PASSWORD'),
        'HOST': os.getenv('DB_HOST', 'localhost'),
        'PORT': os.getenv('DB_PORT', '5432'),
    }
}

# REST Framework
REST_FRAMEWORK = {
    'DEFAULT_AUTHENTICATION_CLASSES': [
        'rest_framework_simplejwt.authentication.JWTAuthentication',
    ],
    'DEFAULT_PERMISSION_CLASSES': [
        'rest_framework.permissions.IsAuthenticated',
    ],
    'DEFAULT_PAGINATION_CLASS': 'rest_framework.pagination.PageNumberPagination',
    'PAGE_SIZE': 100
}

# CORS
CORS_ALLOW_ALL_ORIGINS = True  # Apenas desenvolvimento
```

### Variáveis de Ambiente (.env)

```env
# Django
SECRET_KEY=django-insecure-key-mudar-em-producao
DEBUG=True
ALLOWED_HOSTS=localhost,127.0.0.1

# Database
DB_NAME=ReservaSalas
DB_USER=MauroFurtado
DB_PASSWORD=
DB_HOST=postgres
DB_PORT=5432

# JWT
JWT_SECRET_KEY=seu-jwt-secret-aqui
JWT_ALGORITHM=HS256
JWT_EXPIRATION_HOURS=24
```

## 🚀 Executando o Projeto

### Requisitos

- Python 3.11+
- PostgreSQL 16+
- pip

### Modo Desenvolvimento

```bash
# Criar ambiente virtual
python -m venv venv

# Ativar ambiente virtual
# Windows
venv\Scripts\activate
# Linux/Mac
source venv/bin/activate

# Instalar dependências
pip install -r requirements.txt

# Executar migrações
python manage.py makemigrations
python manage.py migrate

# Criar superusuário (opcional)
python manage.py createsuperuser

# Executar servidor
python manage.py runserver 0.0.0.0:8000
```

### Docker

```bash
# Build da imagem
docker build -t reserva-django-api .

# Executar container
docker run -p 8000:8000 \
  -e DB_HOST=postgres \
  -e DB_NAME=ReservaSalas \
  -e DB_USER=MauroFurtado \
  -e DB_PASSWORD=MGFurtad0 \
  reserva-django-api
```

### Com Docker Compose (Recomendado)

```bash
# Do diretório raiz do projeto
docker-compose up -d django_api
```

## 📡 Endpoints

### Base URL
```
http://localhost:8000/api
```

### Django Admin
```
http://localhost:8000/admin
```

### Browsable API (DRF)
```
http://localhost:8000/api/
```

### Autenticação JWT

#### POST /api/token/
Obter token de acesso
```json
// Request
{
  "email": "usuario@exemplo.com",
  "password": "senha123"
}

// Response 200 OK
{
  "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refresh": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### POST /api/token/refresh/
Renovar token
```json
// Request
{
  "refresh": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

// Response 200 OK
{
  "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Usuários

#### GET /api/usuarios/
```json
// Response 200 OK
{
  "count": 50,
  "next": "http://localhost:8000/api/usuarios/?page=2",
  "previous": null,
  "results": [
    {
      "id": 1,
      "nome": "João Silva",
      "email": "joao@exemplo.com",
      "perfil": "admin",
      "criado_em": "2026-01-20T10:00:00Z"
    }
  ]
}
```

#### POST /api/usuarios/
```json
// Request
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
  "criado_em": "2026-01-22T14:30:00Z"
}
```

### Salas

#### GET /api/salas/
```json
// Response 200 OK
{
  "count": 20,
  "next": null,
  "previous": null,
  "results": [
    {
      "id": 1,
      "nome": "Sala 101",
      "tipo": "aula",
      "capacidade": 30,
      "localizacao": "Bloco A",
      "disponivel": true,
      "criado_em": "2026-01-15T08:00:00Z"
    }
  ]
}
```

#### POST /api/salas/
```json
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
  "disponivel": true,
  "criado_em": "2026-01-22T14:35:00Z"
}
```

### Reservas

#### GET /api/reservas/
```json
// Response 200 OK
{
  "count": 100,
  "next": "http://localhost:8000/api/reservas/?page=2",
  "previous": null,
  "results": [
    {
      "id": 1,
      "usuario": {
        "id": 1,
        "nome": "João Silva"
      },
      "sala": {
        "id": 5,
        "nome": "Laboratório 205"
      },
      "data_reserva": "2026-02-10",
      "hora_inicio": "10:00:00",
      "hora_fim": "12:00:00",
      "criado_em": "2026-01-22T14:40:00Z"
    }
  ]
}
```

#### POST /api/reservas/
```json
// Request
{
  "usuario_id": 1,
  "sala_id": 5,
  "data_reserva": "2026-02-10",
  "hora_inicio": "10:00",
  "hora_fim": "12:00"
}

// Response 201 Created
{
  "id": 10,
  "usuario_id": 1,
  "sala_id": 5,
  "data_reserva": "2026-02-10",
  "hora_inicio": "10:00:00",
  "hora_fim": "12:00:00",
  "criado_em": "2026-01-22T14:40:00Z"
}
```

## 🧪 Testes

### Executar Testes

```bash
# Todos os testes
python manage.py test

# Testes de um app específico
python manage.py test core

# Testes com coverage
pip install coverage
coverage run --source='.' manage.py test
coverage report
coverage html
```

### Exemplo de Teste

```python
from django.test import TestCase
from rest_framework.test import APIClient
from core.models import Usuario

class UsuarioTestCase(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.usuario = Usuario.objects.create(
            nome="Teste",
            email="teste@exemplo.com",
            perfil="comum"
        )
    
    def test_listar_usuarios(self):
        response = self.client.get('/api/usuarios/')
        self.assertEqual(response.status_code, 200)
```

## 🔧 Desenvolvimento

### Modelos (models.py)

```python
from django.db import models

class Usuario(models.Model):
    PERFIL_CHOICES = [
        ('admin', 'Administrador'),
        ('comum', 'Comum'),
    ]
    
    nome = models.CharField(max_length=255)
    email = models.EmailField(unique=True)
    senha = models.CharField(max_length=255)
    perfil = models.CharField(max_length=10, choices=PERFIL_CHOICES, default='comum')
    criado_em = models.DateTimeField(auto_now_add=True)
    
    class Meta:
        db_table = 'usuarios'
        verbose_name = 'Usuário'
        verbose_name_plural = 'Usuários'
    
    def __str__(self):
        return self.nome
```

### Serializers (serializers.py)

```python
from rest_framework import serializers
from .models import Usuario

class UsuarioSerializer(serializers.ModelSerializer):
    class Meta:
        model = Usuario
        fields = ['id', 'nome', 'email', 'perfil', 'criado_em']
        read_only_fields = ['id', 'criado_em']

class UsuarioCreateSerializer(serializers.ModelSerializer):
    senha = serializers.CharField(write_only=True)
    
    class Meta:
        model = Usuario
        fields = ['nome', 'email', 'senha', 'perfil']
    
    def create(self, validated_data):
        # Hash da senha antes de salvar
        from django.contrib.auth.hashers import make_password
        validated_data['senha'] = make_password(validated_data['senha'])
        return super().create(validated_data)
```

### ViewSets (views.py)

```python
from rest_framework import viewsets, permissions
from .models import Usuario
from .serializers import UsuarioSerializer, UsuarioCreateSerializer

class UsuarioViewSet(viewsets.ModelViewSet):
    queryset = Usuario.objects.all()
    permission_classes = [permissions.IsAuthenticated]
    
    def get_serializer_class(self):
        if self.action == 'create':
            return UsuarioCreateSerializer
        return UsuarioSerializer
    
    def get_permissions(self):
        if self.action in ['create', 'update', 'partial_update', 'destroy']:
            return [permissions.IsAdminUser()]
        return super().get_permissions()
```

### Permissões Customizadas (permissions.py)

```python
from rest_framework import permissions

class IsAdminOrReadOnly(permissions.BasePermission):
    """
    Permite leitura para todos, mas escrita apenas para admins
    """
    def has_permission(self, request, view):
        if request.method in permissions.SAFE_METHODS:
            return True
        return request.user and request.user.perfil == 'admin'

class IsOwnerOrAdmin(permissions.BasePermission):
    """
    Permite acesso ao proprietário ou admin
    """
    def has_object_permission(self, request, view, obj):
        if request.user.perfil == 'admin':
            return True
        return obj.usuario == request.user
```

### Adicionar Nova Migration

```bash
# Criar migrations
python manage.py makemigrations

# Aplicar migrations
python manage.py migrate

# Ver SQL da migration
python manage.py sqlmigrate core 0001

# Listar migrations
python manage.py showmigrations
```

## 🏛️ Padrões Django

### MTV Pattern (Model-Template-View)

Para APIs REST, o padrão é adaptado:
- **Model**: Camada de dados (ORM)
- **Serializer**: Validação e serialização
- **ViewSet**: Lógica de negócio
- **Router**: Roteamento automático

### DRF ViewSets

```python
# Roteamento automático
from rest_framework.routers import DefaultRouter

router = DefaultRouter()
router.register(r'usuarios', UsuarioViewSet)
router.register(r'salas', SalaViewSet)
router.register(r'reservas', ReservaViewSet)

urlpatterns = router.urls
```

## 📊 Performance

### Otimizações Implementadas

```python
# Select Related (JOIN)
queryset = Reserva.objects.select_related('usuario', 'sala')

# Prefetch Related (Múltiplas queries otimizadas)
queryset = Usuario.objects.prefetch_related('reservas')

# Only (Selecionar campos específicos)
queryset = Usuario.objects.only('id', 'nome', 'email')

# Defer (Excluir campos pesados)
queryset = Usuario.objects.defer('senha')

# Índices no modelo
class Usuario(models.Model):
    email = models.EmailField(unique=True, db_index=True)
    
    class Meta:
        indexes = [
            models.Index(fields=['email']),
            models.Index(fields=['perfil']),
        ]
```

### Database Connection Pooling

```python
# settings.py
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql',
        'CONN_MAX_AGE': 600,  # Pool de conexões
        'OPTIONS': {
            'connect_timeout': 10,
        }
    }
}
```

## 🔒 Segurança

### Configurações de Segurança (settings.py)

```python
# Produção
DEBUG = False
ALLOWED_HOSTS = ['seu-dominio.com']

# HTTPS
SECURE_SSL_REDIRECT = True
SESSION_COOKIE_SECURE = True
CSRF_COOKIE_SECURE = True

# Security Headers
SECURE_BROWSER_XSS_FILTER = True
SECURE_CONTENT_TYPE_NOSNIFF = True
X_FRAME_OPTIONS = 'DENY'

# Password Validation
AUTH_PASSWORD_VALIDATORS = [
    {'NAME': 'django.contrib.auth.password_validation.UserAttributeSimilarityValidator'},
    {'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator'},
    {'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator'},
    {'NAME': 'django.contrib.auth.password_validation.NumericPasswordValidator'},
]
```

### Hash de Senhas

```python
from django.contrib.auth.hashers import make_password, check_password

# Criar hash
hashed = make_password('senha123')

# Verificar senha
is_valid = check_password('senha123', hashed)
```

## 🐛 Troubleshooting

### Erro: "No module named 'core'"

```bash
# Verificar INSTALLED_APPS no settings.py
INSTALLED_APPS = [
    # ...
    'core.apps.CoreConfig',
]
```

### Erro: "relation does not exist"

```bash
# Executar migrations
python manage.py makemigrations
python manage.py migrate
```

### Erro: "Port already in use"

```bash
# Usar porta diferente
python manage.py runserver 8001
```

## 📚 Recursos Adicionais

- [Documentação Django](https://docs.djangoproject.com/)
- [Django REST Framework](https://www.django-rest-framework.org/)
- [Django Best Practices](https://django-best-practices.readthedocs.io/)

## 🔥 Django Admin

Acesse o painel administrativo em `http://localhost:8000/admin`

```python
# core/admin.py
from django.contrib import admin
from .models import Usuario, Sala, Reserva

@admin.register(Usuario)
class UsuarioAdmin(admin.ModelAdmin):
    list_display = ['id', 'nome', 'email', 'perfil', 'criado_em']
    list_filter = ['perfil', 'criado_em']
    search_fields = ['nome', 'email']

@admin.register(Sala)
class SalaAdmin(admin.ModelAdmin):
    list_display = ['id', 'nome', 'tipo', 'capacidade', 'disponivel']
    list_filter = ['tipo', 'disponivel']
    search_fields = ['nome', 'localizacao']

@admin.register(Reserva)
class ReservaAdmin(admin.ModelAdmin):
    list_display = ['id', 'usuario', 'sala', 'data_reserva', 'hora_inicio']
    list_filter = ['data_reserva']
    date_hierarchy = 'data_reserva'
```

---

**Versão**: 1.0.0  
**Última Atualização**: Janeiro 2026
