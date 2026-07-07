-- =============================================
-- Script to create DisciplinaryForm table in SQL Server
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DisciplinaryForm](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [PassengerId] [int] NULL,
        [PersonName] [nvarchar](500) NULL,
        [CardNo] [nvarchar](50) NULL,
        [FamilyName] [nvarchar](500) NULL,
        [Age] [int] NULL,
        [IsMarried] [bit] NULL,
        [IsEmployed] [bit] NULL,
        [EducationLevel] [int] NULL,
        [AgencyId] [int] NULL,
        [AgencyName] [nvarchar](500) NULL,
        [LegionNo] [nvarchar](50) NULL,
        [PreviousSubstanceUseDetails] [nvarchar](max) NULL,
        [CurrentSubstanceUseDetails] [nvarchar](max) NULL,
        [AbsenceDuration] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_AbsenceDuration] DEFAULT (0),
        [RelapseDuration] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_RelapseDuration] DEFAULT (0),
        [ReferralReasons] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_ReferralReasons] DEFAULT (0),
        [ReferralReasonOtherDetails] [nvarchar](max) NULL,
        [IsCurrentSubstanceUseBelowHalfGram] [bit] NULL,
        [HasSubstanceUseInAnotherBranch] [bit] NULL,
        [HasCigaretteUse] [bit] NULL,
        [CigaretteTreatmentStatus] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_CigaretteTreatmentStatus] DEFAULT (0),
        [HeightCm] [int] NULL,
        [WeightKg] [decimal](18, 4) NULL,
        [HasOverOrUnderWeight] [bit] NULL,
        [HadHealthyDietInPastYear] [bit] NULL,
        [RelapsedDueToWeightIssue] [bit] NULL,
        [HadHealthyWeightBeforeRecovery] [bit] NULL,
        [EducationalResources] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_EducationalResources] DEFAULT (0),
        [WroteOneCdPerWeek] [bit] NULL,
        [CompletedThirtyCdExam] [bit] NULL,
        [CompletedFortyCdExam] [bit] NULL,
        [AttendedParksDuringFirstSixMonths] [bit] NULL,
        [ParticipatedInAtLeastOneSport] [bit] NULL,
        [ParticipatedInSportsActivitiesOrCompetitions] [bit] NULL,
        [HasRegularWorkshopAttendance] [bit] NULL,
        [HasRegularLegionAttendance] [bit] NULL,
        [HadWeeklyParticipationAndTravelDeclarationInFirstTrip] [bit] NULL,
        [TookSecondTripExams] [bit] NULL,
        [ServiceRoles] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_ServiceRoles] DEFAULT (0),
        [FamilyRelapseFactors] [int] NOT NULL CONSTRAINT [DF_DisciplinaryForm_FamilyRelapseFactors] DEFAULT (0),
        [FamilyRelapseFactorOtherDetails] [nvarchar](max) NULL,
        [MedicalConditionAndMedicationNotes] [nvarchar](max) NULL,
        [CreatedOnUtc] [datetime2](7) NOT NULL,
        [CreatedByCustomerId] [int] NULL,
        CONSTRAINT [PK_DisciplinaryForm] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND type in (N'U'))
    BEGIN
        ALTER TABLE [dbo].[DisciplinaryForm] WITH CHECK ADD CONSTRAINT [FK_DisciplinaryForm_Passenger]
        FOREIGN KEY([PassengerId])
        REFERENCES [dbo].[Passenger] ([Id]);

        ALTER TABLE [dbo].[DisciplinaryForm] CHECK CONSTRAINT [FK_DisciplinaryForm_Passenger];
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Passenger table does not exist. Passenger foreign key was not created.';
    END

    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agency]') AND type in (N'U'))
    BEGIN
        ALTER TABLE [dbo].[DisciplinaryForm] WITH CHECK ADD CONSTRAINT [FK_DisciplinaryForm_Agency]
        FOREIGN KEY([AgencyId])
        REFERENCES [dbo].[Agency] ([Id]);

        ALTER TABLE [dbo].[DisciplinaryForm] CHECK CONSTRAINT [FK_DisciplinaryForm_Agency];
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Agency table does not exist. AgencyId foreign key was not created.';
    END

    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
    BEGIN
        ALTER TABLE [dbo].[DisciplinaryForm] WITH CHECK ADD CONSTRAINT [FK_DisciplinaryForm_Customer_CreatedBy]
        FOREIGN KEY([CreatedByCustomerId])
        REFERENCES [dbo].[Customer] ([Id]);

        ALTER TABLE [dbo].[DisciplinaryForm] CHECK CONSTRAINT [FK_DisciplinaryForm_Customer_CreatedBy];
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Customer table does not exist. CreatedByCustomerId foreign key was not created.';
    END

    CREATE UNIQUE NONCLUSTERED INDEX [IX_DisciplinaryForm_PassengerId]
    ON [dbo].[DisciplinaryForm] ([PassengerId] ASC)
    WHERE [PassengerId] IS NOT NULL
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_DisciplinaryForm_CardNo]
    ON [dbo].[DisciplinaryForm] ([CardNo] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_DisciplinaryForm_CreatedOnUtc]
    ON [dbo].[DisciplinaryForm] ([CreatedOnUtc] DESC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    CREATE NONCLUSTERED INDEX [IX_DisciplinaryForm_AgencyId]
    ON [dbo].[DisciplinaryForm] ([AgencyId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'DisciplinaryForm table created successfully.';
END
ELSE
BEGIN
    PRINT 'DisciplinaryForm table already exists.';
END
GO
