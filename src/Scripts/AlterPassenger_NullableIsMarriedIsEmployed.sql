-- =============================================
-- Alter Passenger table: Make IsMarried and IsEmployed nullable
-- Existing TRUE/FALSE values are preserved.
-- New records can have NULL meaning "نامشخص" (Unknown).
-- =============================================

IF COL_LENGTH('Passenger', 'IsMarried') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Passenger]
        ALTER COLUMN [IsMarried] BIT NULL;
    PRINT 'Column [IsMarried] changed to nullable.';
END
GO

IF COL_LENGTH('Passenger', 'IsEmployed') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Passenger]
        ALTER COLUMN [IsEmployed] BIT NULL;
    PRINT 'Column [IsEmployed] changed to nullable.';
END
GO

-- (Optional) Set existing FALSE values to NULL if you want
-- to treat old "false" entries as "unknown" instead:
--
-- UPDATE [dbo].[Passenger] SET [IsMarried]  = NULL WHERE [IsMarried]  = 0;
-- UPDATE [dbo].[Passenger] SET [IsEmployed] = NULL WHERE [IsEmployed] = 0;
-- GO
