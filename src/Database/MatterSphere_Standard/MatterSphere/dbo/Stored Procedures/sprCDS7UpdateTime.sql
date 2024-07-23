

CREATE  PROCEDURE [dbo].[sprCDS7UpdateTime] 
(
	@UFNHEADCODE nvarchar(20),
	@BILLNO nvarchar(20)
)
AS

IF @UFNHEADCODE is not null 
BEGIN	UPDATE 
		T 
	SET 
		T.TimeBilled = 1, 
		T.TimeBillNo = @BILLNO
	FROM 
		dbUFN UFN, dbFile F, dbTimeLedger T, dbFileLegal LEGAL
	WHERE 
		T.fileid = F.fileid and 
		LEGAL.matlaufn = UFN.UFNHEADCODE AND 
		F.fileid = LEGAL.fileid AND
		T.TimeBilled=0 and
		UFN.UFNHeadCode=@UFNHEADCODE
END 
SELECT @@ROWCOUNT, @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS7UpdateTime] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS7UpdateTime] TO [OMSAdminRole]
    AS [dbo];

