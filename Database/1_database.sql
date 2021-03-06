USE [master]
GO
/****** Object:  Database [StealthyCommerceDB]    Script Date: 4/15/2019 8:39:55 AM ******/
CREATE DATABASE [StealthyCommerceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StealthyCommerceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\StealthyCommerceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StealthyCommerceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\StealthyCommerceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [StealthyCommerceDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StealthyCommerceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StealthyCommerceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StealthyCommerceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StealthyCommerceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StealthyCommerceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StealthyCommerceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [StealthyCommerceDB] SET  MULTI_USER 
GO
ALTER DATABASE [StealthyCommerceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StealthyCommerceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StealthyCommerceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StealthyCommerceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StealthyCommerceDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'StealthyCommerceDB', N'ON'
GO
ALTER DATABASE [StealthyCommerceDB] SET QUERY_STORE = OFF
GO
USE [StealthyCommerceDB]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 4/15/2019 8:39:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offer]    Script Date: 4/15/2019 8:39:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offer](
	[OfferId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Price] [decimal](6, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[NumberOfTerms] [int] NULL,
	[DateModified] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED 
(
	[OfferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 4/15/2019 8:39:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OfferId] [int] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CancelDate] [datetime] NULL,
	[AmountRefunded] [money] NULL,
	[AmountCharged] [money] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 4/15/2019 8:39:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateModified] [datetime] NULL,
	[Brand] [nvarchar](50) NULL,
	[Term] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Offer] ADD  CONSTRAINT [DF_Offer_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Offer] ADD  CONSTRAINT [DF_Offer_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_EndDate]  DEFAULT (getdate()) FOR [EndDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_PurchaseDate]  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Offer]  WITH CHECK ADD  CONSTRAINT [FK_Offer_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Offer] CHECK CONSTRAINT [FK_Offer_Product]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Offer] FOREIGN KEY([OfferId])
REFERENCES [dbo].[Offer] ([OfferId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Offer]
GO
USE [master]
GO
ALTER DATABASE [StealthyCommerceDB] SET  READ_WRITE 
GO
