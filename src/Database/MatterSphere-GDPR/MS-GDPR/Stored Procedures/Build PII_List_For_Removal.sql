CREATE PROCEDURE [dbo].[Build_PII_List_For_Removal] AS
DECLARE @SQL nvarchar(max)
DECLARE @T1 int
DECLARE @TJ1 nvarchar(250)



DECLARE  Control_Cursor CURSOR FOR 
select distinct pt.ID,ptj.[TO_Join_column] from [dbo].[PII_Tables] pt JOIN [dbo].PII_Table_Join ptj ON pt.ID = ptj.[FR_Table_Name] and pt.PII_TableName = ptj.[TO_Table_Name] WHERE [PII_ISController] = 1
OPEN Control_Cursor;
/******************* FIRST A CURSOR To POPULATE FROM FOR MAIN TABLES CAUSES AND STRUCTUES FOR LATTER IN PROCEDURE **************/
FETCH NEXT FROM Control_Cursor into @T1,@TJ1;
WHILE @@FETCH_STATUS = 0
BEGIN
	select distinct @SQL='TRUNCATE TABLE config.[PII_CHANGE_'+[PII_TableName] +']; INSERT INTO config.[PII_CHANGE_'+[PII_TableName] +'] select '+QUOTENAME(@TJ1)+' FROM '+[PII_TABLENAME]+' WHERE '+PII_RULE from [dbo].[PII_Tables] WHERE [ID] = @T1

	print (@SQL)
	EXEC(@SQL)
	FETCH NEXT FROM Control_Cursor into @T1,@TJ1;
END;

CLOSE Control_Cursor;
DEALLOCATE Control_Cursor;

RETURN 0
