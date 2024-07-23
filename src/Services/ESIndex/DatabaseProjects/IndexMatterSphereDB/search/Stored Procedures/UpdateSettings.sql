CREATE PROCEDURE search.UpdateSettings(@BatchSize BIGINT, @ProcessOrder BIT, @DocumentDateLimit DATETIME, @PreviousDocumentDateLimit DATETIME, @SummaryFieldEnabled BIT)
AS
SET NOCOUNT ON
UPDATE search.ChangeVersionControl 
SET  [BatchSize] = @BatchSize
    , ProcessOrder = @ProcessOrder
    , DocumentDateLimit = @DocumentDateLimit
    , PreviousDocumentDateLimit = @PreviousDocumentDateLimit
    , SummaryFieldEnabled = @SummaryFieldEnabled