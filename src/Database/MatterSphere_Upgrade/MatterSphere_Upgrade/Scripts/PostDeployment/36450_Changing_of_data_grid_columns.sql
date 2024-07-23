GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT * from sys.triggers WHERE name = 'tgrUpdateSearchListConfig')
	DROP TRIGGER [dbo].[tgrUpdateSearchListConfig]
GO

CREATE TRIGGER [dbo].[tgrUpdateSearchListConfig] ON [dbo].[dbSearchListConfig]
FOR UPDATE NOT FOR REPLICATION
AS
	SET NOCOUNT ON;
	DELETE dbo.dbUserSearchListColumns WHERE schCode in (SELECT i.schCode FROM inserted i INNER JOIN deleted d ON d.schCode = i.schCode AND ISNULL(d.schListView, '') <> ISNULL(i.schListView, ''))
