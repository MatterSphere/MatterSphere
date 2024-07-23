-----------------------------------------------------------------
-- Run on MatterCentre Database
-----------------------------------------------------------------

USE [MatterCentre]

IF NOT EXISTS ( SELECT sc.[name] FROM syscolumns sc join sysobjects so on sc.[id] = so.[id] WHERE sc.[name] = 'addCountryOld')
BEGIN
	ALTER TABLE [dbo].[dbAddress]
	ADD addCountryOld nvarchar (50) NULL
END
GO