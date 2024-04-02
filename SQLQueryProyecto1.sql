create database DBFITNESS_P1
USE DBFITNESS_P1


-- crear tabla de usuarios
Create Table Usuario(
Id int primary key identity (1,1),
Correo varchar(80) unique,
Clave varchar(20),
Nombre varchar(50),
FechaNacimiento datetime,
Altura int,
PesoActual decimal,
Genero nvarchar(20),
Imagen varbinary(max));


--procedimiento para validar registro de un nuevo usuario
Create proc pr_CrearUsuario(
@Correo varchar(80),
@Clave varchar(20),
@Nombre varchar(50),
@FechaNacimiento datetime,
@Altura int,
@PesoActual decimal,
@Genero nvarchar(20),
@Imagen varbinary(max),
@Registrado bit output,
@Mensaje varchar (100) output)
as
begin

if (not exists(select * from Usuario where Correo = @Correo))
begin

Insert into Usuario(Correo, Clave, Nombre,FechaNacimiento,Altura,PesoActual,Genero,Imagen) 
values(@Correo, @Clave, @Nombre, @FechaNacimiento, @Altura, @PesoActual, @Genero, @Imagen)

set @Registrado = 1
set @Mensaje = 'Usuario registrado'
end
else
begin
set @Registrado = 0
set @Mensaje = 'Correo ha sido registrado previamente'
end
end

--procedimiento para validar si el usuario está registrado
create proc pr_ValidarUsuario(
@Correo varchar(80),
@Clave varchar(20))
as
begin

if (exists(select * from Usuario where Correo = @Correo and Clave = @Clave))
select Id from Usuario where Correo = @Correo and Clave = @Clave
else
select '0'
end


--crear tabla de actividad física
CREATE TABLE ActividadFisica (
    IdActividad INT PRIMARY KEY IDENTITY,
    Correo varchar(80) NOT NULL,
    TipoActividad NVARCHAR(100) NOT NULL,
    FechaHora DATETIME NOT NULL,
    Duracion int NOT NULL,
    DistanciaRecorrida int NOT NULL,
    CaloriasQuemadas int not null,
    Comentarios NVARCHAR(MAX)
)

CREATE proc GuardarActividadFisica(
	@Correo varchar(80),
    @TipoActividad NVARCHAR(100),
    @FechaHora DATETIME,
    @Duracion int,
    @DistanciaRecorrida int,
    @CaloriasQuemadas int,
    @Comentarios NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO ActividadFisica (Correo, TipoActividad, FechaHora, Duracion, DistanciaRecorrida, CaloriasQuemadas, Comentarios)
    VALUES (@Correo,@TipoActividad, @FechaHora, @Duracion, @DistanciaRecorrida, @CaloriasQuemadas, @Comentarios)
END



CREATE TABLE Dieta (
    Id INT PRIMARY KEY IDENTITY,
    Correo varchar(80) NOT NULL,
    FechaComida DATE not null,
    TipoComida NVARCHAR(100) not null,
    AlimentosConsumidos NVARCHAR(MAX) not null,
    CaloriasTotales int not null,
    Comentarios NVARCHAR(MAX)
	);
Create proc GuardarDieta(
	@Correo varchar(80),
    @FechaComida DATE,
    @TipoComida NVARCHAR(100),
    @AlimentosConsumidos NVARCHAR(MAX),
    @CaloriasTotales int,
    @Comentarios NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO Dieta (Correo,FechaComida,TipoComida,AlimentosConsumidos,CaloriasTotales,Comentarios)
    VALUES (@Correo,@FechaComida,@TipoComida,@AlimentosConsumidos,@CaloriasTotales,@Comentarios)
END


--Creación de tabla y procedimiento Meta Salud
CREATE TABLE MetaSalud (
    IdMeta int primary key identity (1,1),
    Correo varchar(80) NOT NULL,
    TipoMeta VARCHAR(80) not null,
    PesoObjetivo Decimal not null,
	FechaObjetivo DATE not null,
    NivelActividadDes NVARCHAR(80) not null,
    ObjEspecificos NVARCHAR(MAX)
	);

CREATE proc GuardarMetaSalud(
	@Correo varchar(80),
    @TipoMeta VARCHAR(80),
    @PesoObjetivo Decimal,
	@FechaObjetivo DATE,
    @NivelActividadDes NVARCHAR(80),
    @ObjEspecificos NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO MetaSalud (Correo,TipoMeta,PesoObjetivo,FechaObjetivo,NivelActividadDes,ObjEspecificos)
    VALUES (@Correo,@TipoMeta,@PesoObjetivo,@FechaObjetivo,@NivelActividadDes,@ObjEspecificos)
END



-- sentencias adicionales

select * from ActividadFisica