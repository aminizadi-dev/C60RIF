-- =============================================
-- Alter Passenger table: Make ClinicId nullable
-- Drop the existing FK constraint first, then alter the column,
-- then re-create the FK to allow NULL values.
-- =============================================

-- Step 1: Drop the existing FK constraint
IF EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_Passenger_Clinic' AND parent_object_id = OBJECT_ID('dbo.Passenger')
)
BEGIN
    ALTER TABLE [dbo].[Passenger] DROP CONSTRAINT [FK_Passenger_Clinic];
    PRINT 'Dropped FK constraint [FK_Passenger_Clinic].';
END
GO

-- Step 2: Make ClinicId nullable
IF COL_LENGTH('Passenger', 'ClinicId') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Passenger]
        ALTER COLUMN [ClinicId] INT NULL;
    PRINT 'Column [ClinicId] changed to nullable.';
END
GO

-- Step 3: Re-create FK constraint (now allows NULL)
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_Passenger_Clinic' AND parent_object_id = OBJECT_ID('dbo.Passenger')
)
BEGIN
    ALTER TABLE [dbo].[Passenger]
        ADD CONSTRAINT [FK_Passenger_Clinic]
        FOREIGN KEY ([ClinicId]) REFERENCES [dbo].[Clinic] ([Id]);
    PRINT 'Re-created FK constraint [FK_Passenger_Clinic] (nullable).';
END
GO
