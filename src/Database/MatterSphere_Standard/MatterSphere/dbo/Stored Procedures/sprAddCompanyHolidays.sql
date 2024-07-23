

CREATE PROCEDURE [dbo].[sprAddCompanyHolidays] 
	@startDate smalldatetime ,
	@endDate smalldatetime ,
	@description nvarchar(50) = null,
	@UI uUIcultureInfo
	
AS
SET NOCOUNT ON

SET @startDate = Convert( smalldatetime , Convert(nchar(10) , @startDate , 112 ))
SET @endDate = Convert( smalldatetime , Convert(nchar(10) , @endDate , 112 ))

UPDATE dbo.dbBizDay SET bizIsWorkDay = 0, bizHolidayDescription = @description WHERE bizDate Between @startDate AND @endDate and bizCultureInfo = @UI
--Return value so Datalist can validate
Select 0,0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddCompanyHolidays] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddCompanyHolidays] TO [OMSAdminRole]
    AS [dbo];

