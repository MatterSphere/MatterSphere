

CREATE FUNCTION dbo.GetNoOfBusinessDays ( @fromDate smalldatetime , @toDate smalldatetime , @usrID int , @UI uUIcultureInfo) 
RETURNS int
AS
BEGIN
      DECLARE @bizDays int
      SET @fromDate = Convert( smalldatetime , Convert(nchar(10) , @fromDate , 112 ))
      SET @toDate = Convert( smalldatetime , Convert(nchar(10) , @toDate , 112 ))
      IF ( @fromDate < @toDate )
      BEGIN
            SET @bizDays = ( SELECT COUNT(*) FROM dbo.dbBizDay BD LEFT JOIN (SELECT * FROM dbo.dbUserAnnualLeave WHERE usrID = @usrID ) AL ON AL.alDate = BD.bizDate 
            WHERE BD.bizDate >= @fromDate AND BD.bizDate <= @toDate AND BD.bizIsWorkDay = 1 AND BD.bizCultureInfo = @UI AND AL.alDate IS NULL )
      END
      ELSE
      BEGIN
            SET @bizDays = ( SELECT COUNT(*) FROM dbo.dbBizDay BD LEFT JOIN (SELECT * FROM dbo.dbUserAnnualLeave WHERE usrID = @usrID ) AL ON AL.alDate = BD.bizDate 
            WHERE BD.bizDate <= @fromDate AND BD.bizDate >= @toDate AND BD.bizIsWorkDay = 1 AND BD.bizCultureInfo = @UI AND AL.alDate IS NULL )
      END
   RETURN @bizDays -1 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoOfBusinessDays] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoOfBusinessDays] TO [OMSAdminRole]
    AS [dbo];

