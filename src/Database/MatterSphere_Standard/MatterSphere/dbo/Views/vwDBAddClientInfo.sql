

CREATE VIEW[dbo].[vwDBAddClientInfo]
AS
SELECT     dbo.dbAddClientInfo.clField1 AS AdField1, dbo.dbAddClientInfo.clField2 AS AdField2, dbo.dbAddClientInfo.clField3 AS AdField3, 
                      dbo.dbAddClientInfo.clField4 AS Adfield4, dbo.dbAddClientInfo.clField5 AS AdField5, dbo.dbAddClientInfo.clField6 AS AdField6, 
                      dbo.dbAddClientInfo.clField7 AS AdField7, dbo.dbAddClientInfo.clField8 AS AdField8, dbo.dbAddClientInfo.clField9 AS AdField9, 
                      dbo.dbAddClientInfo.clField10 AS AdField10, dbo.dbAddClientInfo.clDate1 AS AdDate1, dbo.dbAddClientInfo.clDate2 AS AdDate2, 
                      dbo.dbAddClientInfo.clDate3 AS AdDate3, dbo.dbAddClientInfo.clDate4 AS AdDate4, dbo.dbAddClientInfo.clDate5 AS AdDate5, 
                      dbo.dbAddClientInfo.Updated AS AdUpdated, dbo.GetUser(dbo.dbAddClientInfo.UpdatedBy, NULL) AS AdUpdatedBy, 
                      dbo.dbContactIndividual.contEthnicOrigin AS AdEthnicOrigin, dbo.dbClient.clID
FROM         dbo.dbClient LEFT OUTER JOIN
                      dbo.dbAddClientInfo ON dbo.dbClient.clID = dbo.dbAddClientInfo.clID LEFT OUTER JOIN
                      dbo.dbContactIndividual ON dbo.dbClient.clDefaultContact = dbo.dbContactIndividual.contID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBAddClientInfo] TO [OMSApplicationRole]
    AS [dbo];

