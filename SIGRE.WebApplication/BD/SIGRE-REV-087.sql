USE [master]
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SIGRE')
	BEGIN
		ALTER DATABASE [SIGRE] SET single_user WITH ROLLBACK IMMEDIATE
		DROP DATABASE [SIGRE]
	END
GO
CREATE DATABASE [SIGRE]
GO
USE [SIGRE]
GO
CREATE TABLE [dbo].[categoriadetalle](
	[idcategoriadetalle] [int] IDENTITY(1,1) NOT NULL,
	[idcategoriainventario] [int] NOT NULL,
	[idtipodato] [int] NOT NULL,
	[idatributo] [int] NULL,
	[idformato] [int] NOT NULL,
	[nombre] [nvarchar](30) NOT NULL,
	[identificador] [bit] NOT NULL DEFAULT ((0)),
	[obligatorio] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idcategoriadetalle] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[categoriainventario](
	[idcategoriainventario] [int] IDENTITY(1,1) NOT NULL,
	[idcategoriainventariotipo] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](200) NULL,
	[desactivado] [bit] NOT NULL DEFAULT ((0)),
	[multiple] [bit] NOT NULL DEFAULT ((0)),
	[utilizada] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idcategoriainventario] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[categoriainventariotipo](
	[idcategoriainventariotipo] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](50) NOT NULL,
PRIMARY KEY
(
	[idcategoriainventariotipo] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[centrocosto](
	[idcentrocosto] [int] IDENTITY(1,1) NOT NULL,
	[centrocosto_idcentrocosto] [int] NULL,
	[idcolaborador] [int] NOT NULL,
	[codigo] [char](8) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[desactivado] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idcentrocosto] ASC
)
)

GO

GO

GO

GO

GO
CREATE TABLE [dbo].[colaborador](
	[idcolaborador] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [char](7) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[desactivado] [bit] NOT NULL DEFAULT ((0)),
	[nda] [bit] NOT NULL DEFAULT ((0)),
	[foto] [varbinary](max) NULL,
	[col_1] [int] NULL,
	[colaborador_idcolaborador] [int] NOT NULL,
	[idcolaboradortipo] [int] NOT NULL,
	[idperfil] [int] NULL,
	[idusuario] [int] NULL,
	[idcentrocosto] [int] NOT NULL,
	[aprobado] [bit] NULL DEFAULT (NULL),
	[correo] [nvarchar](50) NULL,
PRIMARY KEY
(
	[idcolaborador] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[colaboradortipo](
	[idcolaboradortipo] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](50) NOT NULL,
PRIMARY KEY
(
	[idcolaboradortipo] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[iteminventario](
	[iditeminventario] [int] IDENTITY(1,1) NOT NULL,
	[idcategoriainventario] [int] NOT NULL,
	[idcolaborador] [int] NULL,
	[descripcion] [nvarchar](50) NOT NULL,
	[fechamodificacion] [date] NULL,
	[detallemodificacion] [nvarchar](100) NULL,
	[tipooperacion] [char](1) NOT NULL DEFAULT ('D'),
PRIMARY KEY
(
	[iditeminventario] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[iteminventariodetalle](
	[iditeminventariodetalle] [int] IDENTITY(1,1) NOT NULL,
	[iditeminventario] [int] NOT NULL,
	[idcategoriadetalle] [int] NOT NULL,
	[valorcadena] [nvarchar](1000) NULL,
	[valorentero] [int] NULL,
	[valordecimal] [decimal](11, 4) NULL,
	[valorfecha] [date] NULL,
	[valorbusqueda] [nvarchar](1000) NULL,
	[monto] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[iditeminventariodetalle] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[opcion](
	[idopcion] [int] IDENTITY(1,1) NOT NULL,
	[idsuperior] [int] NULL,
	[link] [nvarchar](100) NOT NULL,
	[action] [nvarchar](100) NOT NULL,
	[controller] [nvarchar](100) NULL,
	[area] [nvarchar](100) NULL,
	[a] [bit] NOT NULL DEFAULT ((0)),
	[sesion] [int] NOT NULL DEFAULT ((0)),
	[dinamico] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idopcion] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[perfil](
	[idperfil] [int] IDENTITY(1,1) NOT NULL,
	[idcolaborador] [int] NOT NULL,
	[idcentrocosto] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](100) NOT NULL,
	[costo] [decimal](11, 2) NOT NULL DEFAULT ((0)),
	[aprobado] [bit] NULL,
	[desactivado] [bit] NOT NULL DEFAULT ((0)),
	[asignado] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idperfil] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[perfilrecurso](
	[idperfil] [int] NOT NULL,
	[idrecurso] [int] NOT NULL,
	[montocalculado] [decimal](11, 2) NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idperfil] ASC,
	[idrecurso] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[recurso](
	[idrecurso] [int] IDENTITY(1,1) NOT NULL,
	[idcolaborador] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](100) NOT NULL,
	[costo] [decimal](11, 2) NOT NULL DEFAULT ((0)),
	[foto] [varbinary](max) NULL,
	[desactivado] [bit] NOT NULL DEFAULT ((0)),
	[aprobado] [bit] NULL,
PRIMARY KEY
(
	[idrecurso] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[rol](
	[idrol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](100) NOT NULL,
	[dinamico] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idrol] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[rolopcion](
	[idrol] [int] NOT NULL,
	[idopcion] [int] NOT NULL,
	[idsuperior] [int] NULL,
PRIMARY KEY
(
	[idrol] ASC,
	[idopcion] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[solicitudcolaboradorperfil](
	[idcolaboradorperfil] [int] IDENTITY(1,1) NOT NULL,
	[idcolaborador] [int] NOT NULL,
	[idperfil] [int] NOT NULL,
	[fecha] [date] NULL,
	[comentario] [nvarchar](100) NULL,
	[aprobado] [bit] NULL,
	[revocacion] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idcolaboradorperfil] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[tipodato](
	[idtipodato] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](30) NOT NULL,
PRIMARY KEY
(
	[idtipodato] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[tipodatoatributo](
	[idatributo] [int] IDENTITY(1,1) NOT NULL,
	[idtipodato] [int] NOT NULL,
	[codigo] [nvarchar](10) NOT NULL,
	[descripcion] [nvarchar](30) NOT NULL,
	[idoperador] [int] NULL,
	[idreferencia] [int] NULL,
	[valorcadena] [nvarchar](1000) NULL,
	[valorentero] [int] NULL,
	[etiqueta] [nvarchar](1000) NULL,
PRIMARY KEY
(
	[idatributo] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[tipodatoformato](
	[idtipodato] [int] NOT NULL,
	[idformato] [int] NOT NULL,
	[formato] [nvarchar](30) NOT NULL,
	[longitud] [int] NOT NULL,
	[escala] [int] NOT NULL,
	[descripcion] [nvarchar](60) NOT NULL,
	[tipo] [char](1) NOT NULL,
	[activo] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idtipodato] ASC,
	[idformato] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[tipodatooperador](
	[idtipodato] [int] NOT NULL,
	[idoperador] [int] NOT NULL,
	[descripcion] [nvarchar](30) NOT NULL,
PRIMARY KEY
(
	[idtipodato] ASC,
	[idoperador] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[tipodatoreferencia](
	[idtipodato] [int] NOT NULL,
	[idreferencia] [int] NOT NULL,
	[descripcion] [nvarchar](30) NOT NULL,
	[valorentero] [int] NULL,
PRIMARY KEY
(
	[idtipodato] ASC,
	[idreferencia] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
PRIMARY KEY
(
	[UserId] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[usuario](
	[idusuario] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [char](32) NULL,
	[idrol] [int] NOT NULL,
	[intentos] [int] NOT NULL DEFAULT ((0)),
	[intentospass] [int] NOT NULL DEFAULT ((0)),
	[bloqueado] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY
(
	[idusuario] ASC
)
)

GO

GO

GO

GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY
(
	[UserId] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY
(
	[Provider] ASC,
	[ProviderUserId] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY
(
	[RoleId] ASC
)
)

GO

GO

GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY
(
	[UserId] ASC,
	[RoleId] ASC
)
)

GO
SET IDENTITY_INSERT [dbo].[categoriadetalle] ON 

INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1, 1, 2, NULL, 1, N'N° serie', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (2, 1, 2, NULL, 1, N'Marca', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (3, 1, 3, 3, 6, N'Fecha de compra', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (4, 1, 5, 2, 9, N'Precio sin IGV', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (5, 1, 2, NULL, 1, N'Procesador', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (9, 3, 2, NULL, 1, N'N° serie', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (10, 3, 1, NULL, 1, N'Marca', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (11, 3, 2, NULL, 1, N'Modelo', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (12, 4, 2, NULL, 2, N'N° de testimonio', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (13, 4, 3, NULL, 6, N'Fecha de otorgamiento', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (14, 4, 3, 4, 6, N'Fecha de Vencimiento', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1026, 7, 1, NULL, 1, N'campo 1', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1027, 7, 5, 2, 8, N'Precio 1', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1028, 7, 5, NULL, 9, N'Precio 2', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1029, 7, 5, 2, 10, N'Precio 4', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1030, 7, 4, 11, 7, N'Year', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1031, 7, 1, 10, 2, N'Fabri', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1032, 5, 1, NULL, 1, N'#serie', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1033, 5, 1, 12, 1, N'Made in', 0, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1034, 5, 4, NULL, 9, N'Versión', 0, 0)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1035, 2, 2, NULL, 1, N'Número 2', 1, 1)
INSERT [dbo].[categoriadetalle] ([idcategoriadetalle], [idcategoriainventario], [idtipodato], [idatributo], [idformato], [nombre], [identificador], [obligatorio]) VALUES (1036, 2, 5, NULL, 9, N'Línea', 0, 1)
SET IDENTITY_INSERT [dbo].[categoriadetalle] OFF
SET IDENTITY_INSERT [dbo].[categoriainventario] ON 

INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (1, 1, N'Laptop', N'Laptop estándar', 0, 0, 1)
INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (2, 2, N'Tarjeta de Crédito Clásica', N'Para jefaturas', 0, 0, 0)
INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (3, 1, N'Impresora Láser Personal', N'Para gerentes', 0, 0, 1)
INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (4, 2, N'Poder notarial', N'Para actos de la empresa', 0, 1, 1)
INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (5, 1, N'iPad', N'Para la fuerza de ventas de Pharma', 0, 0, 1)
INSERT [dbo].[categoriainventario] ([idcategoriainventario], [idcategoriainventariotipo], [nombre], [descripcion], [desactivado], [multiple], [utilizada]) VALUES (7, 1, N'Categoría 2', N'Categoría 2', 0, 1, 1)
SET IDENTITY_INSERT [dbo].[categoriainventario] OFF
SET IDENTITY_INSERT [dbo].[categoriainventariotipo] ON 

INSERT [dbo].[categoriainventariotipo] ([idcategoriainventariotipo], [descripcion]) VALUES (1, N'Tangible')
INSERT [dbo].[categoriainventariotipo] ([idcategoriainventariotipo], [descripcion]) VALUES (2, N'Intangible')
SET IDENTITY_INSERT [dbo].[categoriainventariotipo] OFF
SET IDENTITY_INSERT [dbo].[centrocosto] ON 

INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (1, NULL, 1, N'MS      ', N'Merck Serono', 0)
INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (2, NULL, 2, N'CHC     ', N'Consumer Health Care', 0)
INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (3, NULL, 3, N'MM      ', N'Merck Millipore', 0)
INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (4, 1, 1, N'MSCC    ', N'MS-CC', 0)
INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (5, 2, 2, N'CHCCC   ', N'CHC-CC', 0)
INSERT [dbo].[centrocosto] ([idcentrocosto], [centrocosto_idcentrocosto], [idcolaborador], [codigo], [nombre], [desactivado]) VALUES (6, 3, 3, N'MMCC    ', N'MM-CC', 0)
SET IDENTITY_INSERT [dbo].[centrocosto] OFF
SET IDENTITY_INSERT [dbo].[colaborador] ON 

INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (1, N'M000001', N'Arnaud Coelho', 0, 1, NULL, NULL, 1, 1, NULL, 4, 4, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (2, N'M000002', N'Andres Garcia', 0, 1, NULL, NULL, 2, 1, NULL, 7, 5, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (3, N'M000003', N'Sergio Sepulveda', 0, 1, NULL, NULL, 3, 1, NULL, NULL, 6, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (4, N'M000004', N'Hector Fernandez', 0, 1, NULL, NULL, 1, 1, NULL, 5, 4, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (5, N'M000005', N'Laura Guevara', 0, 1, NULL, NULL, 2, 1, NULL, NULL, 5, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (6, N'M000006', N'Jorge Rimari', 0, 1, NULL, NULL, 3, 1, NULL, NULL, 6, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (7, N'M000007', N'Sergio Ferreira', 0, 1, NULL, NULL, 1, 1, NULL, NULL, 4, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (8, N'M000008', N'Jackeline Oshiro', 0, 1, NULL, NULL, 1, 1, 1, 6, 4, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (9, N'M000009', N'Mario Perez', 0, 1, NULL, NULL, 2, 1, NULL, NULL, 5, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (10, N'M000010', N'Jorge León', 0, 1, NULL, NULL, 2, 1, NULL, 1, 5, 1, N'tegobijava@gmail.com')
INSERT [dbo].[colaborador] ([idcolaborador], [codigo], [nombre], [desactivado], [nda], [foto], [col_1], [colaborador_idcolaborador], [idcolaboradortipo], [idperfil], [idusuario], [idcentrocosto], [aprobado], [correo]) VALUES (11, N'M000011', N'Leticia Quintana', 0, 1, NULL, NULL, 3, 1, NULL, 2, 6, 1, N'tegobijava@gmail.com')
SET IDENTITY_INSERT [dbo].[colaborador] OFF
SET IDENTITY_INSERT [dbo].[colaboradortipo] ON 

INSERT [dbo].[colaboradortipo] ([idcolaboradortipo], [descripcion]) VALUES (1, N'Interno')
INSERT [dbo].[colaboradortipo] ([idcolaboradortipo], [descripcion]) VALUES (2, N'Externo')
SET IDENTITY_INSERT [dbo].[colaboradortipo] OFF
SET IDENTITY_INSERT [dbo].[iteminventario] ON 

INSERT [dbo].[iteminventario] ([iditeminventario], [idcategoriainventario], [idcolaborador], [descripcion], [fechamodificacion], [detallemodificacion], [tipooperacion]) VALUES (1, 3, 10, N'Gerencia de Producto', NULL, NULL, N'A')
INSERT [dbo].[iteminventario] ([iditeminventario], [idcategoriainventario], [idcolaborador], [descripcion], [fechamodificacion], [detallemodificacion], [tipooperacion]) VALUES (2, 4, 1, N'Firma de contratos', NULL, NULL, N'A')
INSERT [dbo].[iteminventario] ([iditeminventario], [idcategoriainventario], [idcolaborador], [descripcion], [fechamodificacion], [detallemodificacion], [tipooperacion]) VALUES (3, 1, 4, N'LAPPEMS-145', NULL, NULL, N'P')
INSERT [dbo].[iteminventario] ([iditeminventario], [idcategoriainventario], [idcolaborador], [descripcion], [fechamodificacion], [detallemodificacion], [tipooperacion]) VALUES (5, 4, 9, N'Power', NULL, NULL, N'A')
SET IDENTITY_INSERT [dbo].[iteminventario] OFF
SET IDENTITY_INSERT [dbo].[iteminventariodetalle] ON 

INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (1, 1, 9, N'HP12345', NULL, NULL, NULL, N'HP12345', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (2, 1, 10, N'HP', NULL, NULL, NULL, N'HP', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (3, 1, 11, N'SL-325', NULL, NULL, NULL, N'SL-325', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (4, 2, 12, N'PE2014-ABR 001', NULL, NULL, NULL, N'PE2014-ABR 001', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (5, 2, 13, NULL, NULL, NULL, CAST(N'2014-01-03' AS Date), N'03/01/2014', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (6, 2, 14, NULL, NULL, NULL, CAST(N'2015-04-30' AS Date), N'30/04/2015', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (7, 3, 1, N'AS125467', NULL, NULL, NULL, N'AS125467', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (8, 3, 2, N'Dell', NULL, NULL, NULL, N'Dell', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (9, 3, 3, NULL, NULL, NULL, CAST(N'2014-02-28' AS Date), N'28/02/2014', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (10, 3, 4, NULL, NULL, CAST(2500.4600 AS Decimal(11, 4)), NULL, N'2500.46', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (11, 3, 5, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (18, 5, 12, N'232323', NULL, NULL, NULL, N'232323', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (19, 5, 13, NULL, NULL, NULL, CAST(N'2014-05-31' AS Date), N'31/05/2014', 0)
INSERT [dbo].[iteminventariodetalle] ([iditeminventariodetalle], [iditeminventario], [idcategoriadetalle], [valorcadena], [valorentero], [valordecimal], [valorfecha], [valorbusqueda], [monto]) VALUES (20, 5, 14, NULL, NULL, NULL, CAST(N'2014-12-31' AS Date), N'31/12/2014', 0)
SET IDENTITY_INSERT [dbo].[iteminventariodetalle] OFF
SET IDENTITY_INSERT [dbo].[opcion] ON 

INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (1, NULL, N'Consultas', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (2, NULL, N'Administración', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (3, NULL, N'Perfiles', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (4, NULL, N'Inventario', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (5, NULL, N'Seguridad', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (6, 2, N'Recursos', N'Index', N'Recurso', N'ADMIN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (7, 2, N'Colaboradores', N'Index', N'Colaborador', N'ADMIN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (8, 2, N'Centros de costo', N'Index', N'CentroCosto', N'ADMIN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (9, 3, N'Gestionar', N'Index', N'Perfil', N'PERFIL', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (10, 4, N'Categorías', N'Index', N'CategoriaInventario2', N'INVEN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (11, 4, N'Ítems', N'Index', N'ItemInventario2', N'INVEN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (12, 4, N'Formato de tipo de dato', N'Create', N'TipoDatoFormato', N'INVEN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (13, 4, N'Atributo de tipo de dato', N'Create', N'TipoDatoAtributo', N'INVEN', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (14, 5, N'Usuarios', N'Create', N'Usuario', N'SEGU', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (15, 5, N'Roles', N'Index', N'Rol', N'SEGU', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (16, 1, N'Gasto en recursos', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (19, 1, N'Renovación de recursos', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (20, 3, N'Consultar asignación', N'IndexConsultar', N'ColaboradorPerfil', N'PERFIL', 0, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (22, NULL, N'Aprobación', N'#', NULL, NULL, 1, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (23, 22, N'Perfiles', N'IndexAprobar', N'Perfil', N'PERFIL', 0, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (24, 22, N'Asignación de perfil', N'IndexAsignar', N'ColaboradorPerfil', N'PERFIL', 0, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (25, 22, N'Revocación de perfil', N'IndexRevocar', N'ColaboradorPerfil', N'PERFIL', 0, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (26, 22, N'Recursos', N'IndexAprobar', N'Recurso', N'ADMIN', 0, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (27, 22, N'Colaboradores', N'IndexAprobar', N'Colaborador', N'ADMIN', 0, 0, 1)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (28, 4, N'Parámetros para reportes', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (29, 1, N'Emitir acta de entrega', N'#', NULL, NULL, 1, 0, 0)
INSERT [dbo].[opcion] ([idopcion], [idsuperior], [link], [action], [controller], [area], [a], [sesion], [dinamico]) VALUES (30, 1, N'Emitir acta de devolución', N'#', NULL, NULL, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[opcion] OFF
SET IDENTITY_INSERT [dbo].[perfil] ON 

INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (1, 1, 1, N'Key Account Manager', N'Key Account Manager', CAST(650.00 AS Decimal(11, 2)), 1, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (2, 1, 1, N'Representante de ventas', N'Representante de ventas', CAST(650.00 AS Decimal(11, 2)), 1, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (3, 2, 2, N'Gerente de Producto', N'Gerente de Producto', CAST(7920.00 AS Decimal(11, 2)), 1, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (4, 2, 2, N'Gerente de Marketing', N'Gerente de Marketing', CAST(650.00 AS Decimal(11, 2)), 1, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (7, 1, 1, N'1', N'1', CAST(5350.00 AS Decimal(11, 2)), NULL, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (8, 1, 1, N'iuiuiu', N'kjkj', CAST(2370.00 AS Decimal(11, 2)), NULL, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (1007, 2, 2, N'Administrador de Red', N'Admin', CAST(4700.00 AS Decimal(11, 2)), NULL, 0, 0)
INSERT [dbo].[perfil] ([idperfil], [idcolaborador], [idcentrocosto], [nombre], [descripcion], [costo], [aprobado], [desactivado], [asignado]) VALUES (1010, 3, 3, N'w', N'w', CAST(5300.00 AS Decimal(11, 2)), NULL, 0, 0)
SET IDENTITY_INSERT [dbo].[perfil] OFF
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1, 3, CAST(650.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (2, 3, CAST(650.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (3, 1, CAST(3500.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (3, 2, CAST(1200.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (3, 4, CAST(600.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (3, 5, CAST(1120.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (3, 6, CAST(1500.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (4, 3, CAST(650.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (7, 1, CAST(3500.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (7, 2, CAST(1200.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (7, 3, CAST(650.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (8, 3, CAST(650.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (8, 4, CAST(600.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (8, 5, CAST(1120.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1007, 1, CAST(3500.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1007, 4, CAST(600.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1007, 8, CAST(600.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1010, 1, CAST(3500.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1010, 2, CAST(1200.00 AS Decimal(11, 2)))
INSERT [dbo].[perfilrecurso] ([idperfil], [idrecurso], [montocalculado]) VALUES (1010, 4, CAST(600.00 AS Decimal(11, 2)))
SET IDENTITY_INSERT [dbo].[recurso] ON 

INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (1, 4, N'e-workplace', N'e-workplace', CAST(3500.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (2, 4, N'Business laptop', N'Business laptop', CAST(1200.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (3, 4, N'PC', N'PC', CAST(650.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (4, 4, N'Smartphone', N'Smartphone', CAST(600.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (5, 4, N'Plan 300', N'Plan 300', CAST(1120.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (6, 4, N'Datos 5GB', N'Datos 5GB', CAST(1500.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (7, 5, N'Tarjeta de crédito', N'Tarjeta de crédito', CAST(300.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (8, 5, N'Equifax', N'Equifax', CAST(600.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (9, 6, N'Batch de acceso', N'Batch de acceso', CAST(0.00 AS Decimal(11, 2)), NULL, 0, 1)
INSERT [dbo].[recurso] ([idrecurso], [idcolaborador], [nombre], [descripcion], [costo], [foto], [desactivado], [aprobado]) VALUES (11, 1, N'm', N'm', CAST(5.00 AS Decimal(11, 2)), NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[recurso] OFF
SET IDENTITY_INSERT [dbo].[rol] ON 

INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (1, N'Consultor de IT', N'Consultor de IT', 0)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (2, N'Facilitador de Recursos', N'Facilitador de Recursos', 0)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (3, N'Administrador', N'Administrador', 0)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (4, N'Usuario Dashboard', N'Usuario Dashboard', 0)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (5, N'Dirección General', N'Dirección General', 0)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (6, N'Aprobador de Colaborador', N'Aprobador de Colaborador', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (7, N'Propietario de Recurso', N'Propietario de Recurso', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (8, N'Super Administrador', N'Super Administrador', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (9, N'Aprobador de Perfil', N'Aprobador de Perfil', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (10, N'Aprobador de Asignación', N'Aprobador de Asignación', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [dinamico]) VALUES (11, N'Aprobador de Revocación', N'Aprobador de Revocación', 1)
SET IDENTITY_INSERT [dbo].[rol] OFF
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 2, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 3, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 6, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 7, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 8, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (1, 9, 3)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 3, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 4, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 10, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 11, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 12, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 13, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 20, 3)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 28, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 29, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (2, 30, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (3, 5, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (3, 14, 5)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (3, 15, 5)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (4, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (4, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (5, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (5, 19, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (6, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (6, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (6, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (6, 27, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (7, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (7, 26, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 2, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 3, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 4, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 5, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 6, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 7, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 8, 2)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 9, 3)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 10, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 11, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 12, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 13, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 14, 5)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 15, 5)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 19, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 20, 3)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 23, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 24, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 25, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 26, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 27, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 28, 4)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 29, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (8, 30, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (9, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (9, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (9, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (9, 23, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (10, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (10, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (10, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (10, 24, 22)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (11, 1, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (11, 16, 1)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (11, 22, NULL)
INSERT [dbo].[rolopcion] ([idrol], [idopcion], [idsuperior]) VALUES (11, 25, 22)
SET IDENTITY_INSERT [dbo].[solicitudcolaboradorperfil] ON 

INSERT [dbo].[solicitudcolaboradorperfil] ([idcolaboradorperfil], [idcolaborador], [idperfil], [fecha], [comentario], [aprobado], [revocacion]) VALUES (1, 8, 1, NULL, NULL, NULL, 1)
INSERT [dbo].[solicitudcolaboradorperfil] ([idcolaboradorperfil], [idcolaborador], [idperfil], [fecha], [comentario], [aprobado], [revocacion]) VALUES (2, 2, 4, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[solicitudcolaboradorperfil] OFF
SET IDENTITY_INSERT [dbo].[tipodato] ON 

INSERT [dbo].[tipodato] ([idtipodato], [descripcion]) VALUES (1, N'Texto')
INSERT [dbo].[tipodato] ([idtipodato], [descripcion]) VALUES (2, N'Alfanúmerico')
INSERT [dbo].[tipodato] ([idtipodato], [descripcion]) VALUES (3, N'Fecha')
INSERT [dbo].[tipodato] ([idtipodato], [descripcion]) VALUES (4, N'Número')
INSERT [dbo].[tipodato] ([idtipodato], [descripcion]) VALUES (5, N'Monto')
SET IDENTITY_INSERT [dbo].[tipodato] OFF
SET IDENTITY_INSERT [dbo].[tipodatoatributo] ON 

INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (1, 5, N'PRE', N'Precio', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (2, 5, N'VC', N'Valor de compra', 3, 2, NULL, 0, N'> Cero (0)')
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (3, 3, N'FC', N'Fecha de compra', 6, 1, NULL, NULL, N'≤ Fecha de hoy')
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (4, 3, N'FV', N'Fecha de vencimiento', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (5, 4, N'AG', N'Años de garantía', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (9, 2, N'MARC', N'Marca', 1, NULL, N'Akita', NULL, N'≠ Akita')
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (10, 1, N'FABR', N'Fabricante', 2, NULL, N'Samsung', NULL, N'= Samsung')
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (11, 4, N'YEAR', N'Años', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tipodatoatributo] ([idatributo], [idtipodato], [codigo], [descripcion], [idoperador], [idreferencia], [valorcadena], [valorentero], [etiqueta]) VALUES (12, 1, N'PROC', N'Procedencia', 7, NULL, N'China', NULL, N'⊇ China')
SET IDENTITY_INSERT [dbo].[tipodatoatributo] OFF
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (1, 1, N'30', 30, 0, N'Texto 30', N'C', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (1, 2, N'50', 50, 0, N'Texto 50', N'C', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (1, 3, N'100', 100, 0, N'Texto 100', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (1, 4, N'200', 200, 0, N'Texto 200', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (1, 5, N'Máximo', 1000, 0, N'Texto Máximo', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (2, 1, N'30', 30, 0, N'Alfanúmerico 30', N'C', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (2, 2, N'50', 50, 0, N'Alfanúmerico 50', N'C', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (2, 3, N'100', 100, 0, N'Alfanúmerico 100', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (2, 4, N'200', 200, 0, N'Alfanúmerico 200', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (2, 5, N'Máximo', 1000, 0, N'Alfanúmerico Máximo', N'C', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (3, 6, N'dd/mm/aaaa', 10, 0, N'Fecha dd/mm/aaaa', N'F', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (4, 7, N'Entero', 11, 0, N'Número Entero', N'E', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (4, 8, N'Decimal, 1', 11, 1, N'Número Decimal, 1', N'D', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (4, 9, N'Decimal, 2', 11, 2, N'Número Decimal, 2', N'D', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (4, 10, N'Decimal, 4', 11, 4, N'Número Decimal, 4', N'D', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (5, 7, N'Entero', 11, 0, N'Monto Entero', N'E', 0)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (5, 8, N'Decimal, 1', 11, 1, N'Monto Decimal, 1', N'D', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (5, 9, N'Decimal, 2', 11, 2, N'Monto Decimal, 2', N'D', 1)
INSERT [dbo].[tipodatoformato] ([idtipodato], [idformato], [formato], [longitud], [escala], [descripcion], [tipo], [activo]) VALUES (5, 10, N'Decimal, 4', 11, 4, N'Monto Decimal, 4', N'D', 1)
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (1, 1, N'≠')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (1, 2, N'=')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (1, 7, N'⊇')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (2, 1, N'≠')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (2, 2, N'=')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (2, 7, N'⊇')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 1, N'≠')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 2, N'=')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 3, N'>')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 4, N'<')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 5, N'≥')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (3, 6, N'≤')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 1, N'≠')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 2, N'=')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 3, N'>')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 4, N'<')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 5, N'≥')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (4, 6, N'≤')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 1, N'≠')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 2, N'=')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 3, N'>')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 4, N'<')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 5, N'≥')
INSERT [dbo].[tipodatooperador] ([idtipodato], [idoperador], [descripcion]) VALUES (5, 6, N'≤')
INSERT [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia], [descripcion], [valorentero]) VALUES (3, 1, N'Fecha de hoy', NULL)
INSERT [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia], [descripcion], [valorentero]) VALUES (4, 2, N'Cero (0)', 0)
INSERT [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia], [descripcion], [valorentero]) VALUES (4, 3, N'Uno (1)', 1)
INSERT [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia], [descripcion], [valorentero]) VALUES (5, 2, N'Cero (0)', 0)
INSERT [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia], [descripcion], [valorentero]) VALUES (5, 3, N'Uno (1)', 1)
SET IDENTITY_INSERT [dbo].[usuario] ON 

INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (1, N'M000010', N'bfbe04c28f819d2fa626d886a2bb1597', 1, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (2, N'M000011', N'bfbe04c28f819d2fa626d886a2bb1597', 3, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (3, N'ADMIN', N'bfbe04c28f819d2fa626d886a2bb1597', 8, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (4, N'M000001', N'bfbe04c28f819d2fa626d886a2bb1597', 4, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (5, N'M000004', N'bfbe04c28f819d2fa626d886a2bb1597', 5, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (6, N'M000008', N'bfbe04c28f819d2fa626d886a2bb1597', 5, 0, 0, 0)
INSERT [dbo].[usuario] ([idusuario], [username], [password], [idrol], [intentos], [intentospass], [bloqueado]) VALUES (7, N'M000002', N'bfbe04c28f819d2fa626d886a2bb1597', 2, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[usuario] OFF


GO
ALTER TABLE [dbo].[categoriainventario] ADD UNIQUE
(
	[nombre] ASC
)
GO


GO
ALTER TABLE [dbo].[colaborador] ADD UNIQUE
(
	[codigo] ASC
)
GO


GO
ALTER TABLE [dbo].[iteminventario] ADD UNIQUE
(
	[descripcion] ASC
)
GO


GO
ALTER TABLE [dbo].[UserProfile] ADD UNIQUE
(
	[UserName] ASC
)
GO


GO
ALTER TABLE [dbo].[usuario] ADD UNIQUE
(
	[username] ASC
)
GO


GO
ALTER TABLE [dbo].[webpages_Roles] ADD UNIQUE
(
	[RoleName] ASC
)
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
ALTER TABLE [dbo].[categoriadetalle] ADD FOREIGN KEY([idatributo])
REFERENCES [dbo].[tipodatoatributo] ([idatributo])
GO
ALTER TABLE [dbo].[categoriadetalle] ADD FOREIGN KEY([idcategoriainventario])
REFERENCES [dbo].[categoriainventario] ([idcategoriainventario])
GO
ALTER TABLE [dbo].[categoriadetalle] ADD FOREIGN KEY([idtipodato])
REFERENCES [dbo].[tipodato] ([idtipodato])
GO
ALTER TABLE [dbo].[categoriadetalle] ADD FOREIGN KEY([idtipodato], [idformato])
REFERENCES [dbo].[tipodatoformato] ([idtipodato], [idformato])
GO
ALTER TABLE [dbo].[categoriainventario] ADD FOREIGN KEY([idcategoriainventariotipo])
REFERENCES [dbo].[categoriainventariotipo] ([idcategoriainventariotipo])
GO
ALTER TABLE [dbo].[centrocosto] ADD FOREIGN KEY([centrocosto_idcentrocosto])
REFERENCES [dbo].[centrocosto] ([idcentrocosto])
GO
ALTER TABLE [dbo].[centrocosto] ADD FOREIGN KEY([idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[colaborador] ADD FOREIGN KEY([colaborador_idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[colaborador] ADD FOREIGN KEY([idcentrocosto])
REFERENCES [dbo].[centrocosto] ([idcentrocosto])
GO
ALTER TABLE [dbo].[colaborador] ADD FOREIGN KEY([idcolaboradortipo])
REFERENCES [dbo].[colaboradortipo] ([idcolaboradortipo])
GO
ALTER TABLE [dbo].[colaborador] ADD FOREIGN KEY([idperfil])
REFERENCES [dbo].[perfil] ([idperfil])
GO
ALTER TABLE [dbo].[colaborador] ADD FOREIGN KEY([idusuario])
REFERENCES [dbo].[usuario] ([idusuario])
GO
ALTER TABLE [dbo].[iteminventario] ADD FOREIGN KEY([idcategoriainventario])
REFERENCES [dbo].[categoriainventario] ([idcategoriainventario])
GO
ALTER TABLE [dbo].[iteminventario] ADD FOREIGN KEY([idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[iteminventariodetalle] ADD FOREIGN KEY([idcategoriadetalle])
REFERENCES [dbo].[categoriadetalle] ([idcategoriadetalle])
GO
ALTER TABLE [dbo].[iteminventariodetalle] ADD FOREIGN KEY([iditeminventario])
REFERENCES [dbo].[iteminventario] ([iditeminventario])
GO
ALTER TABLE [dbo].[opcion] ADD FOREIGN KEY([idsuperior])
REFERENCES [dbo].[opcion] ([idopcion])
GO
ALTER TABLE [dbo].[perfil] ADD FOREIGN KEY([idcentrocosto])
REFERENCES [dbo].[centrocosto] ([idcentrocosto])
GO
ALTER TABLE [dbo].[perfil] ADD FOREIGN KEY([idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[perfilrecurso] ADD FOREIGN KEY([idperfil])
REFERENCES [dbo].[perfil] ([idperfil])
GO
ALTER TABLE [dbo].[perfilrecurso] ADD FOREIGN KEY([idrecurso])
REFERENCES [dbo].[recurso] ([idrecurso])
GO
ALTER TABLE [dbo].[recurso] ADD FOREIGN KEY([idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[rolopcion] ADD FOREIGN KEY([idrol], [idsuperior])
REFERENCES [dbo].[rolopcion] ([idrol], [idopcion])
GO
ALTER TABLE [dbo].[rolopcion] ADD FOREIGN KEY([idopcion])
REFERENCES [dbo].[opcion] ([idopcion])
GO
ALTER TABLE [dbo].[rolopcion] ADD FOREIGN KEY([idrol])
REFERENCES [dbo].[rol] ([idrol])
GO
ALTER TABLE [dbo].[solicitudcolaboradorperfil] ADD FOREIGN KEY([idcolaborador])
REFERENCES [dbo].[colaborador] ([idcolaborador])
GO
ALTER TABLE [dbo].[solicitudcolaboradorperfil] ADD FOREIGN KEY([idperfil])
REFERENCES [dbo].[perfil] ([idperfil])
GO
ALTER TABLE [dbo].[tipodatoatributo] ADD FOREIGN KEY([idtipodato])
REFERENCES [dbo].[tipodato] ([idtipodato])
GO
ALTER TABLE [dbo].[tipodatoatributo] ADD FOREIGN KEY([idtipodato], [idoperador])
REFERENCES [dbo].[tipodatooperador] ([idtipodato], [idoperador])
GO
ALTER TABLE [dbo].[tipodatoatributo] ADD FOREIGN KEY([idtipodato], [idreferencia])
REFERENCES [dbo].[tipodatoreferencia] ([idtipodato], [idreferencia])
GO
ALTER TABLE [dbo].[tipodatoformato] ADD FOREIGN KEY([idtipodato])
REFERENCES [dbo].[tipodato] ([idtipodato])
GO
ALTER TABLE [dbo].[tipodatooperador] ADD FOREIGN KEY([idtipodato])
REFERENCES [dbo].[tipodato] ([idtipodato])
GO
ALTER TABLE [dbo].[tipodatoreferencia] ADD FOREIGN KEY([idtipodato])
REFERENCES [dbo].[tipodato] ([idtipodato])
GO
ALTER TABLE [dbo].[usuario] ADD FOREIGN KEY([idrol])
REFERENCES [dbo].[rol] ([idrol])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
