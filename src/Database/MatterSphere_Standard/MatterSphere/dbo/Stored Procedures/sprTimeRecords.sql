

CREATE PROCEDURE [dbo].[sprTimeRecords](@FILEID bigint,
@UI uUICultureInfo = '{default}')
AS SELECT     dbo.dbTimeLedger.*, COALESCE(CL1.cdDesc, '~' + NULLIF(timeActivityCode, '') + '~') AS TimeActivityCodeDesc, dbo.dbUser.usrInits
FROM         dbo.dbUser INNER JOIN
                      dbo.dbFeeEarner ON dbo.dbUser.usrID = dbo.dbFeeEarner.feeusrID INNER JOIN
                      dbo.dbTimeLedger ON dbo.dbFeeEarner.feeusrID = dbo.dbTimeLedger.feeusrID
			LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  timeActivityCode
WHERE     (fileID = @fileid)
ORDER BY dbtimeledger.Created DESC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeRecords] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeRecords] TO [OMSAdminRole]
    AS [dbo];

