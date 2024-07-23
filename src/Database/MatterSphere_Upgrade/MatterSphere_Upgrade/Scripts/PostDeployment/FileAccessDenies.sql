IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[FileAccessDenies]'))
	DROP VIEW [config].[FileAccessDenies]
GO

CREATE VIEW [config].[FileAccessDenies]
AS
	SELECT     c.FileID AS ID
	FROM         
		config.dbFile AS C JOIN
		config.FileAccess() AS CA ON c.FileID = CA.FileID



GO
