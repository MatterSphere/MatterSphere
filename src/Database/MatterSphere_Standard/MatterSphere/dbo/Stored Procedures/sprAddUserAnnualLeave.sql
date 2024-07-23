

CREATE PROCEDURE dbo.sprAddUserAnnualLeave 
	@startDate smalldatetime ,
	@endDate smalldatetime ,
	@usrID int
	
AS
SET NOCOUNT ON

SET @startDate = Convert( smalldatetime , Convert(nchar(10) , @startDate , 112 ))
SET @endDate = Convert( smalldatetime , Convert(nchar(10) , @endDate , 112 ))

IF @startDate > @endDate
BEGIN
DECLARE @swap smalldatetime
SET @swap = @startDate
SET @startDate = @endDate
SET @endDate = @swap
END


WHILE (@startDate<=@endDate)
BEGIN
	IF (SELECT COUNT(*) FROM dbUserAnnualLeave WHERE usrID=@usrID AND alDate=@startDate) = 0
		INSERT INTO dbUserAnnualLeave (usrID, alDate) Values(@usrID, @startDate) 
	SET @startDate = @startDate + 1
END

--Return value for datalist to validate
SELECT 0,0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddUserAnnualLeave] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddUserAnnualLeave] TO [OMSAdminRole]
    AS [dbo];

