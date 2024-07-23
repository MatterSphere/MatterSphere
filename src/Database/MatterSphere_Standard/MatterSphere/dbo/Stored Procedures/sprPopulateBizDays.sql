
CREATE PROCEDURE [dbo].[sprPopulateBizDays] 
	@startDate smalldatetime ,
	@endDate smalldatetime ,
	@culture uUIcultureInfo
	
AS
SET NOCOUNT ON

SET @startDate = Convert( smalldatetime , Convert(nchar(10) , @startDate , 112 ))
SET @endDate = Convert( smalldatetime , Convert(nchar(10) , @endDate , 112 ))


DECLARE @dt SMALLDATETIME; 
SET @dt = @startDate; 
WHILE @dt <= @endDate 
BEGIN 
    INSERT dbo.dbBizDay(bizDate , bizCultureInfo) SELECT @dt , @culture 
    WHERE NOT EXISTS (SELECT * FROM dbo.dbBizDay WHERE bizDate=@dt and bizCultureInfo=@culture)
    SET @dt = @dt + 1; 
END 

-- now reset weekends back to 0 
UPDATE dbo.dbBizDay SET bizIsWorkDay = 0 WHERE bizIsWeekday = 0 
--Return Value for Datalist to validate
SELECT 0,0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPopulateBizDays] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprPopulateBizDays] TO [OMSAdminRole]
    AS [dbo];

