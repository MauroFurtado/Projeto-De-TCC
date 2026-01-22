from rest_framework import routers
from rest_framework_simplejwt.views import TokenRefreshView
from django.urls import path, include
from .views import UsuarioViewSet, SalaViewSet, ReservaViewSet, LoginView

router = routers.DefaultRouter(trailing_slash=False)
router.register(r'usuarios', UsuarioViewSet, basename='usuario')
router.register(r'salas', SalaViewSet, basename='sala')
router.register(r'reservas', ReservaViewSet, basename='reserva')

urlpatterns = [
    path('login/', LoginView.as_view(), name='login'),
    path('token/refresh/', TokenRefreshView.as_view(), name='token_refresh'),
    path('', include(router.urls)),
]