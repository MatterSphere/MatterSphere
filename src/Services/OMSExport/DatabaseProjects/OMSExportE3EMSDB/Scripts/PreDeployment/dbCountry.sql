-- Adding new column for ISO Alpha-2 Codes
IF COL_LENGTH('dbo.dbCountry', 'ctryISOCode') IS NULL
    ALTER TABLE dbo.dbCountry
        ADD ctryISOCode  NCHAR(2)  NULL
GO