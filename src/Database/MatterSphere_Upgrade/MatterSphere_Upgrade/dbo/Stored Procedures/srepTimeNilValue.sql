CREATE PROCEDURE [dbo].[srepTimeNilValue]
(
	@UI uUICultureInfo = '{default}'
)

AS 

SELECT     
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), ', ') as fileDesc, 
	COALESCE(CL1.cdDesc, '~' + NULLIF(F.fileFundCode, '') + '~') AS fileFundCode,
	TL.timeCharge, 
	F.filePrincipleID, 
	U.usrInits
FROM    
	dbo.dbClient CL 
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbTimeLedger TL ON F.fileID = TL.fileID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN 
	dbo.GetCodeLookupDescription('FUNDTYPE', @UI) CL1 ON CL1.cdCode = F.fileFundCode
WHERE
	TL.timeCharge = 0 AND 
	F.fileFundCode <> 'NOCHG'
