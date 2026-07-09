/* =============================================================================
   STEP 3 - REGISTER MIGRATIONS AS APPLIED
   Because the schema/data were applied manually, we insert the version rows so
   FluentMigrator (on app startup) treats these migrations as already applied and
   never re-runs them. The Version values are the .NET Ticks of each migration's
   date + 5 (UpdateMigrationType.Data), exactly as the app computes them.

   Run AFTER 01_schema.sql and 02_data.sql, before/without another deploy needed.
   Safe to re-run (only inserts missing rows).
   ============================================================================= */
INSERT INTO dbo.MigrationVersionInfo (Version, AppliedOn, Description)
SELECT v.Version, GETUTCDATE(), v.Descr
FROM (VALUES
    (CAST(639151272000000005 AS BIGINT), N'AddPassengerCreatedByCustomerId (applied manually)'),
    (CAST(639182448000000005 AS BIGINT), N'AddDisciplinaryFormTable (applied manually)'),
    (CAST(639187092000000005 AS BIGINT), N'AddDisciplinaryFormAgencyId (applied manually)'),
    (CAST(639187110000000005 AS BIGINT), N'AddDisciplinaryFormCurrentSubstanceUseDetails (applied manually)'),
    (CAST(639190152000000005 AS BIGINT), N'AddPersonAggregate (applied manually)'),
    (CAST(639190188000000005 AS BIGINT), N'RenamePassengerToRecoveryForm (applied manually)')
) AS v(Version, Descr)
WHERE NOT EXISTS (SELECT 1 FROM dbo.MigrationVersionInfo m WHERE m.Version = v.Version);

/* Show the registered rows */
SELECT Version, AppliedOn, Description
FROM dbo.MigrationVersionInfo
WHERE Version IN (639151272000000005, 639182448000000005, 639187092000000005,
                  639187110000000005, 639190152000000005, 639190188000000005)
ORDER BY Version;
