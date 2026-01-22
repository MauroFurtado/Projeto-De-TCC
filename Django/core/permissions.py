from rest_framework import permissions
from django.conf import settings

class IsAuthenticatedOrDisabled(permissions.BasePermission):
    """Permite acesso se autenticado OU se AUTH_DISABLED=true"""
    def has_permission(self, request, view):
        if settings.AUTH_DISABLED:
            return True
        return request.user and request.user.is_authenticated

class AllowAnyOrDisabled(permissions.BasePermission):
    """Sempre permite acesso"""
    def has_permission(self, request, view):
        return True