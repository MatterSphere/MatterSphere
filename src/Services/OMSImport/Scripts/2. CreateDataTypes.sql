-----------------------------------------------------------------
-- Run on newly created OMSImport Database
-----------------------------------------------------------------

USE [OMSImport]

/****** Object:  UserDefinedDataType [dbo].[uAddressLine] ******/
CREATE TYPE [dbo].[uAddressLine] FROM [nvarchar](64) NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uAddressLineReq] ******/
CREATE TYPE [dbo].[uAddressLineReq] FROM [nvarchar](64) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uCodeLookup] ******/
CREATE TYPE [dbo].[uCodeLookup] FROM [nvarchar](15) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uCountry] ******/
CREATE TYPE [dbo].[uCountry] FROM [int] NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uCreated] ******/
CREATE TYPE [dbo].[uCreated] FROM [datetime] NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uCreatedBy] ******/
CREATE TYPE [dbo].[uCreatedBy] FROM [int] NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uDirectory] ******/
CREATE TYPE [dbo].[uDirectory] FROM [nvarchar](255) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uEmail] ******/
CREATE TYPE [dbo].[uEmail] FROM [nvarchar](200) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uFilePath] ******/
CREATE TYPE [dbo].[uFilePath] FROM [nvarchar](255) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uPostcode] ******/
CREATE TYPE [dbo].[uPostcode] FROM [nvarchar](20) NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uTelephone] ******/
CREATE TYPE [dbo].[uTelephone] FROM [nvarchar](30) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uUICultureInfo] ******/
CREATE TYPE [dbo].[uUICultureInfo] FROM [varchar](10) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uURL] ******/
CREATE TYPE [dbo].[uURL] FROM [nvarchar](255) NOT NULL
GO

/****** Object:  UserDefinedDataType [dbo].[uXML] ******/
CREATE TYPE [dbo].[uXML] FROM [nvarchar](MAX) NULL
GO


