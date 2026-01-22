from rest_framework import serializers
from django.db.models import Q
from django.contrib.auth.hashers import make_password, check_password
from .models import Usuario, Sala, Reserva
from rest_framework_simplejwt.serializers import TokenObtainPairSerializer
class UsuarioSerializer(serializers.ModelSerializer):
    senha = serializers.CharField(write_only=True, required=False)

    class Meta:
        model = Usuario
        fields = ['id', 'nome', 'email', 'senha', 'perfil', 'criado_em']
        read_only_fields = ['id', 'criado_em']

    def create(self, validated_data):
        # Senha é obrigatória no POST
        if 'senha' not in validated_data:
            raise serializers.ValidationError({'senha': 'Este campo é obrigatório.'})
        validated_data['senha'] = make_password(validated_data['senha'])
        return super().create(validated_data)

    def update(self, instance, validated_data):
        # Remove senha do update - não permitir alteração via PUT
        validated_data.pop('senha', None)
        return super().update(instance, validated_data)


class SalaSerializer(serializers.ModelSerializer):
    class Meta:
        model = Sala
        fields = ['id', 'nome', 'tipo', 'capacidade', 'localizacao', 'disponivel', 'criado_em']
        read_only_fields = ['id', 'criado_em']


class ReservaSerializer(serializers.ModelSerializer):
    # Permitir entrada via usuario_id e sala_id, mapeando para os FKs reais
    usuario_id = serializers.PrimaryKeyRelatedField(
        queryset=Usuario.objects.all(), source='usuario', write_only=True, required=False
    )
    sala_id = serializers.PrimaryKeyRelatedField(
        queryset=Sala.objects.all(), source='sala', write_only=True, required=False
    )

    class Meta:
        model = Reserva
        fields = [
            'id',
            'usuario', 'usuario_id',
            'sala', 'sala_id',
            'data_reserva', 'hora_inicio', 'hora_fim',
            'criado_em'
        ]
        read_only_fields = ['id', 'criado_em', 'usuario', 'sala']

    def validate(self, attrs):
        inicio = attrs.get('hora_inicio')
        fim = attrs.get('hora_fim')
        if inicio and fim and inicio >= fim:
            raise serializers.ValidationError("hora_inicio deve ser anterior a hora_fim")

        sala = attrs.get('sala') or getattr(self.instance, 'sala', None)
        data = attrs.get('data_reserva') or getattr(self.instance, 'data_reserva', None)
        if sala and data and inicio and fim:
            qs = Reserva.objects.filter(sala=sala, data_reserva=data)
            if self.instance:
                qs = qs.exclude(pk=self.instance.pk)
            overlap = qs.filter(Q(hora_inicio__lt=fim) & Q(hora_fim__gt=inicio)).exists()
            if overlap:
                raise serializers.ValidationError("Conflito: existe outra reserva para esta sala nesse horário")
        return attrs

class CustomTokenObtainPairSerializer(TokenObtainPairSerializer):
    """Serializer customizado para login com email + senha"""
    username_field = 'email'
    
    def validate(self, attrs):
        # Buscar usuário por email
        try:
            usuario = Usuario.objects.get(email=attrs.get('email'))
        except Usuario.DoesNotExist:
            raise serializers.ValidationError('Credenciais inválidas')
        
        # Verificar senha
        if not check_password(attrs.get('password'), usuario.senha):
            raise serializers.ValidationError('Credenciais inválidas')
        
        # Gerar token com perfil
        refresh = self.get_token(usuario)
        refresh['perfil'] = usuario.perfil
        return {
            'refresh': str(refresh),
            'access': str(refresh.access_token),
            'perfil': usuario.perfil,
        }
    
    @classmethod
    def get_token(cls, user):
        token = super().get_token(user)
        token['email'] = user.email
        token['perfil'] = user.perfil
        return token