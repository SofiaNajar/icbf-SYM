USE [master]
GO
/****** Object:  Database [bd_bienestar]    Script Date: 27/06/2024 5:24:15 p. m. ******/
CREATE DATABASE [bd_bienestar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bd_bienestar_Data', FILENAME = N'B:\Programas\SQL server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bd_bienestar.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'bd_bienestar_Log', FILENAME = N'B:\Programas\SQL server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bd_bienestar.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [bd_bienestar] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bd_bienestar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bd_bienestar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bd_bienestar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bd_bienestar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bd_bienestar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bd_bienestar] SET ARITHABORT OFF 
GO
ALTER DATABASE [bd_bienestar] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bd_bienestar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bd_bienestar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bd_bienestar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bd_bienestar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bd_bienestar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bd_bienestar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bd_bienestar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bd_bienestar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bd_bienestar] SET  DISABLE_BROKER 
GO
ALTER DATABASE [bd_bienestar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bd_bienestar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bd_bienestar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bd_bienestar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bd_bienestar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bd_bienestar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bd_bienestar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bd_bienestar] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [bd_bienestar] SET  MULTI_USER 
GO
ALTER DATABASE [bd_bienestar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bd_bienestar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bd_bienestar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bd_bienestar] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [bd_bienestar] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [bd_bienestar] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [bd_bienestar] SET QUERY_STORE = ON
GO
ALTER DATABASE [bd_bienestar] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [bd_bienestar]
GO
/****** Object:  Table [dbo].[Asistencias]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asistencias](
	[idAsistencia] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [date] NOT NULL,
	[estadoNino] [nchar](20) NOT NULL,
	[idNino] [int] NOT NULL,
 CONSTRAINT [PK_Asistencias] PRIMARY KEY CLUSTERED 
(
	[idAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AvancesAcademicos]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvancesAcademicos](
	[idAvanceAcademico] [int] IDENTITY(1,1) NOT NULL,
	[nivel] [nchar](20) NOT NULL,
	[notas] [nchar](10) NOT NULL,
	[descripcion] [nchar](150) NOT NULL,
	[fechaEntrega] [date] NOT NULL,
	[idNino] [int] NOT NULL,
 CONSTRAINT [PK_AvancesAcademicos] PRIMARY KEY CLUSTERED 
(
	[idAvanceAcademico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatosBasicos]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatosBasicos](
	[idDatosBasicos] [int] IDENTITY(1,1) NOT NULL,
	[identificacion] [varchar](10) NOT NULL,
	[nombres] [nchar](100) NOT NULL,
	[fechaNacimiento] [date] NOT NULL,
	[celular] [nchar](10) NOT NULL,
	[direccion] [nchar](80) NOT NULL,
	[idTipoDocumento] [int] NOT NULL,
 CONSTRAINT [PK_DatosBasicos] PRIMARY KEY CLUSTERED 
(
	[idDatosBasicos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EPS]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EPS](
	[idEps] [int] IDENTITY(1,1) NOT NULL,
	[NIT] [nchar](15) NOT NULL,
	[nombre] [nchar](50) NOT NULL,
	[direccion] [nchar](80) NOT NULL,
	[telefono] [nchar](10) NULL,
 CONSTRAINT [PK_EPS] PRIMARY KEY CLUSTERED 
(
	[idEps] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jardines]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jardines](
	[idJardin] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nchar](50) NOT NULL,
	[direccion] [nchar](80) NOT NULL,
	[estado] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Jardin] PRIMARY KEY CLUSTERED 
(
	[idJardin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ninos]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ninos](
	[idNino] [int] IDENTITY(1,1) NOT NULL,
	[tipoSangre] [nchar](10) NOT NULL,
	[ciudadNacimiento] [nchar](50) NOT NULL,
	[peso] [int] NULL,
	[estatura] [float] NULL,
	[idJardin] [int] NOT NULL,
	[idAcudiente] [int] NOT NULL,
	[idMadreComunitaria] [int] NOT NULL,
	[idDatosBasicos] [int] NOT NULL,
	[idEps] [int] NOT NULL,
 CONSTRAINT [PK_Ninos] PRIMARY KEY CLUSTERED 
(
	[idNino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nchar](70) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocumento]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumento](
	[idTipoDoc] [int] IDENTITY(1,1) NOT NULL,
	[tipo] [nchar](10) NOT NULL,
 CONSTRAINT [PK_TipoDocumento] PRIMARY KEY CLUSTERED 
(
	[idTipoDoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 27/06/2024 5:24:15 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[clave] [nchar](50) NULL,
	[nombreUsuario] [nchar](50) NULL,
	[correo] [nchar](50) NULL,
	[idDatosBasicos] [int] NOT NULL,
	[idRol] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Asistencias] ON 

INSERT [dbo].[Asistencias] ([idAsistencia], [fecha], [estadoNino], [idNino]) VALUES (2, CAST(N'2024-06-24' AS Date), N'Sano                ', 2)
SET IDENTITY_INSERT [dbo].[Asistencias] OFF
GO
SET IDENTITY_INSERT [dbo].[AvancesAcademicos] ON 

INSERT [dbo].[AvancesAcademicos] ([idAvanceAcademico], [nivel], [notas], [descripcion], [fechaEntrega], [idNino]) VALUES (3, N'Prenatal: No aplica ', N'S         ', N'                                                                                                                                                      ', CAST(N'2024-06-01' AS Date), 2)
SET IDENTITY_INSERT [dbo].[AvancesAcademicos] OFF
GO
SET IDENTITY_INSERT [dbo].[DatosBasicos] ON 

INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (9, N'4357533', N'acudiente 1                                                                                         ', CAST(N'2024-06-04' AS Date), N'76543     ', N'calle 3434                                                                      ', 1)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (15, N'2672389749', N'Alejandra Garcia Perez                                                                              ', CAST(N'2004-09-29' AS Date), N'1372398923', N'calle sena                                                                      ', 1)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (27, N'1098001002', N'MIGUEL ANGEL                                                                                        ', CAST(N'2024-06-12' AS Date), N'3118842895', N'CL 20 B    97   45                                                              ', 3)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (30, N'2387492313', N'asdasdasd                                                                                           ', CAST(N'2020-09-29' AS Date), N'1238912311', N'3183013ad                             1                                         ', 3)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (31, N'5432456435', N'Madre Primera                                                                                       ', CAST(N'2003-09-28' AS Date), N'2113213123', N'asdasd                                                                          ', 1)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (33, N'5463434534', N'Madre Segunda                                                                                       ', CAST(N'1998-09-19' AS Date), N'2143213123', N'calle 2232323                                                                   ', 2)
INSERT [dbo].[DatosBasicos] ([idDatosBasicos], [identificacion], [nombres], [fechaNacimiento], [celular], [direccion], [idTipoDocumento]) VALUES (34, N'1015997537', N'asdasdasd                                                                                           ', CAST(N'2020-09-30' AS Date), N'1312312321', N'asdcdasd                                                                        ', 3)
SET IDENTITY_INSERT [dbo].[DatosBasicos] OFF
GO
SET IDENTITY_INSERT [dbo].[EPS] ON 

INSERT [dbo].[EPS] ([idEps], [NIT], [nombre], [direccion], [telefono]) VALUES (1, N'433341231-2    ', N'Sanitas                                           ', N'cataratas                                                                       ', N'3123123123')
INSERT [dbo].[EPS] ([idEps], [NIT], [nombre], [direccion], [telefono]) VALUES (2, N'3423424-433    ', N'Compensar                                         ', N'comprensar                                                                      ', N'2342342342')
SET IDENTITY_INSERT [dbo].[EPS] OFF
GO
SET IDENTITY_INSERT [dbo].[Jardines] ON 

INSERT [dbo].[Jardines] ([idJardin], [nombre], [direccion], [estado]) VALUES (1, N'Jardin del Norte                                  ', N'Calle 12345                                                                     ', N'En Trámite')
INSERT [dbo].[Jardines] ([idJardin], [nombre], [direccion], [estado]) VALUES (2, N'Jardin del Sur                                    ', N'calle 01 23                                                                     ', N'Aprobado')
INSERT [dbo].[Jardines] ([idJardin], [nombre], [direccion], [estado]) VALUES (3, N'Jardin de Occidente                               ', N'Calle sin fin                                                                   ', N'En trámite')
INSERT [dbo].[Jardines] ([idJardin], [nombre], [direccion], [estado]) VALUES (5, N'Jardin del Oriente                                ', N'calle con fin                                                                   ', N'Negado')
INSERT [dbo].[Jardines] ([idJardin], [nombre], [direccion], [estado]) VALUES (6, N'asa                                               ', N'calle55                                                                         ', N'En trámite')
SET IDENTITY_INSERT [dbo].[Jardines] OFF
GO
SET IDENTITY_INSERT [dbo].[Ninos] ON 

INSERT [dbo].[Ninos] ([idNino], [tipoSangre], [ciudadNacimiento], [peso], [estatura], [idJardin], [idAcudiente], [idMadreComunitaria], [idDatosBasicos], [idEps]) VALUES (2, N'O+        ', N'sdasdasd                                          ', NULL, NULL, 1, 8, 14, 34, 1)
SET IDENTITY_INSERT [dbo].[Ninos] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([idRol], [nombre]) VALUES (2, N'Administrador                                                         ')
INSERT [dbo].[Roles] ([idRol], [nombre]) VALUES (5, N'Madre Comunitaria                                                     ')
INSERT [dbo].[Roles] ([idRol], [nombre]) VALUES (6, N'Acudiente                                                             ')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[TipoDocumento] ON 

INSERT [dbo].[TipoDocumento] ([idTipoDoc], [tipo]) VALUES (1, N'CC        ')
INSERT [dbo].[TipoDocumento] ([idTipoDoc], [tipo]) VALUES (2, N'CE        ')
INSERT [dbo].[TipoDocumento] ([idTipoDoc], [tipo]) VALUES (3, N'NIUP      ')
SET IDENTITY_INSERT [dbo].[TipoDocumento] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([idUsuario], [clave], [nombreUsuario], [correo], [idDatosBasicos], [idRol]) VALUES (8, NULL, NULL, NULL, 9, 6)
INSERT [dbo].[Usuarios] ([idUsuario], [clave], [nombreUsuario], [correo], [idDatosBasicos], [idRol]) VALUES (9, NULL, NULL, NULL, 15, 6)
INSERT [dbo].[Usuarios] ([idUsuario], [clave], [nombreUsuario], [correo], [idDatosBasicos], [idRol]) VALUES (13, NULL, NULL, NULL, 31, 5)
INSERT [dbo].[Usuarios] ([idUsuario], [clave], [nombreUsuario], [correo], [idDatosBasicos], [idRol]) VALUES (14, NULL, NULL, NULL, 33, 5)
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
/****** Object:  Index [Unique_Key_Table_1]    Script Date: 27/06/2024 5:24:15 p. m. ******/
ALTER TABLE [dbo].[Jardines] ADD  CONSTRAINT [Unique_Key_Table_1] UNIQUE NONCLUSTERED 
(
	[idJardin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Unique_Key_Table_N]    Script Date: 27/06/2024 5:24:15 p. m. ******/
ALTER TABLE [dbo].[Ninos] ADD  CONSTRAINT [Unique_Key_Table_N] UNIQUE NONCLUSTERED 
(
	[idNino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asistencias]  WITH CHECK ADD  CONSTRAINT [FK_Asistencias_Ninos] FOREIGN KEY([idNino])
REFERENCES [dbo].[Ninos] ([idNino])
GO
ALTER TABLE [dbo].[Asistencias] CHECK CONSTRAINT [FK_Asistencias_Ninos]
GO
ALTER TABLE [dbo].[AvancesAcademicos]  WITH CHECK ADD  CONSTRAINT [FK_AvancesAcademicos_Ninos] FOREIGN KEY([idNino])
REFERENCES [dbo].[Ninos] ([idNino])
GO
ALTER TABLE [dbo].[AvancesAcademicos] CHECK CONSTRAINT [FK_AvancesAcademicos_Ninos]
GO
ALTER TABLE [dbo].[DatosBasicos]  WITH CHECK ADD  CONSTRAINT [FK_DatosBasicos_TipoDocumento] FOREIGN KEY([idTipoDocumento])
REFERENCES [dbo].[TipoDocumento] ([idTipoDoc])
GO
ALTER TABLE [dbo].[DatosBasicos] CHECK CONSTRAINT [FK_DatosBasicos_TipoDocumento]
GO
ALTER TABLE [dbo].[Ninos]  WITH CHECK ADD  CONSTRAINT [FK_Ninos_DatosBasicos] FOREIGN KEY([idDatosBasicos])
REFERENCES [dbo].[DatosBasicos] ([idDatosBasicos])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ninos] CHECK CONSTRAINT [FK_Ninos_DatosBasicos]
GO
ALTER TABLE [dbo].[Ninos]  WITH CHECK ADD  CONSTRAINT [FK_Ninos_EPS] FOREIGN KEY([idEps])
REFERENCES [dbo].[EPS] ([idEps])
GO
ALTER TABLE [dbo].[Ninos] CHECK CONSTRAINT [FK_Ninos_EPS]
GO
ALTER TABLE [dbo].[Ninos]  WITH CHECK ADD  CONSTRAINT [FK_Ninos_Jardines] FOREIGN KEY([idJardin])
REFERENCES [dbo].[Jardines] ([idJardin])
GO
ALTER TABLE [dbo].[Ninos] CHECK CONSTRAINT [FK_Ninos_Jardines]
GO
ALTER TABLE [dbo].[Ninos]  WITH CHECK ADD  CONSTRAINT [FK_Ninos_Usuarios] FOREIGN KEY([idAcudiente])
REFERENCES [dbo].[Usuarios] ([idUsuario])
GO
ALTER TABLE [dbo].[Ninos] CHECK CONSTRAINT [FK_Ninos_Usuarios]
GO
ALTER TABLE [dbo].[Ninos]  WITH CHECK ADD  CONSTRAINT [FK_Ninos_Usuarios2] FOREIGN KEY([idAcudiente])
REFERENCES [dbo].[Usuarios] ([idUsuario])
GO
ALTER TABLE [dbo].[Ninos] CHECK CONSTRAINT [FK_Ninos_Usuarios2]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_DatosBasicos] FOREIGN KEY([idDatosBasicos])
REFERENCES [dbo].[DatosBasicos] ([idDatosBasicos])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_DatosBasicos]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY([idRol])
REFERENCES [dbo].[Roles] ([idRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles]
GO
USE [master]
GO
ALTER DATABASE [bd_bienestar] SET  READ_WRITE 
GO
