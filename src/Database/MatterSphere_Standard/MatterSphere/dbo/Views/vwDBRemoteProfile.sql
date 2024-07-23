

CREATE VIEW[dbo].[vwDBRemoteProfile]
AS
SELECT     contID AS RemID, proUserName AS RemUserName, proPassword AS RemPasswd, 0 AS RemAccessLevel, proEmail AS RemEmailAddress, 
                      proDefSecSetting AS RemDefSecLevel, proInformSMS AS RemSMSInformChange, proInformEmail AS RemEmailInformChange, 
                      proSMSNumber AS RemSMSNumber, proSecValue AS RemSecLevel
FROM         dbo.dbInteractiveProfile
WHERE     (proEmail <> N'')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBRemoteProfile] TO [OMSApplicationRole]
    AS [dbo];

