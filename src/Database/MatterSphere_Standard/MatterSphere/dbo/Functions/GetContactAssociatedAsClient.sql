

CREATE FUNCTION [dbo].[GetContactAssociatedAsClient] (	@clid bigint )
RETURNS nvarchar(1000)
AS
BEGIN
	declare @clist nvarchar(1000)
	SELECT
		@clist = Coalesce( @clist + ',' , '' ) +  C.ContName
	FROM
		dbo.dbClientContacts CC
	JOIN
		dbContact C ON CC.contId = C.contId
	WHERE
		CC.clId = @clId AND
		CC.clActive = 1 AND
		C.contIsClient = 1
	return @clist
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactAssociatedAsClient] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactAssociatedAsClient] TO [OMSAdminRole]
    AS [dbo];

