-- =============================================
-- Alter Passenger table: Change RecoveryNo from int to nvarchar(20)
-- Preserves existing numeric values as text (e.g. 543 -> N'543').
-- New records may include leading zeros (e.g. N'0543').
-- =============================================

IF COL_LENGTH('Passenger', 'RecoveryNo') IS NOT NULL
   AND EXISTS (
       SELECT 1
       FROM sys.columns c
       INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
       WHERE c.object_id = OBJECT_ID('dbo.Passenger')
         AND c.name = 'RecoveryNo'
         AND t.name IN ('int', 'bigint', 'smallint', 'tinyint')
   )
BEGIN
    IF EXISTS (
        SELECT 1 FROM sys.indexes
        WHERE name = 'IX_Passenger_RecoveryNo' AND object_id = OBJECT_ID('dbo.Passenger')
    )
    BEGIN
        DROP INDEX [IX_Passenger_RecoveryNo] ON [dbo].[Passenger];
        PRINT 'Dropped index [IX_Passenger_RecoveryNo].';
    END

    ALTER TABLE [dbo].[Passenger]
        ALTER COLUMN [RecoveryNo] NVARCHAR(20) NOT NULL;
    PRINT 'Column [RecoveryNo] changed to NVARCHAR(20).';

    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes
        WHERE name = 'IX_Passenger_RecoveryNo' AND object_id = OBJECT_ID('dbo.Passenger')
    )
    BEGIN
        CREATE NONCLUSTERED INDEX [IX_Passenger_RecoveryNo]
        ON [dbo].[Passenger] ([RecoveryNo] ASC);
        PRINT 'Re-created index [IX_Passenger_RecoveryNo].';
    END
END
ELSE IF COL_LENGTH('Passenger', 'RecoveryNo') IS NOT NULL
BEGIN
    PRINT 'Column [RecoveryNo] is already a string type; no change applied.';
END
ELSE
BEGIN
    PRINT 'Column [RecoveryNo] not found on [dbo].[Passenger].';
END
GO
