CREATE TABLE [dbo].[PII_Table_Join]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FR_Table_Name] INT NOT NULL,
	[FR_Join_column] Nvarchar(250) NOT NULL,
    [TO_Table_Name] NVARCHAR(250) NOT NULL,
	[TO_Join_column] Nvarchar(250) NOT NULL, 
    [ActivityToController] BIT NOT NULL DEFAULT 0, 
    [Audit_Removal_link] NVARCHAR(250) NULL)
