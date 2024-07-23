Print 'NewInstallScripts\Default_Users.sql'


DECLARE @user nvarchar(200)
SET @user = suser_sname()

SET IDENTITY_INSERT dbo.dbUser ON
IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -1 )
BEGIN
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive, usrWelcomewizard, usrXML)
	VALUES	(-1, 'ADMIN', 'ADMINISTRATOR', CASE WHEN @user like '%\%' THEN @user ELSE 'ADMINISTRATOR'END, 'ADMINISTRATOR', 'Administrator', 'ADMIN', NULL, -1, 1, NULL, NULL, 1, 0, '<config><settings><property name="Roles" value="ADMIN"/></settings></config>')
END
GO

IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -2 )
BEGIN
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive)
	VALUES	(-2, 'GUEST', 'GUEST', 'GUEST', 'GUEST', 'Guest', 'STANDARD', NULL, -1, 1, NULL, NULL, 1)
END
GO

IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -3 )
BEGIN
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive, 	usrXML)
	VALUES (-3, 'PARTNERSUPPORT', 'PARTNERSUPPORT', 'PARTNERSUPPORT', 'PARTNERSUPPORT', 'Partner Support', 'ADMIN', NULL, -1, 1, NULL, NULL, 1, '<config><settings><property name="Roles" value="PARTNER;ADMIN"/></settings></config>')
END
GO

IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -4 )
BEGIN
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrCCType, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive, usrXML)
	VALUES (-4, 'FWBSSUPPORT', 'FWBSSUPPORT', 'FWBSSUPPORT', 'FWBSSUPPORT', 'FWBS Support Account', 'ADMIN', 'SUPPORT' , 'support@fwbs.net', -1, 1, '+44(0)0844 414 2999', '+44(0)1327 322912', 1, '<config><settings><property name="Roles" value="195.70.86.2;PARTNER;ADMIN"/></settings></config>')
END
GO
	
IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -100 )
BEGIN	
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive, usrXML)
	VALUES	(-100, 'OMSJOBPROCESSOR', 'OMSJOBPROCESSOR', 'OMSJOBPROCESSOR', 'OMSJOBPROCESSOR', 'OMS Job Processor Service', 'STANDARD', 'omsjobprocessor@fwbs.net', -1, 1, NULL, NULL, 0, '<config><settings><property name="Roles" value="ADMIN"/></settings></config>')
END
GO	

IF NOT EXISTS ( SELECT usrID FROM dbo.dbUser WHERE usrID = -101 )
BEGIN
	INSERT dbo.dbUser (usrid, usrinits, usralias, usradid, usrsqlid, usrFullName, usrtype, usrEmail, usrWorksFor, brID, usrDDI, usrDDIFax, usrActive, usrXML)
	VALUES (-101, 'OMSCOREIMPORT', 'OMSCOREIMPORT', 'OMSCOREIMPORT', 'OMSCOREIMPORT', 'OMS Core Import Engine Service', 'STANDARD', 'omscoreimport@fwbs.net', -1, 1, NULL, NULL, 0, '<config><settings><property name="Roles" value="ADMIN"/></settings></config>')
END
GO

SET IDENTITY_INSERT dbo.dbUser OFF	
GO


-- Add feeEarner user
IF NOT EXISTS ( SELECT feeUsrID FROM dbo.dbFeeEarner WHERE feeUsrID = -1)
BEGIN
	INSERT dbo.dbFeeEarner (feeusrid, feetype, feeResponsible, feeActive)
	VALUES (-1, 'STANDARD', 1, 1)
END
GO


