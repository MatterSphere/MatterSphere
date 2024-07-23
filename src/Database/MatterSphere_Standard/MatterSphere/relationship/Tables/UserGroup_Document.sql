CREATE TABLE [relationship].[UserGroup_Document] (
    [RelationshipID] UNIQUEIDENTIFIER CONSTRAINT [DF_UserGroup_Document_RelationshipID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserGroupID]    UNIQUEIDENTIFIER NOT NULL,
    [DocumentID]     BIGINT           NOT NULL,
    [PolicyID]       UNIQUEIDENTIFIER NOT NULL,
    [clid]           BIGINT           DEFAULT ((0)) NOT NULL,
    [fileid]         BIGINT           DEFAULT ((0)) NOT NULL,
    [inherited]      CHAR (1)         NULL,
    CONSTRAINT [PK_UserGroup_Document] PRIMARY KEY CLUSTERED ([RelationshipID] ASC),
    CONSTRAINT [FK_UserGroup_Document_ObjectPolicy] FOREIGN KEY ([PolicyID]) REFERENCES [config].[ObjectPolicy] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [IX_UserGroup_Document] UNIQUE NONCLUSTERED ([UserGroupID] ASC, [DocumentID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_Document_FileId]
    ON [relationship].[UserGroup_Document]([PolicyID] ASC, [fileid] ASC)
    INCLUDE([UserGroupID], [DocumentID]);


GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_Document_ClientId]
    ON [relationship].[UserGroup_Document]([PolicyID] ASC, [clid] ASC)
    INCLUDE([UserGroupID], [DocumentID], [fileid]);


GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_Document_DocumentID_UserGroupID]
    ON [relationship].[UserGroup_Document]([DocumentID] ASC, [UserGroupID] ASC)
    INCLUDE([RelationshipID], [PolicyID]);


GO
GRANT UPDATE
    ON OBJECT::[relationship].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[relationship].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[relationship].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[relationship].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];

