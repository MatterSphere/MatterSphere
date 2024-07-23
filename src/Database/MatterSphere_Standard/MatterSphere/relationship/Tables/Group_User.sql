CREATE TABLE [relationship].[Group_User] (
    [GroupID] UNIQUEIDENTIFIER NOT NULL,
    [UserID]  UNIQUEIDENTIFIER NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_Group_User_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_Group_User] PRIMARY KEY CLUSTERED ([GroupID] ASC, [UserID] ASC),
    CONSTRAINT [FK_Group_User_Group_ID] FOREIGN KEY ([GroupID]) REFERENCES [item].[Group] ([ID]) ON DELETE CASCADE
);




GO
GRANT UPDATE
    ON OBJECT::[relationship].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[relationship].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[relationship].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[relationship].[Group_User] TO [OMSApplicationRole]
    AS [dbo];

