CREATE TABLE [relationship].[UserGroup_File] (
    [RelationshipID] UNIQUEIDENTIFIER CONSTRAINT [DF_UserGroup_File_RelationshipID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserGroupID]    UNIQUEIDENTIFIER NOT NULL,
    [FileID]         BIGINT           NOT NULL,
    [PolicyID]       UNIQUEIDENTIFIER NOT NULL,
    [clid]           BIGINT           DEFAULT ((0)) NOT NULL,
    [inherited]      CHAR (1)         NULL,
    CONSTRAINT [PK_UserGroup_File] PRIMARY KEY CLUSTERED ([RelationshipID] ASC),
    CONSTRAINT [FK_UserGroup_File_ObjectPolicy] FOREIGN KEY ([PolicyID]) REFERENCES [config].[ObjectPolicy] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [IX_UserGroup_File] UNIQUE NONCLUSTERED ([UserGroupID] ASC, [FileID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_File_FileId_UserGroupID]
    ON [relationship].[UserGroup_File]([FileID] ASC, [UserGroupID] ASC)
    INCLUDE([RelationshipID], [PolicyID]);


GO
GRANT UPDATE
    ON OBJECT::[relationship].[UserGroup_File] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[relationship].[UserGroup_File] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[relationship].[UserGroup_File] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[relationship].[UserGroup_File] TO [OMSApplicationRole]
    AS [dbo];


GO

CREATE NONCLUSTERED INDEX [IX_UserGroup_File_Clid] ON [relationship].[UserGroup_File] 
(
	[clid] ASC,
	[PolicyID] ASC
)
INCLUDE ( 	[FileID],
	[UserGroupID],
	[RelationshipID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
