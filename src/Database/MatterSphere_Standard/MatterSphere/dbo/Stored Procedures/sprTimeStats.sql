

CREATE PROCEDURE [dbo].[sprTimeStats] (@FILEID bigint = 0, @CLID bigint = 0,  @UI uUICultureInfo = '{default}')  
AS

IF @CLID =  0 
BEGIN
	--- Stats for Breakdown if CLID =  0  then default assumed so search for FileID as 0


	SELECT     clid, fileID, timeStatus, TIMEBILLED
	, SUM(timeMins) AS TotMins, COUNT(ID) AS NumTrans, SUM(timeCost) AS TotCost, SUM(timeCharge) AS TotCharge, 
	                      SUM(timeUnits) AS TotUnits
	FROM         dbo.dbTimeLedger
	WHERE fileid = @fileid
	GROUP BY clid, fileID, timeStatus , TIMEBILLED
END
ELSE
BEGIN

	declare @CREDLIMTOTAL money 
	set @CREDLIMTOTAL = 0
	
	SELECT @credlimtotal =  SUM(filecreditlimit) FROM dbfile where dbo.dbfile.clid = @clid


	SELECT     clid, fileID, timeStatus, TIMEBILLED,@credlimtotal as CreditLimitTotal
	, SUM(timeMins) AS TotMins, COUNT(ID) AS NumTrans, SUM(timeCost) AS TotCost, SUM(timeCharge) AS TotCharge, 
	                      SUM(timeUnits) AS TotUnits
	FROM         dbo.dbTimeLedger
	WHERE clid = @clid
	GROUP BY clid, fileID, timeStatus , TIMEBILLED
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeStats] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeStats] TO [OMSAdminRole]
    AS [dbo];

