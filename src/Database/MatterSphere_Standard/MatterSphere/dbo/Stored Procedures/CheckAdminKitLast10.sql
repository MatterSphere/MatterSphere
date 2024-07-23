

CREATE PROCEDURE [dbo].[CheckAdminKitLast10] 
(
    @objID nvarchar(2000) 
    ,@objDesc nvarchar(100)
	,@favType nvarchar(100)
	,@userID int
)

AS

if exists (
select 1 from dbUserFavourites where 
                           usrFavType = @favType and
                           isnull(usrFavObjParam1,'') = @objID and
                           isnull(usrFavObjParam2,'') = @objDesc and
						   usrID = @userID
						   )
begin
       select 1 as result
end
else
begin
       select 0 as result
end


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckAdminKitLast10] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckAdminKitLast10] TO [OMSAdminRole]
    AS [dbo];

