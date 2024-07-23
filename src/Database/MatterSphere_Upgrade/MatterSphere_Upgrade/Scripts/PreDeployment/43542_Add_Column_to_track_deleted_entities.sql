EXEC dbo.fwbsAddColumn @Tablename = 'dbClientDelete',       @ColumnName = 'DeletedGuid', @columnDesc = 'UNIQUEIDENTIFIER NULL'
EXEC dbo.fwbsAddColumn @Tablename = 'dbContactMergeDelete', @ColumnName = 'DeletedGuid', @columnDesc = 'UNIQUEIDENTIFIER NULL'
EXEC dbo.fwbsAddColumn @Tablename = 'dbMatterMergeDelete',  @ColumnName = 'DeletedGuid', @columnDesc = 'UNIQUEIDENTIFIER NULL'
