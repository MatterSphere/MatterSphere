CREATE PROCEDURE [dbo].[sprGetLicenseInfo]
	@CurrentUser int = null , 
	@CurrentTerminal nvarchar(50) = null

AS
SET NOCOUNT ON

SELECT 
	U.usrid	 as [ID] , 
	U.usrfullname as [Name] , 
	U.usrregisteredfor as [LicInfo] , 
	dbo.Decr(U.usrregisteredfor) as [LicInfoEx] ,
	U.usrloggedin as [LoggedIn] , 
	U.usrlastlogin as [LastLoggedIn] , 
	U.brid as [Branch] , 
	U.usrinits as [Initials] , 
	U.usractive as [Active] ,
	U.usrtermname as [TerminalName] ,
	U.brid as [Branch] ,
	F.feedepartment as [Department],
	dbo.Decr(U.usrSystemLicense) as [SystemLicenseEx],
	U.usrSystemLicense as [SystemLicense]
FROM 
	dbuser U
LEFT JOIN 
	dbfeeearner F ON F.feeusrid = U.usrworksfor
WHERE
	U.usrid = coalesce ( @CurrentUser , U.usrid ) AND U.AccessType = 'INTERNAL'

SELECT
	T.termname as [ID] , 
	T.termname as [Name] , 
	T.termregisteredfor as [LicInfo] ,
	dbo.Decr(T.termregisteredfor) as [LicInfoEx] , 
	T.termloggedin as [LoggedIn] , 
	T.termlastlogin as [LastLoggedIn] , 
	T.brid as [Branch] ,
	T.termchecksum as [Checksum] ,
	dbo.Decr(T.termchecksum) as [ChecksumEx] , 
	U.usrid as [UserId] ,
	U.usrfullname as [UserName] ,
	T.termVersion as [Version]
FROM
	dbterminal T
LEFT JOIN 
	dbuser U on T.termLastUser = U.usrid
WHERE 
	T.termname = Coalesce(@CurrentTerminal, T.termname)



GO