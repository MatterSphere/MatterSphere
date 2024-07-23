

CREATE PROCEDURE [dbo].[sprAddMenuItem]
(
	@mcode nvarchar(15),
	@mparent nvarchar(15),
	@mimage int,
	@mdescription nvarchar(150),
	@maction nvarchar(100),
	@mvisibleinsidebar bit,
	@mincfav bit,
        @morder int,
	@mroles nvarchar(150),
	@mname nvarchar(15) = 'ADMIN'
)
AS


-- catch to prevent duplicate entries
IF EXISTS ( SELECT admnuID FROM dbo.dbAdminMenu WHERE admnuname = @mname AND admnucode = @mcode AND Coalesce(AdmnuSearchListCode, '') = Coalesce(@maction, '') )
	RETURN

DECLARE @parentid int

SELECT TOP 1 @parentid = admnuID FROM dbAdminMenu WHERE admnuCode = @mparent AND admnuName = @mname ORDER BY Convert(int,admnuID) ASC
IF @parentid is null
BEGIN
 SET @parentid = 1
END

DELETE FROM dbAdminMenu WHERE admnuCode = @mcode AND admnuParent = @parentid AND Coalesce(AdmnuSearchListCode, '') = Coalesce(@maction, '')

INSERT INTO dbAdminMenu (admnuName,admnuParent,admnuCode,admnuImageIndex,admnuSearchListCode,admnuVisibleInSideBar,admnuIncFav,admnuOrder,admnuRoles) 
VALUES 
(@mname,@parentid,@mcode,@mimage,@maction,@mvisibleinsidebar,@mincfav,@morder,@mroles)

DELETE FROM dbCodeLookup WHERE cdCode = @mcode AND cdType = 'ADMINMENU'

INSERT INTO dbCodeLookup (cdType,cdCode,cdUICultureInfo,cdDesc,cdSystem,cdDeletable,cdHelp,cdGroup) 
VALUES
('ADMINMENU',@mcode,'{default}',@mdescription,1,0,null,0)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddMenuItem] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddMenuItem] TO [OMSAdminRole]
    AS [dbo];

