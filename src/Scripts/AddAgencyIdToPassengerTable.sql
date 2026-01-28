-- =============================================
-- Script to add AgencyId column to Passenger table
-- =============================================

-- Check if Passenger table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: Passenger table does not exist.';
    RETURN;
END

-- Check if Agency table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agency]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: Agency table does not exist. Please run CreateAgencyTable.sql first.';
    RETURN;
END

-- Check if City table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: City table does not exist. Please run CreateCityTable.sql first.';
    RETURN;
END

-- Check if AgencyId column already exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND name = 'AgencyId')
BEGIN
    PRINT 'Adding AgencyId column to Passenger table...';

    -- Step 1: Create default City if none exists
    IF NOT EXISTS (SELECT 1 FROM [dbo].[City])
    BEGIN
        INSERT INTO [dbo].[City] ([Name], [Published], [DisplayOrder]) 
        VALUES (N'شهر پیش‌فرض', 1, 0);
        PRINT 'Default City created.';
    END

    -- Step 2: Create default Agency if none exists
    DECLARE @DefaultCityId INT = (SELECT TOP 1 [Id] FROM [dbo].[City] ORDER BY [Id]);
    
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Agency])
    BEGIN
        INSERT INTO [dbo].[Agency] ([CityId], [Name], [Published], [DisplayOrder]) 
        VALUES (@DefaultCityId, N'نمایندگی پیش‌فرض', 1, 0);
        PRINT 'Default Agency created.';
    END

    -- Step 3: Add AgencyId column as nullable first
    ALTER TABLE [dbo].[Passenger]
    ADD [AgencyId] [int] NULL;

    PRINT 'AgencyId column added (nullable).';

    -- Step 4: Update all existing Passenger records to use the first available Agency
    DECLARE @DefaultAgencyId INT = (SELECT TOP 1 [Id] FROM [dbo].[Agency] ORDER BY [Id]);
    
    UPDATE [dbo].[Passenger] 
    SET [AgencyId] = @DefaultAgencyId 
    WHERE [AgencyId] IS NULL;

    PRINT 'All existing Passenger records updated with default Agency.';

    -- Step 5: Make column NOT NULL
    ALTER TABLE [dbo].[Passenger]
    ALTER COLUMN [AgencyId] [int] NOT NULL;

    PRINT 'AgencyId column set to NOT NULL.';

    -- Step 6: Add Foreign Key constraint
    ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_Agency] 
    FOREIGN KEY([AgencyId])
    REFERENCES [dbo].[Agency] ([Id])
    ON DELETE CASCADE;
    
    ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_Agency];

    PRINT 'Foreign Key constraint added.';

    -- Step 7: Add index on AgencyId for foreign key lookups
    CREATE NONCLUSTERED INDEX [IX_Passenger_AgencyId] 
    ON [dbo].[Passenger] ([AgencyId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'Index on AgencyId created.';
    PRINT 'AgencyId column added to Passenger table successfully.';
END
ELSE
BEGIN
    PRINT 'AgencyId column already exists in Passenger table.';
END
GO

