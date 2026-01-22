# This is an auto-generated Django model module.
# You'll have to do the following manually to clean this up:
#   * Rearrange models' order
#   * Make sure each model has one field with primary_key=True
#   * Make sure each ForeignKey and OneToOneField has `on_delete` set to the desired behavior
#   * Remove `managed = False` lines if you wish to allow Django to create, modify, and delete the table
# Feel free to rename the models, but don't rename db_table values or field names.
from django.db import models
from django.db.models import F, Q


class Usuario(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=255)
    email = models.EmailField(unique=True, max_length=255)
    senha = models.TextField()

    class Perfil(models.TextChoices):
        ADMIN = "admin", "admin"
        COMUM = "comum", "comum"
    perfil = models.CharField(max_length=10, choices=Perfil.choices, default=Perfil.COMUM)

    criado_em = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = "usuarios"
        managed = False

    def __str__(self):
        return self.nome


class Sala(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=255)

    class TipoSala(models.TextChoices):
        LAB = "laboratorio", "laboratorio"
        AULA = "aula", "aula"
        REUNIAO = "reuniao", "reuniao"
        AUDITORIO = "auditório", "auditório"
    tipo = models.CharField(max_length=20, choices=TipoSala.choices)

    capacidade = models.PositiveIntegerField()
    localizacao = models.CharField(max_length=255, blank=True, null=True)
    disponivel = models.BooleanField(default=True)
    criado_em = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = "salas"
        managed = False

    def __str__(self):
        return self.nome


class Reserva(models.Model):
    id = models.AutoField(primary_key=True)
    usuario = models.ForeignKey(Usuario, on_delete=models.CASCADE, db_column="usuario_id", related_name="reservas")
    sala = models.ForeignKey(Sala, on_delete=models.CASCADE, db_column="sala_id", related_name="reservas")
    data_reserva = models.DateField()
    hora_inicio = models.TimeField()
    hora_fim = models.TimeField()
    criado_em = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = "reservas"
        managed = False
        constraints = [
            models.UniqueConstraint(fields=["sala", "data_reserva", "hora_inicio", "hora_fim"], name="reserva_unica"),
            models.CheckConstraint(check=Q(hora_fim__gt=F("hora_inicio")), name="chk_horario"),
        ]
        indexes = [
            models.Index(fields=["usuario"], name="idx_reservas_usuario"),
            models.Index(fields=["sala"], name="idx_reservas_sala"),
        ]

    def __str__(self):
        return f"{self.sala} - {self.data_reserva} {self.hora_inicio}-{self.hora_fim}"
