-- =============================================
-- Complete Script to Create/Update City, Agency and Passenger Tables
-- Run this script to set up all tables in the correct order
-- =============================================

PRINT '========================================';
PRINT 'Starting City, Agency and Passenger table setup...';
PRINT '========================================';
PRINT '';

-- Step 1: Create City table
PRINT 'Step 1: Creating City table...';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[City](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](500) NOT NULL,
        [Published] [bit] NOT NULL,
        [DisplayOrder] [int] NOT NULL,
        CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_City_Name] 
    ON [dbo].[City] ([Name] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_City_DisplayOrder] 
    ON [dbo].[City] ([DisplayOrder] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'City table created successfully.';
END
ELSE
BEGIN
    PRINT 'City table already exists.';
END
GO

-- Step 2: Create Agency table
PRINT '';
PRINT 'Step 2: Creating Agency table...';
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agency]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agency](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CityId] [int] NOT NULL,
        [Name] [nvarchar](500) NOT NULL,
        [Published] [bit] NOT NULL,
        [DisplayOrder] [int] NOT NULL,
        CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    ALTER TABLE [dbo].[Agency] WITH CHECK ADD CONSTRAINT [FK_Agency_City] 
    FOREIGN KEY([CityId])
    REFERENCES [dbo].[City] ([Id])
    ON DELETE CASCADE;
    
    ALTER TABLE [dbo].[Agency] CHECK CONSTRAINT [FK_Agency_City];

    CREATE NONCLUSTERED INDEX [IX_Agency_CityId] 
    ON [dbo].[Agency] ([CityId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_Agency_Name] 
    ON [dbo].[Agency] ([Name] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_Agency_DisplayOrder] 
    ON [dbo].[Agency] ([DisplayOrder] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'Agency table created successfully.';
END
ELSE
BEGIN
    PRINT 'Agency table already exists.';
END
GO

-- Step 3: Create default City and Agency if they don't exist
PRINT '';
PRINT 'Step 3: Creating default City and Agency if needed...';
IF NOT EXISTS (SELECT 1 FROM [dbo].[City])
BEGIN
    INSERT INTO [dbo].[City] ([Name], [Published], [DisplayOrder]) 
    VALUES (N'شهر پیش‌فرض', 1, 0);
    PRINT 'Default City created.';
END

DECLARE @DefaultCityId INT = (SELECT TOP 1 [Id] FROM [dbo].[City] ORDER BY [Id]);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Agency])
BEGIN
    INSERT INTO [dbo].[Agency] ([CityId], [Name], [Published], [DisplayOrder]) 
    VALUES (@DefaultCityId, N'نمایندگی پیش‌فرض', 1, 0);
    PRINT 'Default Agency created.';
END
GO

-- Step 4: Add AgencyId to Passenger table
PRINT '';
PRINT 'Step 4: Adding AgencyId column to Passenger table...';
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND name = 'AgencyId')
    BEGIN
        -- Add column as nullable first
        ALTER TABLE [dbo].[Passenger]
        ADD [AgencyId] [int] NULL;

        PRINT 'AgencyId column added (nullable).';

        -- Update all existing records
        DECLARE @DefaultAgencyId INT = (SELECT TOP 1 [Id] FROM [dbo].[Agency] ORDER BY [Id]);
        
        UPDATE [dbo].[Passenger] 
        SET [AgencyId] = @DefaultAgencyId 
        WHERE [AgencyId] IS NULL;

        PRINT 'All existing Passenger records updated with default Agency.';

        -- Make NOT NULL
        ALTER TABLE [dbo].[Passenger]
        ALTER COLUMN [AgencyId] [int] NOT NULL;

        PRINT 'AgencyId column set to NOT NULL.';

        -- Add Foreign Key
        ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_Agency] 
        FOREIGN KEY([AgencyId])
        REFERENCES [dbo].[Agency] ([Id])
        ON DELETE CASCADE;
        
        ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_Agency];

        PRINT 'Foreign Key constraint added.';

        -- Add index
        CREATE NONCLUSTERED INDEX [IX_Passenger_AgencyId] 
        ON [dbo].[Passenger] ([AgencyId] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

        PRINT 'Index on AgencyId created.';
        PRINT 'AgencyId column added successfully.';
    END
    ELSE
    BEGIN
        PRINT 'AgencyId column already exists in Passenger table.';
    END
END
ELSE
BEGIN
    PRINT 'WARNING: Passenger table does not exist. Skipping AgencyId addition.';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Setup completed successfully!';
PRINT '========================================';
GO

