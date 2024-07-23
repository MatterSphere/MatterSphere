CREATE TABLE [dbo].[dbTokens] (
	[siteCode] CHAR(2) NOT NULL,
	[userId] INT NULL,
	[accessToken] VARCHAR (2000) NOT NULL,
	[accessTokenExpiresAt] DATETIME NOT NULL,
	[refreshToken] VARCHAR (200) NULL,
	[refreshTokenExpiresAt] DATETIME NULL,
	[rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbTokens_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
	CONSTRAINT [IX_dbTokens] UNIQUE CLUSTERED ([siteCode], [userId]) WITH (FILLFACTOR = 90) ON [IndexGroup],
	CONSTRAINT [FK_dbTokens_dbUser] FOREIGN KEY ([userId]) REFERENCES [dbo].[dbUser] ([usrID]) ON DELETE CASCADE ON UPDATE CASCADE NOT FOR REPLICATION
);

GO