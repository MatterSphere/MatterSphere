

CREATE VIEW[dbo].[vwDBAddMatterInfo]
AS
SELECT     fileID AS MatterID, fileField1 AS MADField1, fileField2 AS MADField2, fileField3 AS MADField3, fileField4 AS MADField4, fileField5 AS MADField5, 
                      fileField6 AS MADField6, fileField7 AS MADField7, fileField8 AS MADField8, fileField9 AS MADField9, fileField10 AS MADField10, 
                      fileDate1 AS MADFieldDate1, fileDate2 AS MADFieldDate2, fileDate3 AS MADFieldDate3, fileDate4 AS MADFieldDate4, fileDate5 AS MADFieldDate5, 
                      Updated AS MADUpdated, dbo.GetUser(UpdatedBy, NULL) AS MADUpdatedBy
FROM         dbo.dbAddFileInfo

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBAddMatterInfo] TO [OMSApplicationRole]
    AS [dbo];

