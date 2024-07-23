CREATE TABLE [search].[ChangeVersionControl] (
	[LastCopiedVersion]   BIGINT   NOT NULL DEFAULT 0,
    [WorkingVersion] BIGINT  NOT NULL DEFAULT 0, 
    [FullCopyRequired] BIT NOT NULL DEFAULT 0, 
	BatchSize INT NOT NULL DEFAULT 1000
	)
	