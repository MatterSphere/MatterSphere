CREATE TABLE [dbo].[dbMSData_OMS2K] (
    [fileID]              BIGINT           NOT NULL,
    [MSCode]              NVARCHAR (15)    NOT NULL,
    [MSNextDueDate]       DATETIME         NULL,
    [MSNextDueStage]      TINYINT          NULL,
    [MSStage1Due]         DATETIME         NULL,
    [MSStage2Due]         DATETIME         NULL,
    [MSStage3Due]         DATETIME         NULL,
    [MSStage4Due]         DATETIME         NULL,
    [MSStage5Due]         DATETIME         NULL,
    [MSStage6Due]         DATETIME         NULL,
    [MSStage7Due]         DATETIME         NULL,
    [MSStage8Due]         DATETIME         NULL,
    [MSStage9Due]         DATETIME         NULL,
    [MSStage10Due]        DATETIME         NULL,
    [MSStage11Due]        DATETIME         NULL,
    [MSStage12Due]        DATETIME         NULL,
    [MSStage13Due]        DATETIME         NULL,
    [MSStage14Due]        DATETIME         NULL,
    [MSStage15Due]        DATETIME         NULL,
    [MSStage16Due]        DATETIME         NULL,
    [MSStage17Due]        DATETIME         NULL,
    [MSStage18Due]        DATETIME         NULL,
    [MSStage19Due]        DATETIME         NULL,
    [MSStage20Due]        DATETIME         NULL,
    [MSStage21Due]        DATETIME         NULL,
    [MSStage22Due]        DATETIME         NULL,
    [MSStage23Due]        DATETIME         NULL,
    [MSStage24Due]        DATETIME         NULL,
    [MSStage25Due]        DATETIME         NULL,
    [MSStage26Due]        DATETIME         NULL,
    [MSStage27Due]        DATETIME         NULL,
    [MSStage28Due]        DATETIME         NULL,
    [MSStage29Due]        DATETIME         NULL,
    [MSStage30Due]        DATETIME         NULL,
    [MSStage1Achieved]    DATETIME         NULL,
    [MSStage2Achieved]    DATETIME         NULL,
    [MSStage3Achieved]    DATETIME         NULL,
    [MSStage4Achieved]    DATETIME         NULL,
    [MSStage5Achieved]    DATETIME         NULL,
    [MSStage6Achieved]    DATETIME         NULL,
    [MSStage7Achieved]    DATETIME         NULL,
    [MSStage8Achieved]    DATETIME         NULL,
    [MSStage9Achieved]    DATETIME         NULL,
    [MSStage10Achieved]   DATETIME         NULL,
    [MSStage11Achieved]   DATETIME         NULL,
    [MSStage12Achieved]   DATETIME         NULL,
    [MSStage13Achieved]   DATETIME         NULL,
    [MSStage14Achieved]   DATETIME         NULL,
    [MSStage15Achieved]   DATETIME         NULL,
    [MSStage16Achieved]   DATETIME         NULL,
    [MSStage17Achieved]   DATETIME         NULL,
    [MSStage18Achieved]   DATETIME         NULL,
    [MSStage19Achieved]   DATETIME         NULL,
    [MSStage20Achieved]   DATETIME         NULL,
    [MSStage21Achieved]   DATETIME         NULL,
    [MSStage22Achieved]   DATETIME         NULL,
    [MSStage23Achieved]   DATETIME         NULL,
    [MSStage24Achieved]   DATETIME         NULL,
    [MSStage25Achieved]   DATETIME         NULL,
    [MSStage26Achieved]   DATETIME         NULL,
    [MSStage27Achieved]   DATETIME         NULL,
    [MSStage28Achieved]   DATETIME         NULL,
    [MSStage29Achieved]   DATETIME         NULL,
    [MSStage30Achieved]   DATETIME         NULL,
    [MSStage1AchievedBy]  INT              NULL,
    [MSStage2AchievedBy]  INT              NULL,
    [MSStage3AchievedBy]  INT              NULL,
    [MSStage4AchievedBy]  INT              NULL,
    [MSStage5AchievedBy]  INT              NULL,
    [MSStage6AchievedBy]  INT              NULL,
    [MSStage7AchievedBy]  INT              NULL,
    [MSStage8AchievedBy]  INT              NULL,
    [MSStage9AchievedBy]  INT              NULL,
    [MSStage10AchievedBy] INT              NULL,
    [MSStage11AchievedBy] INT              NULL,
    [MSStage12AchievedBy] INT              NULL,
    [MSStage13AchievedBy] INT              NULL,
    [MSStage14AchievedBy] INT              NULL,
    [MSStage15AchievedBy] INT              NULL,
    [MSStage16AchievedBy] INT              NULL,
    [MSStage17AchievedBy] INT              NULL,
    [MSStage18AchievedBy] INT              NULL,
    [MSStage19AchievedBy] INT              NULL,
    [MSStage20AchievedBy] INT              NULL,
    [MSStage21AchievedBy] INT              NULL,
    [MSStage22AchievedBy] INT              NULL,
    [MSStage23AchievedBy] INT              NULL,
    [MSStage24AchievedBy] INT              NULL,
    [MSStage25AchievedBy] INT              NULL,
    [MSStage26AchievedBy] INT              NULL,
    [MSStage27AchievedBy] INT              NULL,
    [MSStage28AchievedBy] INT              NULL,
    [MSStage29AchievedBy] INT              NULL,
    [MSStage30AchievedBy] INT              NULL,
    [MSActive]            BIT              NULL,
    [MSCreated]           DATETIME         NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_dbMSData_OMS2K_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMSData_OMS2K] PRIMARY KEY CLUSTERED ([fileID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbMSData_OMS2K_dbMSConfig_OMS2K] FOREIGN KEY ([MSCode]) REFERENCES [dbo].[dbMSConfig_OMS2K] ([MSCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMSData_OMS2K_rowguid]
    ON [dbo].[dbMSData_OMS2K]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMSData_OMS2K] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMSData_OMS2K] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMSData_OMS2K] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMSData_OMS2K] TO [OMSApplicationRole]
    AS [dbo];

