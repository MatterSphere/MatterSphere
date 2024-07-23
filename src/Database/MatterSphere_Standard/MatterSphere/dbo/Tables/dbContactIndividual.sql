CREATE TABLE [dbo].[dbContactIndividual] (
    [contID]                BIGINT              NOT NULL,
    [contTitle]             NVARCHAR (10)       NULL,
    [contInitials]          NVARCHAR (50)       NULL,
    [contChristianNames]    NVARCHAR (50)       NULL,
    [contSurname]           NVARCHAR (50)       NULL,
    [contMaidenName]        NVARCHAR (30)       NULL,
    [contMaritalStatus]     [dbo].[uCodeLookup] NULL,
    [contOccupation]        [dbo].[uCodeLookup] NULL,
    [contDOB]               DATETIME            NULL,
    [contDOD]               DATETIME            NULL,
    [contPOB]               NVARCHAR (100)      NULL,
    [contSex]               NVARCHAR (1)        NULL,
    [contReligion]          NVARCHAR (50)       NULL,
    [contNationality]       NVARCHAR (50)       NULL,
    [contNINumber]          NVARCHAR (50)       NULL,
    [contNOK]               NVARCHAR (50)       NULL,
    [contNOKaddID]          BIGINT              NULL,
    [contIdentity]          [dbo].[uCodeLookup] NULL,
    [contIdentity2]         [dbo].[uCodeLookup] NULL,
    [contSpouseName]        NVARCHAR (30)       NULL,
    [contSpouseName2]       NVARCHAR (30)       NULL,
    [contEthnicOrigin]      TINYINT             NULL,
    [contInterest1]         [dbo].[uCodeLookup] NULL,
    [contInterest2]         [dbo].[uCodeLookup] NULL,
    [contInterest3]         [dbo].[uCodeLookup] NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbContactIndividual_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [contDisabilityFlag]    [dbo].[uCodeLookup] NULL,
    [contResidence]         BIGINT              NULL,
    [contResidencePrevious] BIGINT              NULL,
    [contResidenceSince]    SMALLDATETIME       NULL,
    CONSTRAINT [PK_DBClientIndividual] PRIMARY KEY CLUSTERED ([contID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbContactIndividual_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbContactIndividual_rowguid]
    ON [dbo].[dbContactIndividual]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbContactIndividual] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbContactIndividual] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbContactIndividual] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbContactIndividual] TO [OMSApplicationRole]
    AS [dbo];

