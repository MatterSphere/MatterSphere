CREATE TABLE [dbo].[eEmailMessageConfig] (
    [ID]                            BIGINT           IDENTITY (1, 1) NOT NULL,
    [emailDocInternalChgSubjectMsg] NVARCHAR (50)    NULL,
    [emailDocInternalChgBodyMsg]    NVARCHAR (MAX)   NULL,
    [emailDocExternalChgSubjectMsg] NVARCHAR (50)    NULL,
    [emailDocExternalChgBodyMsg]    NVARCHAR (MAX)   NULL,
    [emailForgotPWReqSubjectMsg]    NVARCHAR (50)    NULL,
    [emailForgotPWReqBodyMsg]       NVARCHAR (MAX)   NULL,
    [emailSuccessPWSubjectMsg]      NVARCHAR (50)    NULL,
    [emailSuccessPWBodyMsg]         NVARCHAR (MAX)   NULL,
    [rowguid]                       UNIQUEIDENTIFIER CONSTRAINT [DF_eEmailMessageConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_eEmailMessageConfig] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[eEmailMessageConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[eEmailMessageConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[eEmailMessageConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[eEmailMessageConfig] TO [OMSApplicationRole]
    AS [dbo];

