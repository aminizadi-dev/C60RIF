/* =============================================================================
   STEP 2 - DATA MIGRATION + FINALIZE
   For every existing RecoveryForm (the old Passenger rows) creates a matching
   Person and links it, enforces the PersonId constraint/FK/index, then drops
   the legacy identity columns that were moved to Person.

   Run AFTER 01_schema.sql. Safe to re-run (idempotent).
   ============================================================================= */
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN TRANSACTION;

/* ---------------------------------------------------------------------------
   1) Create one Person per not-yet-linked RecoveryForm and set PersonId.
      The full legacy name goes into Person.FirstName (as the original design did).
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.RecoveryForm', 'PersonId') IS NOT NULL
   AND COL_LENGTH('dbo.RecoveryForm', 'PersonName') IS NOT NULL
BEGIN
    DECLARE @map TABLE (RecoveryFormId INT, PersonId INT);

    MERGE INTO dbo.Person AS tgt
    USING (SELECT Id, PersonName, CardNo, BirthYear, CreatedOnUtc, CreatedByCustomerId
           FROM dbo.RecoveryForm WHERE PersonId IS NULL) AS src
    ON 1 = 0
    WHEN NOT MATCHED THEN
        INSERT (FirstName, LastName, MobileNumber, CardNo, BirthYear, CreatedOnUtc, CreatedByCustomerId)
        VALUES (src.PersonName, NULL, NULL, src.CardNo, src.BirthYear, src.CreatedOnUtc, src.CreatedByCustomerId)
    OUTPUT src.Id, inserted.Id INTO @map (RecoveryFormId, PersonId);

    UPDATE r SET r.PersonId = m.PersonId
    FROM dbo.RecoveryForm r JOIN @map m ON r.Id = m.RecoveryFormId;
END;

/* ---------------------------------------------------------------------------
   2) Make RecoveryForm.PersonId NOT NULL now that every row is linked
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.RecoveryForm', 'PersonId') IS NOT NULL
   AND EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.RecoveryForm') AND name = 'PersonId' AND is_nullable = 1)
    ALTER TABLE dbo.RecoveryForm ALTER COLUMN PersonId INT NOT NULL;

/* ---------------------------------------------------------------------------
   3) FK + index on RecoveryForm.PersonId
   --------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RecoveryForm_Person_PersonId')
    ALTER TABLE dbo.RecoveryForm ADD CONSTRAINT FK_RecoveryForm_Person_PersonId
        FOREIGN KEY (PersonId) REFERENCES dbo.Person(Id);

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID('dbo.RecoveryForm') AND name = 'IX_RecoveryForm_PersonId')
    CREATE INDEX IX_RecoveryForm_PersonId ON dbo.RecoveryForm(PersonId);

/* ---------------------------------------------------------------------------
   4) Drop the moved identity columns (PersonName, CardNo, BirthYear) from
      RecoveryForm, together with any dependent default constraints / indexes.
   --------------------------------------------------------------------------- */
DECLARE @drop TABLE (ColumnName SYSNAME);
INSERT INTO @drop (ColumnName) VALUES (N'PersonName'), (N'CardNo'), (N'BirthYear');

DECLARE @c SYSNAME, @sql NVARCHAR(MAX);
DECLARE dropCur CURSOR LOCAL FAST_FORWARD FOR SELECT ColumnName FROM @drop;
OPEN dropCur;
FETCH NEXT FROM dropCur INTO @c;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF COL_LENGTH('dbo.RecoveryForm', @c) IS NOT NULL
    BEGIN
        -- default constraints
        SET @sql = N'';
        SELECT @sql = @sql + N'ALTER TABLE dbo.RecoveryForm DROP CONSTRAINT ' + QUOTENAME(dc.name) + N';'
        FROM sys.default_constraints dc
        JOIN sys.columns col ON col.object_id = dc.parent_object_id AND col.column_id = dc.parent_column_id
        WHERE dc.parent_object_id = OBJECT_ID('dbo.RecoveryForm') AND col.name = @c;
        IF LEN(@sql) > 0 EXEC sys.sp_executesql @sql;

        -- non-clustered indexes referencing the column
        SET @sql = N'';
        SELECT @sql = @sql + N'DROP INDEX ' + QUOTENAME(i.name) + N' ON dbo.RecoveryForm;'
        FROM sys.indexes i
        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
        JOIN sys.columns col ON col.object_id = ic.object_id AND col.column_id = ic.column_id
        WHERE i.object_id = OBJECT_ID('dbo.RecoveryForm') AND col.name = @c
              AND i.is_primary_key = 0 AND i.type > 0;
        IF LEN(@sql) > 0 EXEC sys.sp_executesql @sql;

        -- the column itself
        SET @sql = N'ALTER TABLE dbo.RecoveryForm DROP COLUMN ' + QUOTENAME(@c) + N';';
        EXEC sys.sp_executesql @sql;
    END
    FETCH NEXT FROM dropCur INTO @c;
END
CLOSE dropCur;
DEALLOCATE dropCur;

COMMIT TRANSACTION;

/* Quick verification */
SELECT (SELECT COUNT(*) FROM dbo.RecoveryForm) AS RecoveryForms,
       (SELECT COUNT(*) FROM dbo.Person)       AS Persons,
       (SELECT COUNT(*) FROM dbo.RecoveryForm WHERE PersonId IS NULL) AS UnlinkedRecoveryForms;
