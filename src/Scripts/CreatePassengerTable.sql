-- =============================================
-- Script to create Passenger table in SQL Server
-- =============================================

-- Check if table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Passenger](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [RecoveryNo] [int] NOT NULL,
        [RecoveryYear] [int] NOT NULL,
        [RecoveryMonth] [nvarchar](50) NULL,
        [PersonName] [nvarchar](500) NULL,
        [BranchName] [nvarchar](500) NULL,
        [GuideNameAndLegionNo] [nvarchar](500) NULL,
        [ClinicName] [nvarchar](500) NULL,
        [BirthDateUtc] [datetime2](7) NULL,
        [Education] [int] NOT NULL,
        [MaritalStatus] [int] NOT NULL,
        [EmploymentStatus] [int] NOT NULL,
        [CardNo] [bigint] NULL,
        [AntiX1] [nvarchar](500) NULL,
        [AntiX2] [nvarchar](500) NULL,
        [TravelStartDateUtc] [datetime2](7) NULL,
        [TravelEndDateUtc] [datetime2](7) NULL,
        [PictureId] [int] NOT NULL,
        [CreatedOnUtc] [datetime2](7) NOT NULL,
        CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    -- Add index on RecoveryNo and RecoveryYear for better query performance
    CREATE NONCLUSTERED INDEX [IX_Passenger_RecoveryNo_RecoveryYear] 
    ON [dbo].[Passenger] ([RecoveryNo] ASC, [RecoveryYear] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on PersonName for search functionality
    CREATE NONCLUSTERED INDEX [IX_Passenger_PersonName] 
    ON [dbo].[Passenger] ([PersonName] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on CreatedOnUtc for sorting
    CREATE NONCLUSTERED INDEX [IX_Passenger_CreatedOnUtc] 
    ON [dbo].[Passenger] ([CreatedOnUtc] DESC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'Passenger table created successfully.';
END
ELSE
BEGIN
    PRINT 'Passenger table already exists.';
END
GO

