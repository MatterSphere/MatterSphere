

CREATE FUNCTION [dbo].[GetNextBusinessDate] ( @fromDate smalldatetime , @noOfBizdays int , @usrID int , @UI uUICultureInfo) 
RETURNS smalldatetime
AS
BEGIN
      DECLARE @nextBizDate smalldatetime
      SET @fromDate = Convert( smalldatetime , Convert(nchar(10) , @fromDate , 112 ))
      IF @noOfBizDays = 0
            SET @nextBizDate = @fromDate
      ELSE
      BEGIN
      IF @noOfBizDays > 0
      BEGIN 
            SET @nextBizDate = 
            ( SELECT X.BizDate FROM 
                  ( SELECT BD.bizDate ,  Row_Number() Over ( Order by BizDate) as RowNumber FROM dbo.dbBizDay BD LEFT JOIN (SELECT * FROM dbo.dbUserAnnualLeave WHERE usrID = @usrID ) AL ON AL.alDate = BD.bizDate 
                  WHERE BD.bizDate >= @fromDate AND BD.bizIsWorkDay = 1 AND BD.bizCultureInfo = @UI AND AL.alDate IS NULL ) X 
            WHERE X.RowNumber = @noOfBizDays + 1 )
      END
      ELSE
      BEGIN
            SET @nextBizDate = 
            ( SELECT X.BizDate FROM 
                  ( SELECT BD.bizDate ,  Row_Number() Over ( Order by BizDate Desc ) as RowNumber FROM dbo.dbBizDay BD LEFT JOIN (SELECT * FROM dbo.dbUserAnnualLeave WHERE usrID = @usrID ) AL ON AL.alDate = BD.bizDate 
                  WHERE BD.bizDate <= @fromDate AND BD.bizIsWorkDay = 1 AND BD.bizCultureInfo = @UI AND AL.alDate IS NULL ) X 
            WHERE X.RowNumber = ( @noOfBizDays * -1) + 1 )
      END
      END
      RETURN @nextBizDate 
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextBusinessDate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextBusinessDate] TO [OMSAdminRole]
    AS [dbo];

