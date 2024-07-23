

CREATE PROCEDURE [dbo].[srepFeeEarnerSnapshot] 
(
	@UI uUICultureInfo = '{default}'
	, @FeeUsrId bigint
	, @StartDate datetime
)	

AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

-- Could have used DateAdd(mm,12,@startdate) for the between clauses but the query executes faster with @enddate
DECLARE	@EndDate Datetime
SET @EndDate = DateAdd(Month,12,@StartDate)

-- Test --
--set @FeeUsrID = 1031992150

-- Note derived bit is here to try and ease some of the burden of Crystal. Crystal is reaponsible for grouping the data
-- and if the info is sorted already it may help!!!!!!!!!!!!!!!
SELECT
	X.*
FROM
(
-- New CLients ( CL.clId )
SELECT
	CL.Created AS DateId,
	1 AS Cat1,
	0 AS Cat2,
	0 AS Cat3,
	0 AS Cat4,
	0 AS Cat5,
	0 AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbClient CL
WHERE
	CL.feeUsrID = @FeeUsrId AND
	CL.Created BETWEEN @StartDate AND @EndDate

-- new files ( F.fileId )
union all
SELECT
	F.Created AS DateId,
	0 as Cat1,
	1 AS Cat2,
	0 AS Cat3,
	0 AS Cat4,
	0 AS Cat5,
	0 AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbFile F
WHERE
	F.filePrincipleID = @FeeUsrId AND
	F.Created BETWEEN @StartDate AND @EndDate

-- !!!  NEW DOCUMENTS ON MY MATTERS  !!! ( DocId )
union all
SELECT
	D.Created AS DateId,
	0 as Cat1,
	0 as Cat2,
	1 AS Cat3,
	0 AS Cat4,
	0 AS Cat5,
	0 AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbDocument D 
INNER JOIN
	dbFile F ON F.fileID = D.fileID
WHERE
	F.filePrincipleID = @FeeUsrId AND
	D.Created BETWEEN @StartDate AND @EndDate

-- NEW DOCUMENTS I CREATED (D.DocID)
union all
SELECT
	D.Created AS DateId,
	0 as Cat1,
	0 as Cat2,
	0 as Cat3,
	1 AS Cat4,
	0 AS Cat5,
	0 AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbDocument D 
WHERE
	D.CreatedBy = @FEEUSRID AND
	D.Created BETWEEN @StartDate AND @EndDate

-- TIME VALUE ON MY MATTERS
union all
SELECT
	T.Created AS DateId,
	0 as Cat1,
	0 as Cat2,
	0 as Cat3,
	0 as Cat4,
	T.Timecharge AS Cat5,
	0 AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbTimeledger T
INNER JOIN
	dbFile F ON F.fileID = T.fileID
WHERE
	F.filePrincipleID = @FeeUsrId AND
	T.Created BETWEEN @StartDate AND @endDate

-- TIME VALUE I CREATED
union all
SELECT
	T.Created AS DateId,
	0 as Cat1,
	0 as Cat2,
	0 as Cat3,
	0 as Cat4,
	0 as Cat5,
	T.Timecharge AS Cat6,
	0 AS Cat7,
	0 AS Cat8
FROM
	dbTimeledger T 
WHERE
	T.feeusrid = @FeeUsrId AND
	T.Created BETWEEN @StartDate AND @EndDate

-- Billing Info, Fees & billed * pro costs
union all
SELECT
	BI.billDate AS DateId,
	0 as Cat1,
	0 as Cat2,
	0 as Cat3,
	0 as Cat4,
	0 as Cat5,
	0 as Cat6,
	BI.billPaidDisb + BI.billNYPDisb AS Cat7,
	BI.billProCosts AS Cat8
FROM
	dbBillInfo BI 
INNER JOIN
	dbFile F ON F.fileID = BI.fileID
WHERE
	F.filePrincipleID = @FeeUsrId AND
	BI.billDate BETWEEN @StartDate AND @EndDate
) X
ORDER BY DatePart(m,X.DateId)

-- This option negates the Parellism & extra cpu the query suffered from.
option (maxdop 1)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFeeEarnerSnapshot] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFeeEarnerSnapshot] TO [OMSAdminRole]
    AS [dbo];

