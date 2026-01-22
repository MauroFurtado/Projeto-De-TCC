from django.shortcuts import render
from rest_framework import viewsets, permissions
from rest_framework.decorators import action
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from django.contrib.auth.hashers import check_password
from .models import Usuario, Sala, Reserva
from .serializers import UsuarioSerializer, SalaSerializer, ReservaSerializer, CustomTokenObtainPairSerializer
from .permissions import IsAuthenticatedOrDisabled, AllowAnyOrDisabled

class LoginView(TokenObtainPairView):
    """
    POST /login/
    Body: { "email": "...", "password": "..." }
    Response: { "access": "...", "refresh": "...", "perfil": "..." }
    """
    serializer_class = CustomTokenObtainPairSerializer
    permission_classes = [AllowAnyOrDisabled]

class UsuarioViewSet(viewsets.ModelViewSet):
    """
    GET /usuarios/       -> list
    GET /usuarios/{id}/  -> retrieve
    POST /usuarios/      -> create
    PUT /usuarios/{id}/  -> update
    DELETE /usuarios/{id}/ -> destroy
    """
    queryset = Usuario.objects.all()
    serializer_class = UsuarioSerializer
    permission_classes = [IsAuthenticatedOrDisabled]

class SalaViewSet(viewsets.ModelViewSet):
    queryset = Sala.objects.all()
    serializer_class = SalaSerializer
    permission_classes = [IsAuthenticatedOrDisabled]

class ReservaViewSet(viewsets.ModelViewSet):
    queryset = Reserva.objects.all().select_related('usuario', 'sala')
    serializer_class = ReservaSerializer
    permission_classes = [IsAuthenticatedOrDisabled]

    def perform_create(self, serializer):
        # Se o payload já trouxe usuario via usuario_id, apenas salvar
        if 'usuario' in serializer.validated_data:
            serializer.save()
            return

        # Caso contrário, tentar associar pelo usuário autenticado, se existir
        user = getattr(self.request, 'user', None)
        if user and user.is_authenticated:
            try:
                usuario_obj = Usuario.objects.get(email=getattr(user, "email", None))
                serializer.save(usuario=usuario_obj)
                return
            except Usuario.DoesNotExist:
                pass
        serializer.save()
