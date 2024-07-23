

CREATE PROCEDURE [dbo].[schSearchEscalatedTasks] 
(
	@UI uUICultureInfo = '{default}',  
	@FEEUSRID bigint,
	@DATERANGE uCodeLookup = null,
	@RESPONSIBLETO int,
	@DIFF uCodeLookup = null
)

AS

IF @DATERANGE = 'ODWITHIN7'
	BEGIN
		SELECT 
			F.fileID,
			'Task' AS Lvl,
			5 AS Image,
			T.tskDue AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			T.tskDesc AS Description,	
			COALESCE(CL1.cdDesc, '~' + NULLIF(T.tskType, '''') + '~') AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '''') + '~') AS fileType,
			TU.usrFullName AS AllocateTo,
			FU.usrFullName AS fileFeeEarner,
			NULL AS MSNextStage,
			DATEDIFF(day, T.tskDue, getutcdate()) AS DaysOverdue
		FROM         		
			dbo.dbClient CL
		INNER JOIN
			dbo.dbFile F ON CL.clID = F.clID 
		INNER JOIN 
			dbo.dbTasks T ON F.fileID = T.fileID 
		INNER JOIN
			dbo.dbUser as TU ON T.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbUser as FU ON F.fileprincipleID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'TASKTYPE', @UI ) CL1 ON CL1.[cdCode] = T.tskType
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			T.tskDue > getutcdate() AND T.tskDue < getutcdate() + 7 AND
			'Task' = COALESCE(@DIFF, 'Task')
		UNION SELECT
			F.fileID,
			'Milestone' AS Lvl,
			24 AS Image,
			MSD.MSNextDueDate AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			dbo.GetNextStageDesc(F.fileID) AS Description,
			NULL AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '''') + '~') AS fileType,
			NULL AS AllocatedTo,
			FU.usrFullName AS fileFeeEarner,
			MSD.MSNextDueStage AS MSNextStage,
			DATEDIFF(day, MSD.MSNextDueDate, getutcdate()) AS DaysOverdue
		FROM
			dbClient CL
		INNER JOIN
			dbFile F ON F.clID = CL.clID
		INNER JOIN
			dbMSData_OMS2K MSD ON MSD.fileID = F.fileID
		INNER JOIN
			dbMSConfig_OMS2K MSC ON MSC.MSCode = MSD.MSCode
		INNER JOIN
			dbUser FU ON FU.usrID = F.filePrincipleID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			MSD.MSNextDueDate > getutcdate() AND MSD.MSNextDueDate < getutcdate() + 7 AND
			'Milestone' = COALESCE(@DIFF, 'Milestone')
		ORDER BY
			Date DESC
	END
ELSE
IF @DATERANGE = 'ODOVER7'
	BEGIN
		SELECT 
			F.fileID,
			'Task' AS Lvl,
			5 AS Image,
			T.tskDue AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			T.tskDesc AS Description,	
			COALESCE(CL1.cdDesc, '~' + NULLIF(T.tskType, '''') + '~') AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '''') + '~') AS fileType,
			TU.usrFullName AS AllocateTo,
			FU.usrFullName AS fileFeeEarner,
			NULL AS MSNextStage,
			DATEDIFF(day, T.tskDue, getutcdate()) AS DaysOverdue
		FROM         		
			dbo.dbClient CL
		INNER JOIN
			dbo.dbFile F ON CL.clID = F.clID 
		INNER JOIN 
			dbo.dbTasks T ON F.fileID = T.fileID 
		INNER JOIN
			dbo.dbUser as TU ON T.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbUser as FU ON F.fileprincipleID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'TASKTYPE', @UI ) CL1 ON CL1.[cdCode] = T.tskType
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			T.tskDue < getutcdate() + 7 AND
			'Task' = COALESCE(@DIFF, 'Task')
		UNION SELECT
			F.fileID,
			'Milestone' AS Lvl,
			24 AS Image,
			MSD.MSNextDueDate AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			dbo.GetNextStageDesc(F.fileID) AS Description,
			NULL AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '''') + '~') AS fileType,
			NULL AS AllocatedTo,
			FU.usrFullName AS fileFeeEarner,
			MSD.MSNextDueStage AS MSNextStage,
			DATEDIFF(day, MSD.MSNextDueDate, getutcdate()) AS DaysOverdue
		FROM
			dbClient CL
		INNER JOIN
			dbFile F ON F.clID = CL.clID
		INNER JOIN
			dbMSData_OMS2K MSD ON MSD.fileID = F.fileID
		INNER JOIN
			dbMSConfig_OMS2K MSC ON MSC.MSCode = MSD.MSCode
		INNER JOIN
			dbUser FU ON FU.usrID = F.filePrincipleID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			MSD.MSNextDueDate < getutcdate() + 7 AND
			'Milestone' = COALESCE(@DIFF, 'Milestone')
		ORDER BY
			Date DESC
	END
ELSE
	BEGIN
		SELECT 
			F.fileID,
			'Task' AS Lvl,
			5 AS Image,
			T.tskDue AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			T.tskDesc AS Description,	
			COALESCE(CL1.cdDesc, '~' + NULLIF(T.tskType, '') + '~') AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '') + '~') AS fileType,
			TU.usrFullName AS AllocateTo,
			FU.usrFullName AS fileFeeEarner,
			NULL AS MSNextStage,
			DATEDIFF(day, T.tskDue, getutcdate()) AS DaysOverdue
		FROM         		
			dbo.dbClient CL
		INNER JOIN
			dbo.dbFile F ON CL.clID = F.clID 
		INNER JOIN 
			dbo.dbTasks T ON F.fileID = T.fileID 
		INNER JOIN
			dbo.dbUser as TU ON T.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = TU.usrID
		INNER JOIN
			dbo.dbUser as FU ON F.fileprincipleID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'TASKTYPE', @UI ) CL1 ON CL1.[cdCode] = T.tskType
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			T.tskDue < getutcdate() AND
			'Task' = COALESCE(@DIFF, 'Task')
		UNION SELECT
			F.fileID,
			'Milestone' AS Lvl,
			24 AS Image,
			MSD.MSNextDueDate AS Date,
			dbo.GetFileRef(CL.clno, F.fileNo) AS Ref,
			CL.clName AS clientName,
			F.fileDesc AS fileDescription,
			dbo.GetNextStageDesc(F.fileID) AS Description,
			NULL AS Type,
			COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '') + '~') AS fileType,
			NULL AS AllocatedTo,
			FU.usrFullName AS fileFeeEarner,
			MSD.MSNextDueStage AS MSNextStage,
			DATEDIFF(day, MSD.MSNextDueDate, getutcdate()) AS DaysOverdue
		FROM
			dbClient CL
		INNER JOIN
			dbFile F ON F.clID = CL.clID
		INNER JOIN
			dbMSData_OMS2K MSD ON MSD.fileID = F.fileID
		INNER JOIN
			dbMSConfig_OMS2K MSC ON MSC.MSCode = MSD.MSCode
		INNER JOIN
			dbUser FU ON FU.usrID = F.filePrincipleID
		INNER JOIN
			dbo.dbFeeEarner RT ON RT.feeusrID = FU.usrID
		LEFT JOIN 
			dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] = F.fileType
		WHERE
			F.filePrincipleID = COALESCE(@FEEUSRID, F.filePrincipleID) AND
			RT.feeResponsibleTo = COALESCE(@RESPONSIBLETO, RT.feeResponsibleTo) AND
			MSD.MSNextDueDate < getutcdate() AND
			'Milestone' = COALESCE(@DIFF, 'Milestone')
		ORDER BY
			Date DESC
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEscalatedTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchEscalatedTasks] TO [OMSAdminRole]
    AS [dbo];

