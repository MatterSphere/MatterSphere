

CREATE VIEW[dbo].[vwDBMSData]
AS
SELECT fileID AS MatterID, MSCode, MSNextDueDate, MSNextDueStage,
	MSStage1Due, MSStage2Due, MSStage3Due, MSStage4Due, MSStage5Due, MSStage6Due, MSStage7Due, MSStage8Due, MSStage9Due, MSStage10Due,
	MSStage11Due, MSStage12Due, MSStage13Due, MSStage14Due, MSStage15Due, MSStage16Due, MSStage17Due, MSStage18Due, MSStage19Due, MSStage20Due,
	MSStage21Due, MSStage22Due, MSStage23Due, MSStage24Due, MSStage25Due, MSStage26Due, MSStage27Due, MSStage28Due, MSStage29Due, MSStage30Due,
	MSStage1Achieved, MSStage2Achieved, MSStage3Achieved, MSStage4Achieved, MSStage5Achieved, MSStage6Achieved, MSStage7Achieved, MSStage8Achieved, MSStage9Achieved, MSStage10Achieved,
	MSStage11Achieved, MSStage12Achieved, MSStage13Achieved, MSStage14Achieved, MSStage15Achieved, MSStage16Achieved, MSStage17Achieved, MSStage18Achieved, MSStage19Achieved, MSStage20Achieved,
	MSStage21Achieved, MSStage22Achieved, MSStage23Achieved, MSStage24Achieved, MSStage25Achieved, MSStage26Achieved, MSStage27Achieved, MSStage28Achieved, MSStage29Achieved, MSStage30Achieved,
	dbo.GetUser(MSStage1AchievedBy, NULL) AS MSStage1AchievedBy, dbo.GetUser(MSStage2AchievedBy, NULL) AS MSStage2AchievedBy, dbo.GetUser(MSStage3AchievedBy, NULL) AS MSStage3AchievedBy,
	dbo.GetUser(MSStage4AchievedBy, NULL) AS MSStage4AchievedBy, dbo.GetUser(MSStage5AchievedBy, NULL) AS MSStage5AchievedBy, dbo.GetUser(MSStage6AchievedBy, NULL) AS MSStage6AchievedBy,
	dbo.GetUser(MSStage7AchievedBy, NULL) AS MSStage7AchievedBy, dbo.GetUser(MSStage8AchievedBy, NULL) AS MSStage8AchievedBy, dbo.GetUser(MSStage9AchievedBy, NULL) AS MSStage9AchievedBy,
	dbo.GetUser(MSStage10AchievedBy, NULL) AS MSStage10AchievedBy, dbo.GetUser(MSStage11AchievedBy, NULL) AS MSStage11AchievedBy, dbo.GetUser(MSStage12AchievedBy, NULL) AS MSStage12AchievedBy,
	dbo.GetUser(MSStage13AchievedBy, NULL) AS MSStage13AchievedBy, dbo.GetUser(MSStage14AchievedBy, NULL) AS MSStage14AchievedBy, dbo.GetUser(MSStage15AchievedBy, NULL) AS MSStage15AchievedBy,
	dbo.GetUser(MSStage16AchievedBy, NULL) AS MSStage16AchievedBy, dbo.GetUser(MSStage17AchievedBy, NULL) AS MSStage17AchievedBy, dbo.GetUser(MSStage18AchievedBy, NULL) AS MSStage18AchievedBy,
	dbo.GetUser(MSStage19AchievedBy, NULL) AS MSStage19AchievedBy, dbo.GetUser(MSStage20AchievedBy, NULL) AS MSStage20AchievedBy, dbo.GetUser(MSStage21AchievedBy, NULL) AS MSStage21AchievedBy,
	dbo.GetUser(MSStage22AchievedBy, NULL) AS MSStage22AchievedBy, dbo.GetUser(MSStage23AchievedBy, NULL) AS MSStage23AchievedBy, dbo.GetUser(MSStage24AchievedBy, NULL) AS MSStage24AchievedBy,
	dbo.GetUser(MSStage25AchievedBy, NULL) AS MSStage25AchievedBy, dbo.GetUser(MSStage26AchievedBy, NULL) AS MSStage26AchievedBy, dbo.GetUser(MSStage27AchievedBy, NULL) AS MSStage27AchievedBy,
	dbo.GetUser(MSStage28AchievedBy, NULL) AS MSStage28AchievedBy, dbo.GetUser(MSStage29AchievedBy, NULL) AS MSStage29AchievedBy, dbo.GetUser(MSStage30AchievedBy, NULL) AS MSStage30AchievedBy,
	MSActive, MSCreated
FROM dbo.dbMSData_OMS2K

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMSData] TO [OMSApplicationRole]
    AS [dbo];

