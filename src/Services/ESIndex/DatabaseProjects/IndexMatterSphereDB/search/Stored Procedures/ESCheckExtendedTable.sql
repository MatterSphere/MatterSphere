CREATE PROCEDURE search.ESCheckExtendedTable(
	@TableName NVARCHAR(128),
	@pkFieldName NVARCHAR(128)
)
AS
SET NOCOUNT ON

DECLARE @ObjID INT = OBJECT_ID(@TableName)
	, @Ret INT = 1


IF @ObjID IS NULL
	RETURN @Ret

SET @Ret = ISNULL((
			SELECT MAX(CASE c.name WHEN @pkFieldName THEN 0 ELSE 1 END)
			FROM sys.indexes i 
				INNER JOIN sys.index_columns ic ON i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id
				INNER JOIN sys.columns c ON c.column_id = ic.column_id AND c.object_id = ic.object_id
			WHERE i.is_primary_key = 1
				AND i.object_id = @ObjID
			), 1)

RETURN @Ret
