

CREATE PROCEDURE [dbo].[sprFeeEarnerRecord]
(@feeusrID BigInt)
AS
	--Fee Earner Information
	select * from dbfeeearner where feeusrID = @feeusrID;
	
	--Todays Time Recording Units
	SELECT feeusrID, SUM(timeUnits) AS TotalUnitsToday
	FROM dbo.dbTimeLedger
	WHERE (timeRecorded BETWEEN dbo.GetStartDate(getutcdate()) AND dbo.GetEndDate(getutcdate()))
	GROUP BY feeusrID
	HAVING (feeusrID = @feeusrID)

	--This Weeks Time Recording Units
	SELECT feeusrID, SUM(timeUnits) AS TotalUnitsThisWeek
	FROM dbo.dbTimeLedger
	WHERE (DatePart(week,timeRecorded) = DatePart(week,getutcdate()))
	GROUP BY feeusrID
	HAVING (feeusrID = @feeusrID)
	
	--This Months Time Recording Units
	SELECT feeusrID, SUM(timeUnits) AS TotalUnitsThisMonth
	FROM dbo.dbTimeLedger
	WHERE (DatePart(month,timeRecorded) = DatePart(month,getutcdate()))
	GROUP BY feeusrID
	HAVING (feeusrID = @feeusrID)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFeeEarnerRecord] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprFeeEarnerRecord] TO [OMSAdminRole]
    AS [dbo];

