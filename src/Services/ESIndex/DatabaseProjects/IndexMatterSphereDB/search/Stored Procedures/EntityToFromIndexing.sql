CREATE PROCEDURE search.EntityToFromIndexing	(@ESIndexTableId SMALLINT, @IndexingEnabled BIT)
AS
SET NOCOUNT ON

DECLARE @tablename SYSNAME = (SELECT tablename FROM search.ESIndexTable WHERE ESIndexTableId = @ESIndexTableId)
	, @sql NVARCHAR(MAX)

UPDATE search.ESIndexTable
SET IndexingEnabled = @IndexingEnabled
WHERE ESIndexTableId = @ESIndexTableId
	AND IndexingEnabled <> @IndexingEnabled

IF @tablename <> ''
BEGIN
	SET @sql = N'IF ' + CASE @IndexingEnabled WHEN 1 THEN N'NOT' ELSE N'' END + N' EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'''+ @tablename + N''')) ALTER TABLE ' + @tablename +  + CASE @IndexingEnabled WHEN 1 THEN N' ENABLE' ELSE N' DISABLE' END + N' CHANGE_TRACKING'
	PRINT @sql
	EXEC sp_executesql @sql
END