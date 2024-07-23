DECLARE @ESIndexTableId SMALLINT
	, @TableFieldName NVARCHAR(128)
	, @ExtTable NVARCHAR(128) 
	, @FieldName NVARCHAR(128)
	, @FieldCodeLookupGroup NVARCHAR(15) = NULL

DECLARE custom_fields CURSOR FOR
SELECT ESIndexTableId
	, FieldName
	, ExtTable
	, FieldName
	, fieldCodeLookupGroup
FROM search.ESIndexStructure
WHERE IsDefault=0

OPEN custom_fields;

FETCH NEXT FROM custom_fields
INTO @ESIndexTableId
	, @TableFieldName
	, @ExtTable
	, @FieldName
	, @FieldCodeLookupGroup;

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC search.ESUpdateGetDataProcedure @ESIndexTableId, @TableFieldName, @ExtTable, @FieldName, @FieldCodeLookupGroup;
	FETCH NEXT FROM custom_fields
	INTO @ESIndexTableId
		, @TableFieldName
		, @ExtTable
		, @FieldName
		, @FieldCodeLookupGroup;
END

CLOSE custom_fields;
DEALLOCATE custom_fields;
GO
