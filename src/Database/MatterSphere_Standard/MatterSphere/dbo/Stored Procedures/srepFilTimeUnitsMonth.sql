

CREATE PROCEDURE [dbo].[srepFilTimeUnitsMonth]
(
	@UI uUICultureInfo='{default}',
	@FEEEARNER int = null,
	---@CHARGEABLE nvarchar(50) = null,
	@MONTH tinyint,
	@YEAR int
)

AS 

SELECT day(timeRecorded) as [day] , sum(timeunits) as [count] , feeusrid
INTO #rptTimeSheetMonth 
FROM dbTimeLedger T
INNER JOIN dbFile F on F.fileid = T.fileID
WHERE COALESCE(feeusrID, '') = COALESCE(@FEEEARNER, feeusrID, '')  and (month(timeRecorded) = @MONTH and year(timeRecorded) = @YEAR)  and F.filestatus like 'LIVE%'
GROUP BY day(timeRecorded) , feeusrid
ORDER BY [day]

select
F.feeusrid,
U.usrFullName,
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 1 and T.feeusrid = F.feeusrid), 0) as [day1],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 2  and T.feeusrid = F.feeusrid), 0) as [day2],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 3 and T.feeusrid = F.feeusrid), 0) as [day3],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 4 and T.feeusrid = F.feeusrid), 0) as [day4],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 5 and T.feeusrid = F.feeusrid), 0) as [day5],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 6 and T.feeusrid = F.feeusrid), 0) as [day6],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 7 and T.feeusrid = F.feeusrid), 0) as [day7],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 8 and T.feeusrid = F.feeusrid), 0) as [day8],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 9 and T.feeusrid = F.feeusrid), 0) as [day9],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 10 and T.feeusrid = F.feeusrid), 0) as [day10],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 11 and T.feeusrid = F.feeusrid), 0) as [day11],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 12 and T.feeusrid = F.feeusrid), 0) as [day12],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 13 and T.feeusrid = F.feeusrid), 0) as [day13],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 14 and T.feeusrid = F.feeusrid), 0) as [day14],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 15 and T.feeusrid = F.feeusrid), 0) as [day15],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 16 and T.feeusrid = F.feeusrid), 0) as [day16],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 17 and T.feeusrid = F.feeusrid), 0) as [day17],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 18 and T.feeusrid = F.feeusrid), 0) as [day18],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 19 and T.feeusrid = F.feeusrid), 0) as [day19],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 20 and T.feeusrid = F.feeusrid), 0) as [day20],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 21 and T.feeusrid = F.feeusrid), 0) as [day21],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 22 and T.feeusrid = F.feeusrid), 0) as [day22],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 23 and T.feeusrid = F.feeusrid), 0) as [day23],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 24 and T.feeusrid = F.feeusrid), 0) as [day24],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 25 and T.feeusrid = F.feeusrid), 0) as [day25],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 26 and T.feeusrid = F.feeusrid), 0) as [day26],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 27 and T.feeusrid = F.feeusrid), 0) as [day27],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 28 and T.feeusrid = F.feeusrid), 0) as [day28],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 29 and T.feeusrid = F.feeusrid), 0) as [day29],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 30 and T.feeusrid = F.feeusrid), 0) as [day30],
coalesce((select [count] from  #rptTimeSheetMonth T where [day] = 31 and T.feeusrid = F.feeusrid), 0) as [day31],
coalesce((select sum([count]) from  #rptTimeSheetMonth T where T.feeusrid = F.feeusrid), 0) as [dayTotal]
from dbfeeearner F
inner join (select feeusrid from #rptTimeSheetMonth group by feeusrid) F2 on F2.feeusrid = F.feeusrid
inner join dbuser U
on U.usrid = F.feeusrid
order by usrFullName

/*
--- Set the Select Statement
--- set @select = N'
SELECT
	U.usrFullName,
	CASE WHEN (SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 1 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) IS NULL THEN 0 ELSE (SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 1 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) END AS day1,
	CASE WHEN (SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 2 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) IS NULL THEN 0 ELSE (SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 2 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) END AS day2,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 3 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day3,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 4 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day4,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 5 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day5,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 6 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day6,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 7 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day7,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 8 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day8,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 9 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day9,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 10 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day10,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 11 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day11,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 12 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day12,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 13 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day13,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 14 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day14,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 15 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day15,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 16 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day16,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 17 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day17,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 18 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day18,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 19 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day19,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 20 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day20,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 21 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day21,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 22 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day22,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 23 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day23,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 24 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day24,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 25 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day25,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 26 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day26,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 27 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day27,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 28 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day28,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 29 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day29,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 30 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day30,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND DAY(TL1.timeRecorded) = 31 AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS day31,
	(SELECT SUM(TL1.timeUnits) FROM dbTimeLedger TL1 WHERE TL1.feeusrID = TL.feeusrID AND MONTH(TL1.timeRecorded) = COALESCE(@MONTH, MONTH(TL1.timeRecorded)) AND YEAR(TL1.timeRecorded) = COALESCE(@YEAR, YEAR(TL1.timeRecorded))) AS dayTotal
INTO
	#rptTimeUnitsMonth
FROM
	dbTimeLedger TL
INNER JOIN
	dbUser U ON U.usrID = TL.feeusrID
INNER JOIN
	dbFile F ON F.fileID = TL.fileID
WHERE
	COALESCE(TL.feeusrID, '') = COALESCE(@FEEEARNER, TL.feeusrID, '') AND
	F.fileStatus LIKE 'LIVE%'
GROUP BY
	TL.feeusrID,
	U.usrFullName

SELECT
	*
FROM
	#rptTimeUnitsMonth
WHERE
	dayTotal > 0


*/

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTimeUnitsMonth] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilTimeUnitsMonth] TO [OMSAdminRole]
    AS [dbo];

