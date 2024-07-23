

CREATE VIEW[dbo].[vwDBMSConfig]
AS
SELECT MSCode, MSDescription, MSDept, MSAll,
	MSStage1Desc, MSStage2Desc, MSStage3Desc, MSStage5Desc, MSStage6Desc, MSStage4Desc, MSStage7Desc, MSStage8Desc, MSStage9Desc, MSStage10Desc,
	MSStage11Desc, MSStage12Desc, MSStage13Desc, MSStage14Desc, MSStage15Desc, MSStage16Desc, MSStage17Desc, MSStage18Desc, MSStage19Desc, MSStage20Desc,
	MSStage21Desc, MSStage22Desc, MSStage23Desc, MSStage24Desc, MSStage25Desc, MSStage26Desc, MSStage27Desc, MSStage28Desc, MSStage29Desc, MSStage30Desc,
	MSStage1Days, MSStage2Days, MSStage3Days, MSStage4Days, MSStage5Days, MSStage6Days, MSStage7Days, MSStage8Days, MSStage9Days, MSStage10Days,
	MSStage11Days, MSStage12Days, MSStage13Days, MSStage14Days, MSStage15Days, MSStage16Days, MSStage17Days, MSStage18Days, MSStage19Days, MSStage20Days,
	MSStage21Days, MSStage22Days, MSStage23Days, MSStage24Days, MSStage25Days, MSStage26Days, MSStage27Days, MSStage28Days, MSStage29Days, MSStage30Days,
	MSStage1LongDesc, MSStage2LongDesc, MSStage3LongDesc, MSStage4LongDesc, MSStage5LongDesc, MSStage7LongDesc, MSStage8LongDesc, MSStage6LongDesc, MSStage9LongDesc, MSStage10LongDesc,
	MSStage11LongDesc, MSStage12LongDesc, MSStage13LongDesc, MSStage14LongDesc, MSStage15LongDesc, MSStage16LongDesc, MSStage17LongDesc, MSStage18LongDesc, MSStage19LongDesc, MSStage20LongDesc,
	MSStage21LongDesc, MSStage22LongDesc, MSStage23LongDesc, MSStage24LongDesc, MSStage25LongDesc, MSStage26LongDesc, MSStage27LongDesc, MSStage28LongDesc, MSStage29LongDesc, MSStage30LongDesc,
	MSStage1Calcfrom, MSStage2Calcfrom, MSStage3Calcfrom, MSStage4Calcfrom, MSStage5Calcfrom, MSStage6Calcfrom, MSStage7Calcfrom, MSStage8Calcfrom, MSStage9Calcfrom, MSStage10Calcfrom,
	MSStage11Calcfrom, MSStage12Calcfrom, MSStage13Calcfrom, MSStage14Calcfrom, MSStage15Calcfrom, MSStage16Calcfrom, MSStage17Calcfrom, MSStage18Calcfrom, MSStage19Calcfrom, MSStage20Calcfrom,
	MSStage21Calcfrom, MSStage22Calcfrom, MSStage23Calcfrom, MSStage24Calcfrom, MSStage25Calcfrom, MSStage26Calcfrom, MSStage27Calcfrom, MSStage28Calcfrom, MSStage29Calcfrom, MSStage30Calcfrom,
	MSStage1ReallyLongDesc, MSStage2ReallyLongDesc, MSStage3ReallyLongDesc, MSStage4ReallyLongDesc, MSStage5ReallyLongDesc,
	MSStage6ReallyLongDesc, MSStage7ReallyLongDesc, MSStage8ReallyLongDesc, MSStage9ReallyLongDesc, MSStage10ReallyLongDesc,
	MSStage11ReallyLongDesc, MSStage12ReallyLongDesc, MSStage13ReallyLongDesc, MSStage14ReallyLongDesc, MSStage15ReallyLongDesc,
	MSStage16ReallyLongDesc, MSStage17ReallyLongDesc, MSStage18ReallyLongDesc, MSStage19ReallyLongDesc, MSStage20ReallyLongDesc,
	MSStage21ReallyLongDesc, MSStage22ReallyLongDesc, MSStage23ReallyLongDesc, MSStage24ReallyLongDesc, MSStage25ReallyLongDesc,
	MSStage26ReallyLongDesc, MSStage27ReallyLongDesc, MSStage28ReallyLongDesc, MSStage29ReallyLongDesc, MSStage30ReallyLongDesc,
	MSStage1Action, MSStage2Action, MSStage3Action, MSStage4Action, MSStage5Action, MSStage6Action, MSStage7Action, MSStage8Action, MSStage9Action, MSStage10Action,
	MSStage11Action, MSStage12Action, MSStage13Action, MSStage14Action, MSStage15Action,  MSStage16Action, MSStage17Action, MSStage18Action, MSStage19Action, MSStage20Action,
	MSStage21Action, MSStage22Action, MSStage23Action, MSStage24Action, MSStage25Action,  MSStage26Action, MSStage27Action, MSStage28Action, MSStage29Action, MSStage30Action
FROM dbo.dbMSConfig_OMS2K

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMSConfig] TO [OMSApplicationRole]
    AS [dbo];

