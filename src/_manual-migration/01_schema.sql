/* =============================================================================
   STEP 1 - SCHEMA
   Creates the new tables (Person, DisciplinaryForm), renames Passenger ->
   RecoveryForm and adds the (still nullable) PersonId link column.
   The legacy identity columns (PersonName, CardNo, BirthYear) are intentionally
   KEPT here; they are consumed and dropped in step 2 (02_data.sql).

   Safe to re-run (idempotent). Run against the PRODUCTION database.
   ============================================================================= */
SET XACT_ABORT ON;
BEGIN TRANSACTION;

/* ---------------------------------------------------------------------------
   1) Person (shared identity aggregate)
   --------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Person', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Person
    (
        Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Person PRIMARY KEY,
        FirstName           NVARCHAR(500)  NULL,
        LastName            NVARCHAR(500)  NULL,
        MobileNumber        NVARCHAR(50)   NULL,
        CardNo              NVARCHAR(50)   NULL,
        BirthYear           INT            NULL,
        CreatedOnUtc        DATETIME2      NOT NULL,
        CreatedByCustomerId INT            NULL
    );

    ALTER TABLE dbo.Person ADD CONSTRAINT FK_Person_Customer_CreatedByCustomerId
        FOREIGN KEY (CreatedByCustomerId) REFERENCES dbo.Customer(Id);
END;

/* ---------------------------------------------------------------------------
   2) DisciplinaryForm (created empty; matches DisciplinaryForm entity)
   --------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.DisciplinaryForm', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DisciplinaryForm
    (
        Id                                                     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DisciplinaryForm PRIMARY KEY,
        PersonId                                               INT            NOT NULL,
        Age                                                    INT            NULL,
        IsMarried                                              BIT            NULL,
        IsEmployed                                             BIT            NULL,
        EducationLevel                                         INT            NULL,
        AgencyId                                               INT            NULL,
        AgencyName                                             NVARCHAR(500)  NULL,
        LegionNo                                               NVARCHAR(50)   NULL,
        PreviousSubstanceUseDetails                            NVARCHAR(MAX)  NULL,
        CurrentSubstanceUseDetails                             NVARCHAR(MAX)  NULL,
        AbsenceDuration                                        INT            NOT NULL,
        RelapseDuration                                        INT            NOT NULL,
        ReferralReasons                                        INT            NOT NULL,
        ReferralReasonOtherDetails                             NVARCHAR(MAX)  NULL,
        IsCurrentSubstanceUseBelowHalfGram                     BIT            NULL,
        HasSubstanceUseInAnotherBranch                         BIT            NULL,
        HasCigaretteUse                                        BIT            NULL,
        CigaretteTreatmentStatus                               INT            NOT NULL,
        HeightCm                                               INT            NULL,
        WeightKg                                               DECIMAL(18,4)  NULL,
        HasOverOrUnderWeight                                   BIT            NULL,
        HadHealthyDietInPastYear                               BIT            NULL,
        RelapsedDueToWeightIssue                               BIT            NULL,
        HadHealthyWeightBeforeRecovery                         BIT            NULL,
        EducationalResources                                   INT            NOT NULL,
        WroteOneCdPerWeek                                      BIT            NULL,
        CompletedThirtyCdExam                                  BIT            NULL,
        CompletedFortyCdExam                                   BIT            NULL,
        AttendedParksDuringFirstSixMonths                      BIT            NULL,
        ParticipatedInAtLeastOneSport                          BIT            NULL,
        ParticipatedInSportsActivitiesOrCompetitions           BIT            NULL,
        HasRegularWorkshopAttendance                           BIT            NULL,
        HasRegularLegionAttendance                             BIT            NULL,
        HadWeeklyParticipationAndTravelDeclarationInFirstTrip  BIT            NULL,
        TookSecondTripExams                                    BIT            NULL,
        ServiceRoles                                           INT            NOT NULL,
        FamilyRelapseFactors                                   INT            NOT NULL,
        FamilyRelapseFactorOtherDetails                        NVARCHAR(MAX)  NULL,
        MedicalConditionAndMedicationNotes                     NVARCHAR(MAX)  NULL,
        CreatedOnUtc                                           DATETIME2      NOT NULL,
        CreatedByCustomerId                                    INT            NULL
    );

    ALTER TABLE dbo.DisciplinaryForm ADD CONSTRAINT FK_DisciplinaryForm_Person_PersonId
        FOREIGN KEY (PersonId) REFERENCES dbo.Person(Id);
    ALTER TABLE dbo.DisciplinaryForm ADD CONSTRAINT FK_DisciplinaryForm_Agency_AgencyId
        FOREIGN KEY (AgencyId) REFERENCES dbo.Agency(Id);
    ALTER TABLE dbo.DisciplinaryForm ADD CONSTRAINT FK_DisciplinaryForm_Customer_CreatedByCustomerId
        FOREIGN KEY (CreatedByCustomerId) REFERENCES dbo.Customer(Id);

    CREATE INDEX IX_DisciplinaryForm_PersonId     ON dbo.DisciplinaryForm(PersonId);
    CREATE INDEX IX_DisciplinaryForm_AgencyId     ON dbo.DisciplinaryForm(AgencyId);
    CREATE INDEX IX_DisciplinaryForm_CreatedOnUtc ON dbo.DisciplinaryForm(CreatedOnUtc DESC);
END;

/* ---------------------------------------------------------------------------
   3) Rename Passenger -> RecoveryForm
   --------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Passenger', N'U') IS NOT NULL AND OBJECT_ID(N'dbo.RecoveryForm', N'U') IS NULL
    EXEC sys.sp_rename N'dbo.Passenger', N'RecoveryForm';

/* ---------------------------------------------------------------------------
   4) Add the (temporarily nullable) PersonId link on RecoveryForm
   --------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.RecoveryForm', N'U') IS NOT NULL AND COL_LENGTH('dbo.RecoveryForm', 'PersonId') IS NULL
    ALTER TABLE dbo.RecoveryForm ADD PersonId INT NULL;

COMMIT TRANSACTION;
