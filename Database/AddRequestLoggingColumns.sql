-- Run this once against the AionGameCP database.
-- Adds Username, IpAddress, and Path columns to the existing AppLogs table.
-- If AppLogs does not exist yet, Serilog will create it with all columns on first startup.

USE AionGameCP;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('AppLogs') AND name = 'Username')
    ALTER TABLE AppLogs ADD Username nvarchar(256) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('AppLogs') AND name = 'IpAddress')
    ALTER TABLE AppLogs ADD IpAddress nvarchar(45) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('AppLogs') AND name = 'Path')
    ALTER TABLE AppLogs ADD [Path] nvarchar(1000) NULL;

-- Optional: index for fast filtering by user/level/date in the admin viewer
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('AppLogs') AND name = 'IX_AppLogs_Username')
    CREATE NONCLUSTERED INDEX IX_AppLogs_Username ON AppLogs (Username) INCLUDE (TimeStamp, Level);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('AppLogs') AND name = 'IX_AppLogs_Level')
    CREATE NONCLUSTERED INDEX IX_AppLogs_Level ON AppLogs (Level) INCLUDE (TimeStamp, Username);
GO
