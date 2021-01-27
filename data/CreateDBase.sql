USE [master]
GO
/****** Object:  Database [Projects]    Script Date: 27.01.2021 18:56:45 ******/
CREATE DATABASE [Projects]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Projects', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Projects.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Projects_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Projects_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Projects] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Projects].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Projects] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Projects] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Projects] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Projects] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Projects] SET ARITHABORT OFF 
GO
ALTER DATABASE [Projects] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Projects] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Projects] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Projects] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Projects] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Projects] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Projects] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Projects] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Projects] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Projects] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Projects] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Projects] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Projects] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Projects] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Projects] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Projects] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Projects] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Projects] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Projects] SET  MULTI_USER 
GO
ALTER DATABASE [Projects] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Projects] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Projects] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Projects] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Projects] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Projects] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Projects', N'ON'
GO
ALTER DATABASE [Projects] SET QUERY_STORE = OFF
GO
USE [Projects]
GO
/****** Object:  Table [dbo].[Lists]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Id_Project] [int] NOT NULL,
	[Note] [nvarchar](1250) NULL,
	[Id_Owner] [int] NULL,
 CONSTRAINT [PK_Lists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Id_Owner] [int] NOT NULL,
	[Start Date] [date] NULL,
	[Due Date] [date] NULL,
	[Description] [nvarchar](1250) NULL,
	[Priority] [nvarchar](250) NULL,
	[Progress] [int] NULL,
	[Tags] [nvarchar](250) NULL,
	[Id_List] [int] NOT NULL,
	[Status] [nvarchar](250) NULL,
	[Done Date] [date] NULL,
 CONSTRAINT [PK_Tasks_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](1250) NULL,
	[Id_Company] [int] NOT NULL,
	[Id_Owner] [int] NOT NULL,
	[Id_Category] [int] NOT NULL,
	[Tags] [nvarchar](250) NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Website] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Industry] [nvarchar](250) NULL,
	[Fax] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[Post code] [nvarchar](250) NULL,
	[Country] [nvarchar](250) NULL,
	[Notes] [nvarchar](1250) NULL,
	[Id_Owner] [int] NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](1250) NOT NULL,
	[Id_Person] [int] NOT NULL,
	[Create Date] [date] NOT NULL,
	[Edit Date] [date] NOT NULL,
	[Id_Task] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewObject]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewObject]
AS

SELECT C.Id_Person AS Id_Owner,A.Id as Id_Company, P.id as Id_Project, L.Id AS Id_List, T.Id AS Id_Task, C.Id  AS Id_Comment FROM dbo.Comments AS C 
JOIN dbo.Tasks as T on c.Id_Task=t.Id 
JOIN dbo.Lists as L on T.Id_List=L.Id
JOIN dbo.Projects as P on L.Id_Project=P.Id
JOIN dbo.Companies as A on P.Id_Company=A.id
UNION
SELECT T.Id_Owner AS Id_Owner,A.Id as Id_Company, P.id as Id_Project, L.Id AS Id_List, T.Id AS Id_Task, NULL  AS Id_Comment FROM dbo.Tasks as T 
JOIN dbo.Lists as L on T.Id_List=L.Id
JOIN dbo.Projects as P on L.Id_Project=P.Id
JOIN dbo.Companies as A on P.Id_Company=A.id
UNION
SELECT L.Id_Owner AS Id_Owner,A.Id as Id_Company, P.id as Id_Project, L.Id AS Id_List, NULL AS Id_Task, NULL  AS Id_Comment FROM dbo.Lists as L
JOIN dbo.Projects as P on L.Id_Project=P.Id
JOIN dbo.Companies as A on P.Id_Company=A.id
UNION
SELECT P.Id_Owner AS Id_Owner,A.Id as Id_Company, P.id as Id_Project, NULL AS Id_List, NULL AS Id_Task, NULL  AS Id_Comment FROM dbo.Projects as P
JOIN dbo.Companies as A on P.Id_Company=A.id
UNION
SELECT A.Id_Owner AS Id_Owner,A.Id as Id_Company, NULL as Id_Project, NULL AS Id_List, NULL AS Id_Task, NULL  AS Id_Comment FROM dbo.Companies as A

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](1250) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Executers]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Executers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_Task] [int] NOT NULL,
	[Id_Person] [int] NOT NULL,
	[Id_Role] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persons]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[First Name] [nvarchar](250) NULL,
	[Last Name] [nvarchar](250) NOT NULL,
	[Id_Company] [int] NOT NULL,
	[Type] [int] NULL,
	[Job Title] [nvarchar](250) NULL,
	[Office Phone] [nvarchar](250) NULL,
	[Cell Phone] [nvarchar](250) NULL,
	[Home Phone] [nvarchar](250) NULL,
	[Fax] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[Postal Code] [nvarchar](250) NULL,
	[Country] [nvarchar](250) NULL,
	[Notes] [nvarchar](1250) NULL,
	[Pasword] [nvarchar](50) NULL,
	[IsAdmin] [int] NULL,
 CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipients]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_Person] [int] NOT NULL,
	[Id_Comment] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Id_Owner] [int] NULL,
 CONSTRAINT [PK_Recipients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 27.01.2021 18:56:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](1250) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Persons] FOREIGN KEY([Id_Person])
REFERENCES [dbo].[Persons] ([id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Persons]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Tasks] FOREIGN KEY([Id_Task])
REFERENCES [dbo].[Tasks] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Tasks]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Persons] FOREIGN KEY([Id_Owner])
REFERENCES [dbo].[Persons] ([id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Persons]
GO
ALTER TABLE [dbo].[Lists]  WITH CHECK ADD  CONSTRAINT [FK_Lists_Persons] FOREIGN KEY([Id_Owner])
REFERENCES [dbo].[Persons] ([id])
GO
ALTER TABLE [dbo].[Lists] CHECK CONSTRAINT [FK_Lists_Persons]
GO
ALTER TABLE [dbo].[Lists]  WITH CHECK ADD  CONSTRAINT [FK_Lists_Projects] FOREIGN KEY([Id_Project])
REFERENCES [dbo].[Projects] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lists] CHECK CONSTRAINT [FK_Lists_Projects]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_Companies] FOREIGN KEY([Id_Company])
REFERENCES [dbo].[Companies] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_Companies]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Categories] FOREIGN KEY([Id_Category])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Categories]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Companies] FOREIGN KEY([Id_Company])
REFERENCES [dbo].[Companies] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Companies]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Persons] FOREIGN KEY([Id_Owner])
REFERENCES [dbo].[Persons] ([id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Persons]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Lists] FOREIGN KEY([Id_List])
REFERENCES [dbo].[Lists] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Lists]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Persons] FOREIGN KEY([Id_Owner])
REFERENCES [dbo].[Persons] ([id])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Persons]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewObject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewObject'
GO
USE [master]
GO
ALTER DATABASE [Projects] SET  READ_WRITE 
GO
