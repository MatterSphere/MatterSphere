

CREATE PROCEDURE [dbo].[sprCDS11UpdateTime] 
(
	@DATE1 datetime,
	@DATE2 datetime,
	@MATLAUFN nvarchar(20),
	@BILLNO nvarchar(20),
	@LACAT int
)
AS

IF @MATLAUFN is not null 
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
		(T.TimeRecorded Between dateadd(hh,-1,@date1) And @DATE2) AND 
		((UFN.UFNHeadCode)=@MATLAUFN AND 
		((T.TimeBilled)=0)) AND 
		((T.TimeLegalAidCat)=@LACAT)

END 
SELECT @@ROWCOUNT, @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS11UpdateTime] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS11UpdateTime] TO [OMSAdminRole]
    AS [dbo];

