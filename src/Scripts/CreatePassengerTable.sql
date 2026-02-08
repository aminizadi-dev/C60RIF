-- =============================================
-- Script to create Passenger table in SQL Server
-- =============================================

-- Check if table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Passenger](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [RecoveryNo] [int] NOT NULL,
        [PersonName] [nvarchar](500) NULL,
        [GuideNameAndLegionNo] [nvarchar](500) NULL,
        [ClinicId] [int] NOT NULL,
        [BirthYear] [int] NULL,
        [Education] [int] NOT NULL,
        [IsMarried] [bit] NOT NULL,
        [IsEmployed] [bit] NOT NULL,
        [HasCompanion] [bit] NULL,
        [CardNo] [bigint] NULL,
        [AntiX1] [int] NOT NULL,
        [AntiX2] [int] NULL,
        [TravelStartDateUtc] [datetime2](7) NULL,
        [TravelEndDateUtc] [datetime2](7) NULL,
        [PictureId] [int] NOT NULL,
        [AgencyId] [int] NOT NULL,
        [CreatedOnUtc] [datetime2](7) NOT NULL,
        CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    -- Add Foreign Key constraint to Agency table
    ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_Agency] 
    FOREIGN KEY([AgencyId])
    REFERENCES [dbo].[Agency] ([Id])
    ON DELETE CASCADE;
    
    ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_Agency];

    -- Add Foreign Key constraint to AntiX table (if exists)
    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AntiX]') AND type in (N'U'))
    BEGIN
        ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_AntiX1] 
        FOREIGN KEY([AntiX1])
        REFERENCES [dbo].[AntiX] ([Id]);

        ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_AntiX2] 
        FOREIGN KEY([AntiX2])
        REFERENCES [dbo].[AntiX] ([Id]);

        ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_AntiX1];
        ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_AntiX2];
    END
    ELSE
    BEGIN
        PRINT 'WARNING: AntiX table does not exist. AntiX foreign keys were not created.';
    END

    -- Add Foreign Key constraint to Clinic table (if exists)
    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clinic]') AND type in (N'U'))
    BEGIN
        ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_Clinic] 
        FOREIGN KEY([ClinicId])
        REFERENCES [dbo].[Clinic] ([Id]);

        ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_Clinic];
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Clinic table does not exist. Clinic foreign key was not created.';
    END

    -- Add index on RecoveryNo for better query performance
    CREATE NONCLUSTERED INDEX [IX_Passenger_RecoveryNo] 
    ON [dbo].[Passenger] ([RecoveryNo] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on PersonName for search functionality
    CREATE NONCLUSTERED INDEX [IX_Passenger_PersonName] 
    ON [dbo].[Passenger] ([PersonName] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on CreatedOnUtc for sorting
    CREATE NONCLUSTERED INDEX [IX_Passenger_CreatedOnUtc] 
    ON [dbo].[Passenger] ([CreatedOnUtc] DESC)
    -- Add index on AgencyId for lookups
    CREATE NONCLUSTERED INDEX [IX_Passenger_AgencyId] 
    ON [dbo].[Passenger] ([AgencyId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on ClinicId for lookups
    CREATE NONCLUSTERED INDEX [IX_Passenger_ClinicId] 
    ON [dbo].[Passenger] ([ClinicId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on AntiX1/AntiX2 for lookups
    CREATE NONCLUSTERED INDEX [IX_Passenger_AntiX1] 
    ON [dbo].[Passenger] ([AntiX1] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_Passenger_AntiX2] 
    ON [dbo].[Passenger] ([AntiX2] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'Passenger table created successfully.';
END
ELSE
BEGIN
    PRINT 'Passenger table already exists.';
END
GO

