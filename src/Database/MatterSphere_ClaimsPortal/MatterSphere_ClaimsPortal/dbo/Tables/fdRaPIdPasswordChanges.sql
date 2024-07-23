CREATE TABLE [dbo].[fdRaPIdPasswordChanges]
(
	[UpdatedBy] INT NOT NULL,
	[Updated] DATETIME NOT NULL CONSTRAINT [DF_fdRaPIdPasswordChanges_Updated] DEFAULT GETUTCDATE()
)
