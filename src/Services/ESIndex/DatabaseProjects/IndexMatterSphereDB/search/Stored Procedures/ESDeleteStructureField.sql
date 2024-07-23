CREATE PROCEDURE search.ESDeleteStructureField (@ESIndexTableId SMALLINT
	, @FieldName NVARCHAR(128))
AS
SET NOCOUNT ON
DECLARE @ErrMsg NVARCHAR(MAX) = N''
	, @ProcName NVARCHAR(128)
	, @ProcCode NVARCHAR(MAX)
	, @ReplString NVARCHAR(MAX)
	, @PosStart BIGINT
	, @PosEnd BIGINT
	, @ParentProcName NVARCHAR(128) 
	, @ParentProcCode NVARCHAR(MAX)
	, @TableName NVARCHAR(128) 
	, @ObjectType NVARCHAR(100)
	, @pkFieldName NVARCHAR(128)

SELECT @ObjectType = ObjectType
	, @TableName = tablename
	, @pkFieldName = pkFieldName
FROM search.ESIndexTable 
WHERE ESIndexTableId = @ESIndexTableId 

SET @ProcName = N'search.GetMS' + @ObjectType + N'AData' 

SET @ProcCode = (SELECT definition FROM sys.sql_modules  WHERE object_id = OBJECT_ID(@ProcName));   

SET @PosStart  = CHARINDEX(N'/***ccname=' + QUOTENAME(@FieldName) + N'START***/, ', @ProcCode)
SET @PosEnd  = CHARINDEX(N'/***ccname=' + QUOTENAME(@FieldName) + N'END***/', @ProcCode)

IF @PosStart = 0
	RETURN


WHILE @PosStart > 0
BEGIN
	SET @ProcCode = STUFF(@ProcCode, @PosStart, @PosEnd - @PosStart + LEN(N'/***ccname=' + QUOTENAME(@FieldName) + N'END***/'), '')
	SET @PosStart  = CHARINDEX(N'/***ccname=' + QUOTENAME(@FieldName) + N'START***/, ', @ProcCode)
	SET @PosEnd  = CHARINDEX(N'/***ccname=' + QUOTENAME(@FieldName) + N'END***/', @ProcCode)
END

IF CHARINDEX(N'-id', @FieldName) > 0
BEGIN
	SET @ParentProcName = REPLACE(@FieldName, N'-id',  '')
	SET @ParentProcName = N'search.GetMS' + @ParentProcName + N'AData'

	SET @PosStart  = CHARINDEX(N'/**** ' + @TableName + N' ' + @FieldName + N' START *****/', @ProcCode)
	SET @PosEnd  = CHARINDEX(N'/**** ' + @TableName + N' ' + @FieldName + N' END *****/', @ProcCode)

	IF @PosStart > 0
		SET @ProcCode = STUFF(@ProcCode, @PosStart, @PosEnd - @PosStart + LEN(N'/**** ' + @TableName + N' ' + @FieldName + N' END *****/'), '')

	IF ISNULL(@ParentProcName, '') <> ''
	BEGIN
		SET @ParentProcCode = (SELECT definition FROM sys.sql_modules  WHERE object_id = OBJECT_ID(@ParentProcName))

	SET @PosStart  = CHARINDEX(N'/**** ' + @TableName + N' ' + @FieldName + N' START *****/', @ParentProcCode)
	SET @PosEnd  = CHARINDEX(N'/**** ' + @TableName + N' ' + @FieldName + N' END *****/', @ParentProcCode)

	IF @PosStart > 0
		SET @ParentProcCode = STUFF(@ParentProcCode, @PosStart, @PosEnd - @PosStart + LEN(N'/**** ' + @TableName + N' ' + @FieldName + N' END *****/'), '')

	END
END

BEGIN TRAN

SET @ProcCode = REPLACE(@ProcCode, N'CREATE PROCEDURE', N'ALTER PROCEDURE')
SET @ParentProcCode = REPLACE(@ParentProcCode, N'CREATE PROCEDURE', N'ALTER PROCEDURE')

EXEC (@ProcCode)
IF @@ERROR > 0
BEGIN
	ROLLBACK 
	RETURN
END

IF ISNULL(@ParentProcCode, '') <> ''
BEGIN
	EXEC (@ParentProcCode)
	IF @@ERROR > 0
	BEGIN
		ROLLBACK 
		RETURN
	END
END

DELETE search.ESIndexStructure 
WHERE ESIndexTableId = @ESIndexTableId
	AND FieldName = @FieldName
IF @@ERROR > 0
BEGIN
	ROLLBACK 
	RETURN
END

COMMIT TRAN
