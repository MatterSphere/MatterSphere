CREATE TABLE [dbo].[dbRiskAnswers] (
    [fileID]        BIGINT           NOT NULL,
    [datqID]        INT              NOT NULL,
    [datAnswer]     NVARCHAR (50)    NULL,
    [datAnswerNote] NVARCHAR (255)   NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_dbRiskAnswers_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_DBRiskData] PRIMARY KEY CLUSTERED ([fileID] ASC, [datqID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbRiskAnswers_rowguid]
    ON [dbo].[dbRiskAnswers]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRiskAnswers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRiskAnswers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRiskAnswers] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRiskAnswers] TO [OMSApplicationRole]
    AS [dbo];

