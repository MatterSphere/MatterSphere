
-- =============================================
-- Author: Renato Nappo
-- Create date: 20 August 2019
-- Description:	Update ADMINLAST10 & ADMINFAV favourites following Admin Kit tree node editing
-- =============================================
CREATE PROCEDURE [dbo].[sprUpdateAdminFavourites] 
	@id nvarchar(max),
	@objectCode nvarchar(max) = null,
	@description nvarchar(max) = null,
	@roles nvarchar(max) = null
AS
BEGIN
	SET NOCOUNT ON;

	update dbUserFavourites
	set usrFavDesc = @objectCode, usrFavObjParam2 = @description, usrFavObjParam3 = @roles
	where usrFavType in ('ADMINLAST10', 'ADMINFAV')
	and LEFT(usrFavObjParam1, CASE WHEN ISNULL(CHARINDEX(';', usrFavObjParam1), 0) = 0 THEN 0 ELSE CHARINDEX(';', usrFavObjParam1) - 1 END) = @id
END
GO


