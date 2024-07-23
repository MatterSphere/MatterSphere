

CREATE VIEW[dbo].[vwDBRemoteProfileMulti]
AS
SELECT     contID AS RemID, clid AS ClientID, fileid AS MatterID, proContact AS RemProContact, proMilestones AS RemProMileStones, COALESCE (proNotes, 0) 
                      AS RemProNotes, proActionable AS RemProActionable, proDocs AS RemProDocs, proSecValue AS RemProSecValue, proSMS AS RemProSMS
FROM         dbo.dbInteractiveFileProfile

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBRemoteProfileMulti] TO [OMSApplicationRole]
    AS [dbo];

