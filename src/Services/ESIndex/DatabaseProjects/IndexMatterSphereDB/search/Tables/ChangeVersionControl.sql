CREATE TABLE search.ChangeVersionControl (
    LastCopiedVersion   BIGINT   NOT NULL DEFAULT 0,
    WorkingVersion BIGINT  NOT NULL DEFAULT 0,
    FullCopyRequired BIT NOT NULL DEFAULT 0,
    ReindexFailedItems TINYINT NOT NULL DEFAULT 0,
    ProcessOrder BIT NOT NULL DEFAULT 0,
    BatchSize INT NOT NULL DEFAULT 1000,
    DocumentDateLimit DATETIME NULL DEFAULT NULL,
    PreviousDocumentDateLimit DATETIME NULL DEFAULT NULL,
    SummaryFieldEnabled BIT NOT NULL DEFAULT 0
    )
