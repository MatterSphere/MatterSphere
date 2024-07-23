CREATE PROCEDURE [search].[ESUpdateIndexStructure] (
      @ESIndexTableId SMALLINT
	, @FieldName NVARCHAR(128)
	, @ESFieldType NVARCHAR(50)
	, @searchable BIT
	, @facetable BIT
	, @FacetOrder TINYINT = NULL
	, @Analyzer NVARCHAR(50)
	, @Suggestable BIT
	, @cdCode NVARCHAR(15) = NULL
	, @FieldCodeLookupGroup NVARCHAR(15) = NULL
)
AS

SET NOCOUNT ON

UPDATE  search.ESIndexStructure
    SET 
       [ESFieldType] = @ESFieldType
      ,[searchable] = @searchable
      ,[facetable] = @facetable
      ,[FacetOrder] = @FacetOrder
      ,[Analyzer] = @Analyzer
      ,[Suggestable] = @Suggestable
      ,[cdCode] = @cdCode
      ,[fieldCodeLookupGroup] = @FieldCodeLookupGroup
    WHERE ESIndexTableId = @ESIndexTableId 
    AND FieldName = @FieldName
