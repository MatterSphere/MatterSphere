
CREATE PROCEDURE [dbo].[sprAddMenuItemV7]
(
	@mname nvarchar(15) = 'ADMIN',
	@mcode nvarchar(15),
	@mparent nvarchar(15),
	@mimage int,
	@mdescription nvarchar(150),
	@maction nvarchar(100),
	@mvisibleinsidebar bit,
	@mincfav bit,
    @morder int,
	@mroles nvarchar(150),
	@mconsole nvarchar(15),
	@system bit
)
AS

-- catch to prevent duplicate entries
IF EXISTS ( SELECT admnuID FROM dbo.dbAdminMenu WHERE admnuname = @mname AND admnucode = @mcode AND admnuSearchListCode is null )
	RETURN

DECLARE @parentid int

SELECT TOP 1 @parentid = admnuID FROM dbAdminMenu WHERE admnuCode = @mparent AND admnuName = @mname ORDER BY Convert(int,admnuID) ASC
IF @parentid is null
BEGIN
 SET @parentid = (SELECT TOP 1 admnuID FROM dbAdminMenu WHERE admnuSearchListCode = @mconsole)
END

DELETE FROM dbAdminMenu WHERE admnuCode = @mcode AND admnuParent = @parentid AND Coalesce(AdmnuSearchListCode, '') = Coalesce(@mconsole, '')

INSERT INTO dbAdminMenu (admnuName,admnuParent,admnuCode,admnuImageIndex,admnuSearchListCode,admnuVisibleInSideBar,admnuIncFav,admnuOrder,admnuRoles, admnuSystem) 
VALUES 
(@mname,@parentid,@mcode,@mimage,null,@mvisibleinsidebar,@mincfav,@morder,@mroles,@system)
DELETE FROM dbCodeLookup WHERE cdCode = @mcode AND cdType = 'ADMINMENU'

INSERT INTO dbCodeLookup (cdType,cdCode,cdUICultureInfo,cdDesc,cdSystem,cdDeletable,cdHelp,cdGroup) 
VALUES
('ADMINMENU',@mcode,'{default}',@mdescription,1,0,null,0)





SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddMenuItemV7] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddMenuItemV7] TO [OMSAdminRole]
    AS [dbo];

