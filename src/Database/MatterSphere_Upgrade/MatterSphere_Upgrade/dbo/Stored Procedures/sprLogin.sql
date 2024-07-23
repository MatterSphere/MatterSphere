

CREATE PROCEDURE [dbo].[sprLogin](@logintype nvarchar(15) = 'NT', @userName nvarchar(100) = NULL, @termName nvarchar(100) = NULL)

AS
SET NOCOUNT ON

-- Login Type
-- *******
-- OMS - Finds a user based on the oms initals or mapped alias
-- NT - Finds a user based on the current logged in windows id 
-- AAD - Finds a user based on the current logged in windows UPN
-- SQL - Finds a user based on its mapped sql name,
-- ADID - Finds a user based on its mapped windows id

DECLARE @id int , @printid int , @feeusrid int

IF @logintype = 'OMS'
	SELECT @id = usrid FROM dbo.dbuser WHERE usrInits = @userName OR usrAlias = @userName
ELSE IF @logintype = 'SQL'
	SELECT @id = usrid FROM dbo.dbuser WHERE usrSQLID = @userName
ELSE IF @logintype = 'NT' OR @logintype = 'AAD'
	SELECT @id = usrid FROM dbo.dbuser WHERE usrADID = suser_sname()
ELSE IF @logintype = 'ADID'
	SELECT @id = usrid FROM dbo.dbuser WHERE usrADID = @userName
ELSE 
	SELECT @id = usrid FROM dbo.dbuser WHERE usrInits = @userName OR usrAlias = @userName

 

SELECT @printid = usrprintid , @feeusrid = usrworksfor FROM dbo.dbUser WHERE usrID = @id

SELECT * FROM dbo.dbUser WHERE usrid = @id

SELECT * FROM dbo.dbTerminal WHERE termName = coalesce(@termname, host_name())

SELECT * FROM dbo.dbprinter WHERE printID = @printid

SELECT * FROM dbo.dbUser WHERE usrID = @feeusrid

SELECT * FROM dbo.dbfeeearner WHERE feeusrID = @feeusrid

SELECT * FROM dbo.dbuserFavourites WHERE usrid = @id OR NOT usrfavdept IS NULL ORDER BY favid asc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprLogin] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprLogin] TO [OMSAdminRole]
    AS [dbo];

