

CREATE PROCEDURE [dbo].[sprRestoreDateWizard]
(
	@kdid bigint
)
AS
   UPDATE    dbKeyDates
   SET              kdActive = 1
   WHERE     (kdID = @kdid) 

DECLARE @kdRelatedID nvarchar(150)

SELECT @kdRelatedID = kdRelatedID FROM dbKeyDates WHERE kdID = @kdID

   UPDATE    dbTasks
   SET              tskActive = 1
   WHERE     (tskRelatedID = @kdRelatedID)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRestoreDateWizard] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRestoreDateWizard] TO [OMSAdminRole]
    AS [dbo];

