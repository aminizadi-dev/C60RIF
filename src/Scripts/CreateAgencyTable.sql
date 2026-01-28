-- =============================================
-- Script to create Agency table in SQL Server
-- =============================================

-- Check if City table exists first
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: City table does not exist. Please run CreateCityTable.sql first.';
    RETURN;
END

-- Check if table exists, if not create it
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

    -- Add Foreign Key constraint to City table
    ALTER TABLE [dbo].[Agency] WITH CHECK ADD CONSTRAINT [FK_Agency_City] 
    FOREIGN KEY([CityId])
    REFERENCES [dbo].[City] ([Id])
    ON DELETE CASCADE;
    
    ALTER TABLE [dbo].[Agency] CHECK CONSTRAINT [FK_Agency_City];

    -- Add index on CityId for foreign key lookups
    CREATE NONCLUSTERED INDEX [IX_Agency_CityId] 
    ON [dbo].[Agency] ([CityId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on Name for search functionality
    CREATE NONCLUSTERED INDEX [IX_Agency_Name] 
    ON [dbo].[Agency] ([Name] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on DisplayOrder for sorting
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

