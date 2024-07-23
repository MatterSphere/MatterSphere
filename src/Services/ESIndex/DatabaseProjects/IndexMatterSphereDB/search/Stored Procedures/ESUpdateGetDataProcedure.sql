CREATE PROCEDURE search.ESUpdateGetDataProcedure(
	@ESIndexTableId SMALLINT
	, @TableFieldName NVARCHAR(128)
	, @ExtTable NVARCHAR(128) 
	, @FieldName NVARCHAR(128)
	, @FieldCodeLookupGroup NVARCHAR(15) = NULL
)
AS
BEGIN

SET NOCOUNT ON

DECLARE @ObjectType NVARCHAR(100)
	, @TableName NVARCHAR(128)
	, @pkFieldName NVARCHAR(128)
	, @ProcName NVARCHAR(128)
	, @ProcCode NVARCHAR(MAX)
	, @ReplString NVARCHAR(MAX)
	, @PosStart BIGINT
	, @FieldAlias NVARCHAR(128)
	, @ExtTableAlias NVARCHAR(40) = N''
	, @ErrMsg NVARCHAR(MAX) = N''
	, @ParentProcName NVARCHAR(128)
	, @ParentProcCode NVARCHAR(MAX)

SELECT @ObjectType = ObjectType
	, @TableName = tablename
	, @pkFieldName = pkFieldName
FROM search.ESIndexTable 
WHERE ESIndexTableId = @ESIndexTableId 

IF ISNULL(@ExtTable, '') <> ''
	SET @ExtTableAlias = QUOTENAME(REPLACE(REPLACE(@ExtTable, '[', ''), ']', ''))

SET @ProcName = N'search.GetMS' + @ObjectType + N'AData' 

SET @ProcCode = (SELECT definition FROM sys.sql_modules  WHERE object_id = OBJECT_ID(@ProcName));   

SET @PosStart  = CHARINDEX(N'/**** CUSTOM COLUMNS END *****/', @ProcCode)

IF @PosStart > 0
BEGIN
	SET @FieldAlias = CASE WHEN ISNULL(@ExtTable, '') <> '' THEN @ExtTableAlias + N'.' ELSE N'base.' END + QUOTENAME(@TableFieldName) 
	SET @ReplString = N'/***ccname=' + QUOTENAME(@FieldName) + N'START***/, ' + 
	CASE WHEN ISNULL(@FieldCodeLookupGroup, '') <> '' 
		THEN N'[dbo].[GetCodeLookupDesc]((SELECT TOP 1 fieldCodeLookupGroup FROM search.ESIndexStructure WHERE FieldName = ''' + @FieldName + N''' AND ESIndexTableId = ' + CAST(@ESIndexTableId AS NVARCHAR(10)) +N') , ' + @FieldAlias + N', @UI)' 
		ELSE @FieldAlias
	END + N' AS ' + QUOTENAME(@FieldName) + N'/***ccname=' + QUOTENAME(@FieldName) + N'END***/
				/**** CUSTOM COLUMNS END *****/'

	SET @ProcCode = REPLACE(@ProcCode, N'/**** CUSTOM COLUMNS END *****/',  @ReplString)
END
ELSE
BEGIN
	SET @ErrMsg = N'There is no tag ''/**** CUSTOM COLUMNS END *****/'' in stored procedure ''%s''' 
	RAISERROR(@ErrMsg , 16, 1, @ProcName)
	RETURN
END

IF ISNULL(@ExtTable, '') <> ''
BEGIN
	SET @PosStart  = CHARINDEX(N'/***exttable=' + @ExtTableAlias + N'START***/', @ProcCode)
	IF @PosStart = 0
	BEGIN
		SET @PosStart  = CHARINDEX(N'/**** CUSTOM EXTDATA END *****/', @ProcCode)
		IF @PosStart > 0
		BEGIN
			SET @ReplString = N'/***exttable=' + @ExtTableAlias + N'START***/LEFT OUTER JOIN ' + QUOTENAME(@ExtTable) + N' AS ' + @ExtTableAlias + N' ON ' + @ExtTableAlias + N'.' + QUOTENAME(@pkFieldName) + N' = base.' + QUOTENAME(@pkFieldName) + N'/***exttable=' + @ExtTableAlias + N'END***/
					/**** CUSTOM EXTDATA END *****/'

			SET @ProcCode = REPLACE(@ProcCode, N'/**** CUSTOM EXTDATA END *****/',  @ReplString)
		END
		ELSE
		BEGIN
			SET @ErrMsg = N'There is no tag ''/**** CUSTOM EXTDATA END *****/'' in stored procedure ''%s''' 
			RAISERROR(@ErrMsg , 16, 1, @ProcName)
			RETURN
		END
	END
END

IF CHARINDEX(N'-id', @FieldName) > 0
BEGIN
	SET @ParentProcName = REPLACE(@FieldName, N'-id',  '')
	SET @ParentProcName = N'search.GetMS' + @ParentProcName + N'AData'

	SET @PosStart  = CHARINDEX(N'/**** CUSTOM PARENT END *****/', @ProcCode)

	IF @PosStart > 0
	BEGIN
		SET @ReplString = N'/**** ' + @TableName + N' ' + @FieldName + N' START *****/
			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.' + QUOTENAME(@TableFieldName) + N'
			FROM @TBatch rb
				INNER JOIN ' + @TableName + N' p ON p.' + @pkFieldName + N' = rb.Id
			WHERE rb.Op = ''I''
				AND p.' + QUOTENAME(@TableFieldName) + N' IS NOT NULL' 
			+ CASE WHEN @ProcName = 'search.GetMSDocumentAData' OR @ProcName = 'search.GetMSEmailAData' THEN N' AND p.docDeleted <> 1 
			' ELSE N'
			' END +
			N'IF @@ROWCOUNT > 0
			BEGIN
				EXEC ' + @ParentProcName + N' @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 
			END
			DELETE @RelD
			/**** ' + @TableName + N' ' + @FieldName + N' END *****/
			/**** CUSTOM PARENT END *****/'

		SET @ProcCode = REPLACE(@ProcCode, N'/**** CUSTOM PARENT END *****/',  @ReplString)
	END
	ELSE
	BEGIN
		SET @ErrMsg = N'There is no tag ''/**** CUSTOM PARENT END *****/'' in stored procedure ''%s''' 
		RAISERROR(@ErrMsg , 16, 1, @ProcName)
		RETURN
	END

	IF ISNULL(@ParentProcName, '') <> ''
	BEGIN
		SET @ParentProcCode = (SELECT definition FROM sys.sql_modules  WHERE object_id = OBJECT_ID(@ParentProcName))

		IF @ParentProcCode IS NULL
		BEGIN
			SET @ErrMsg = N'Can not find stored procedure  ''%s''' 
			RAISERROR(@ErrMsg , 16, 1, @ParentProcName)
			RETURN
		END		

		SET @PosStart  = CHARINDEX(N'/**** CUSTOM CHILD END *****/', @ParentProcCode)
		IF @PosStart > 0
		BEGIN
			SET @ReplString = N'/**** ' + @TableName + N' ' + @FieldName + N' START *****/
					INSERT INTO @RelC (Id, RelPK)
					SELECT c.rowguid
						, rb.Id
					FROM @TBatch rb
						INNER JOIN ' + @TableName + N' c ON c.' + QUOTENAME(@TableFieldName) + N' = rb.Id
					WHERE rb.Op = ''U''

					IF @@ROWCOUNT > 0
						EXEC ' + @ParentProcName + N' @UI = @UI, @Rel = @RelC, @RelType = ''' + @ObjectType + N'''

					DELETE @RelC
					/**** ' + @TableName + N' ' + @FieldName + N' END *****/
					/**** CUSTOM CHILD END *****/'

			SET @ParentProcCode = REPLACE(@ParentProcCode, N'/**** CUSTOM CHILD END *****/', @ReplString)

		END
		ELSE		
		BEGIN
			SET @ErrMsg = N'There is no tag ''/**** CUSTOM CHILD END *****/'' in stored procedure ''%s''' 
			RAISERROR(@ErrMsg , 16, 1, @ParentProcName)
			RETURN
		END		
	END

	SET @ParentProcCode = REPLACE(@ParentProcCode, N'CREATE PROCEDURE', N'ALTER PROCEDURE')

END

BEGIN TRAN

SET @ProcCode = REPLACE(@ProcCode, N'CREATE PROCEDURE', N'ALTER PROCEDURE')

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

COMMIT TRAN

END