

CREATE PROCEDURE [dbo].[sprCDS11Append] 
	(
	 @MATLAUFN nvarchar(15), --'140504/001',
	 @PROCOSTS money,--314.5,
	 @DISBS money,--0,
	 @TRAVEL money,--0, 
	 @WAITING money,--0,
	 @CONCLUDED datetime,--#02/Jun/2004#,
	 @CREATEDBY bigint ,--'GM',
	 @CLCODE ucodelookup,--'A1',
	 @OFFCODE ucodelookup,--'A1',
	 @OUTCODE ucodelookup,--'A1',
	 @SUSPECTS int,--1,
	 @ATTENDANCES int,--11,
	 @BILLNO nvarchar(20),--'INV26A',
	 @IDENTIFIER nvarchar(15),--'PP',
	 @DUTY bit,--True,
	 @YOUTH bit,--False, 
	 @REFERENCE nvarchar(50),--'Ester, T : F10001/16',
	 @BRANCH int,--'1',
	 @SUPPLEMENTARY bit,--False
	 @SEX ucodelookup, --M, F or Null
	 @DISABILITYFLAG ucodelookup, --Y, N or U
	 @ETHNICORIGIN ucodelookup --Code provided from codelookup
	)

AS
SET NOCOUNT ON

IF @MATLAUFN is not null
BEGIN
	 DECLARE @CREATEDATE datetime
	 SET @CREATEDATE = GETDATE()
	 INSERT INTO DBCDSClaim 
		  (
		   UFNCode, 
		   UFNProCosts, 
		   UFNDisb, 
		   UFNTravel, 
		   UFNWaiting, 
		   UFNCDS11Created,
		   UFNConcluded, 
		   UFNCDS11CreatedBy, 
		   UFNCLCode, 
		   UFNOffCode, 
		   UFNOutCode, 
		   UFNSuspects, 
		   UFNAttendances, 
		   UFNID, 
		   UFNIdentifier, 
		   UFNDuty, 
		   UFNYouth, 
		   UFNCDSRef, 
		   UFNBRID, 
		   UFNCDSSUPPLEMENTARY,
		   UFNSEX,
		   UFNDISABILITYFLAG,
		   UFNETHNICORIGIN
			) 
	 Values 
			(
		   @MATLAUFN, --'140504/001',
		   @PROCOSTS,--314.5,
		   @DISBS,--0,
		   @TRAVEL,--0, 
		   @WAITING,--0,
		   @CREATEDATE,--#02/Jun/2004#,
		   @CONCLUDED,--#02/Jun/2004#,
		   @CREATEDBY,--'GM',
		   @CLCODE,--'A1',
		   @OFFCODE,--'A1',
		   @OUTCODE,--'A1',
		   @SUSPECTS,--1,
		   @ATTENDANCES,--11,
		   @BILLNO,--'INV26A',
		   @IDENTIFIER,--'PP',
		   @DUTY,--True,
		   @YOUTH,--False, 
		   @REFERENCE,--'Ester, T : F10001/16',
		   @BRANCH,--'1',
		   @SUPPLEMENTARY,--False
		   @SEX,
		   @DISABILITYFLAG,
		   @ETHNICORIGIN
			 )
END 
SELECT 
	UFNCode, 
	UFNCDS11Created 
FROM
	dbCDSClaim 
WHERE 
	UFNCode = @MATLAUFN and 
	UFNCDS11Created = @CREATEDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS11Append] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDS11Append] TO [OMSAdminRole]
    AS [dbo];

