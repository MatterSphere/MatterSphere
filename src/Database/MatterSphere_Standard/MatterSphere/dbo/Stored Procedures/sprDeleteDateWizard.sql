

CREATE PROCEDURE [dbo].[sprDeleteDateWizard]
(
	@kdid bigint
)
AS
   UPDATE    dbKeyDates
   SET              kdActive = 0
   WHERE     (kdID = @kdid) 

DECLARE @kdRelatedID nvarchar(150)

SELECT @kdRelatedID = kdRelatedID FROM dbKeyDates WHERE kdID = @kdID

   UPDATE    dbTasks
   SET              tskActive = 0
   WHERE     (tskRelatedID = @kdRelatedID)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDeleteDateWizard] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDeleteDateWizard] TO [OMSAdminRole]
    AS [dbo];

