IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbPrecedentMulti_multiMasterID' AND OBJECT_ID('[dbo].[dbPrecedentMulti]') = object_id)
	CREATE NONCLUSTERED INDEX [IX_dbPrecedentMulti_multiMasterID]
    ON [dbo].[dbPrecedentMulti]([multiMasterID] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];
