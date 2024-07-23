CREATE PROCEDURE [search].[UpdateSummaryTemplate]
      @ESIndexTableId SMALLINT
    , @summaryTemplate NVARCHAR(128)
AS

SET NOCOUNT ON

UPDATE  search.ESIndexTable
    SET [summaryTemplate] = @summaryTemplate
    WHERE ESIndexTableId = @ESIndexTableId
