IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_dbTracking_logID' AND OBJECT_ID('[dbo].[dbTracking]') = object_id)
CREATE NONCLUSTERED INDEX IX_dbTracking_logID
    ON dbo.dbTracking(logID, trackCheckedIn) WITH (FILLFACTOR = 90)
    ON IndexGroup;
