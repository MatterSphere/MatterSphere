

CREATE PROCEDURE [dbo].[srepLaukCmrf]
	@MATLAContract uCodeLookup ,
	@month tinyint ,
	@year smallint

AS
SET NOCOUNT ON 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @regSalesTaxRate decimal (7,3)
SET @regSalesTaxRate = ( SELECT ( 1 + ( regSalesTaxRate ) / 100 ) FROM dbo.dbRegInfo )

SELECT  f.fileclosed,
	CL.clNo + '/' + F.fileNo + '/' + U.usrInits as OurRef ,
	CL.clname ,	
	IND.contSex ,
	IND.contEthnicOrigin ,
	NULL as contDisabilityMonitoring ,
	IND.contDOB ,
	IND.AgeYears ,
	CASE
		WHEN IND.AgeYears <= 16 THEN 'A' 
		WHEN IND.AgeYears <= 24 THEN 'B' 
		WHEN IND.AgeYears <= 34 THEN 'C' 
		WHEN IND.AgeYears <= 49 THEN 'D' 
		WHEN IND.AgeYears <= 64 THEN 'E' 
		WHEN IND.AgeYears >= 65 THEN 'F' 
	END as AgeCode ,
	CASE WHEN ADR.addPostCode IS NOT NULL THEN ADR.addPostCode ELSE ADRCO.addPostCode END as addPostCode ,
	F.Created ,
	FL.MatLAContract ,
	FL.MatLAMatType ,
	FL.MatLAPartI ,
	FL.MatLAPartII ,	
	CASE WHEN T.Mins IS NOT NULL THEN T.Mins ELSE 0 END as TimeMins ,
	@regSalesTaxRate * T.Charge as TimeCharge ,
	@regSalesTaxRate * F.filecreditlimit as MaxClaimable ,
	CASE WHEN T.Charge > F.fileCreditLimit THEN @regSalesTaxRate * F.fileCreditLimit WHEN T.Charge < F.fileCreditLimit THEN @regSalesTaxRate * T.Charge ELSE 0 END as Claim ,
	Substring(MatEndPoint,1,1) as EP1,
	Substring(MatEndPoint,2,1) as EP2,
	Substring(MatEndPoint,3,1) as EP3,
	LAC.LAContractRef
FROM 
	dbo.dbfile F
JOIN
	dbo.dbclient CL on CL.clid = f.clid
JOIN
	dbo.dbContact CO on CL.ClDefaultContact = CO.ContID
LEFT JOIN
	dbo.dbFileLegal FL on F.fileid = FL.fileid
LEFT JOIN
	( SELECT contID , contSex , contEthnicOrigin , contDOB , Convert(int,Datediff(d , contDOB , GetDate()) /365.25) as AgeYears FROM dbo.dbContactIndividual ) IND on CL.cldefaultcontact = IND.contid
LEFT JOIN
	dbo.dbaddress ADR on ADR.addid = CL.cldefaultaddress
LEFT JOIN
	dbo.dbaddress ADRCO on ADRCO.addid = CO.ContDefaultAddress
JOIN
	dbo.dbLegalAidContract LAC on LAC.LAContractCode = FL.MatLAContract
LEFT JOIN
	( SELECT Sum(timeCharge) as Charge , Sum(timeMins) as Mins , FileID FROM dbo.dbTimeLedger GROUP BY FileID ) T ON T.fileID = F.fileID
LEFT JOIN
	dbo.dbUser U ON U.usrID = F.filePrincipleID
WHERE 
	F.filestatus = 'DEAD' 
AND 
	FL.MatLAContract = @MATLAContract 
AND 
	Datepart ( m , F.fileclosed ) = @Month 
AND
	Datepart ( yyyy , F.fileclosed ) = @Year 
AND
	MatEndpoint IS NOT NULL

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLaukCmrf] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLaukCmrf] TO [OMSAdminRole]
    AS [dbo];

