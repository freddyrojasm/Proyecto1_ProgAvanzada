create database DB_SubastaArte

USE DB_SubastaArte
go

-- crear tabla de usuarios
Create Table Usuario(
Id int primary key identity (1,1),
Correo varchar(80) unique,
Clave varchar(20),
Nombre varchar(50),
FechaNacimiento datetime,
Genero nvarchar(20),
TipoUsuario nvarchar(30),
Imagen varbinary(max));


--procedimiento para validar registro de un nuevo usuario
create proc pr_CrearUsuario(
@Correo varchar(80),
@Clave varchar(20),
@Nombre varchar(50),
@FechaNacimiento datetime,
@Genero nvarchar(20),
@TipoUsuario nvarchar(30),
@Imagen varbinary(max),
@Registrado bit output,
@Mensaje varchar (100) output)
as
begin

if (not exists(select * from Usuario where Correo = @Correo))
begin

Insert into Usuario(Correo, Clave, Nombre,FechaNacimiento,Genero,TipoUsuario, Imagen) 
values(@Correo, @Clave, @Nombre, @FechaNacimiento, @Genero, @TipoUsuario, @Imagen)

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
Create proc pr_ValidarUsuario(
@Correo varchar(80),
@Clave varchar(20))
as
begin

if (exists(select * from Usuario where Correo = @Correo and Clave = @Clave))
select Id,TipoUsuario from Usuario where Correo = @Correo and Clave = @Clave
else
select '0'
end


--crear tabla de subasta
CREATE TABLE Subasta (
    Id INT PRIMARY KEY IDENTITY,
    PrecioInicial int NOT NULL,
    Duracion int NOT NULL,
    FechaHora DATETIME NOT NULL,
	Imagen varbinary(max),
    Comentarios NVARCHAR(MAX)
);

CREATE proc GuardarSubasta(
	@PrecioInicial int,
    @Duracion int,
    @FechaHora DATETIME,
    @Imagen varbinary(max),
    @Comentarios NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO GuardarSubasta (PrecioInicial, Duracion, FechaHora, Imagen, Comentarios)
    VALUES (@PrecioInicial,@Duracion, @FechaHora, @Imagen, @Comentarios)
END


--crear tabla de obra de arte
CREATE TABLE ObraArte (
    Id INT PRIMARY KEY IDENTITY,
    IdUsuario int NOT NULL,
    Categoria nvarchar(40) NOT NULL,
    Titulo nvarchar(40) NOT NULL,
	Descripcion nvarchar(max),
	DimensionLargo int not null,
	DimensionAncho int not null,
	Imagen varbinary(max),
);


create proc GuardarObra(
	@IdUsuario int,
    @Categoria nvarchar(40),
	@Titulo nvarchar(40),
    @Descripcion nvarchar (max),
	@DimensionLargo int,
	@DimensionAncho int,
    @Imagen varbinary(max))
AS
BEGIN
    INSERT INTO ObraArte (IdUsuario,Categoria,Titulo,Descripcion,DimensionLargo,DimensionAncho,Imagen)
    VALUES (@IdUsuario,@Categoria,@Titulo,@Descripcion,@DimensionLargo, @DimensionAncho,@Imagen)
END


-- sentencias adicionales


SELECT * 
FROM sys.procedures 
WHERE type_desc = 'SQL_STORED_PROCEDURE';