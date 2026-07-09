using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Introduces the Person aggregate: creates the Person table, links Passenger and DisciplinaryForm
/// to a person via PersonId, backfills existing data, and drops the duplicated identity columns.
/// </summary>
[NopUpdateMigration("2026/07/07 10:00:00", "4.90", UpdateMigrationType.Data)]
public class AddPersonAggregateMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var personTable = NameCompatibilityManager.GetTableName(typeof(Person));
        //at the time this migration runs the recovery-form table is still physically named "Passenger"
        //(it is renamed to "RecoveryForm" by a later migration), so reference it by its current name here
        const string passengerTable = "Passenger";
        var disciplinaryTable = NameCompatibilityManager.GetTableName(typeof(DisciplinaryForm));

        //1. create the Person table
        if (!Schema.Table(personTable).Exists())
            Create.TableFor<Person>();

        //2. add the nullable PersonId link columns (they will be backfilled and then made NOT NULL below)
        if (Schema.Table(passengerTable).Exists() &&
            !Schema.Table(passengerTable).Column(nameof(RecoveryForm.PersonId)).Exists())
        {
            Alter.Table(passengerTable)
                .AddColumn(nameof(RecoveryForm.PersonId)).AsInt32().Nullable();
        }

        if (Schema.Table(disciplinaryTable).Exists() &&
            !Schema.Table(disciplinaryTable).Column(nameof(DisciplinaryForm.PersonId)).Exists())
        {
            Alter.Table(disciplinaryTable)
                .AddColumn(nameof(DisciplinaryForm.PersonId)).AsInt32().Nullable();
        }

        //3. backfill data, enforce constraints and drop the moved identity columns (SQL Server only)
        IfDatabase("SqlServer").Execute.Sql(BackfillAndCleanupSql);
    }

    /// <summary>
    /// T-SQL that migrates existing data into the Person aggregate and removes the duplicated columns.
    /// Guarded with column-existence checks so it is safe on both fresh and existing databases.
    /// </summary>
    private const string BackfillAndCleanupSql = @"
SET NOCOUNT ON;

-- Backfill a Person for every existing recovery form (Passenger).
-- NOTE: these statements reference the legacy identity columns (PersonName, CardNo, BirthYear,
-- PassengerId, FamilyName) which only exist on databases that predate the Person refactor.
-- They MUST run through dynamic SQL: SQL Server binds column names at compile time for existing
-- tables (deferred name resolution applies to missing tables, not missing columns), so a plain
-- guarded block still fails to compile when a table exists with the new schema (no legacy columns).
IF COL_LENGTH('Passenger','PersonId') IS NOT NULL AND COL_LENGTH('Passenger','PersonName') IS NOT NULL
    EXEC sys.sp_executesql N'
        DECLARE @passengerMap TABLE (PassengerId INT, PersonId INT);

        MERGE INTO Person AS tgt
        USING (SELECT Id, PersonName, CardNo, BirthYear, CreatedOnUtc, CreatedByCustomerId
               FROM Passenger WHERE PersonId IS NULL) AS src
        ON 1 = 0
        WHEN NOT MATCHED THEN
            INSERT (FirstName, LastName, MobileNumber, CardNo, BirthYear, CreatedOnUtc, CreatedByCustomerId)
            VALUES (src.PersonName, NULL, NULL, src.CardNo, src.BirthYear, src.CreatedOnUtc, src.CreatedByCustomerId)
        OUTPUT src.Id, inserted.Id INTO @passengerMap (PassengerId, PersonId);

        UPDATE p SET p.PersonId = m.PersonId
        FROM Passenger p JOIN @passengerMap m ON p.Id = m.PassengerId;';

-- Link disciplinary forms that already reference a passenger to that passenger's person
IF COL_LENGTH('DisciplinaryForm','PersonId') IS NOT NULL AND COL_LENGTH('DisciplinaryForm','PassengerId') IS NOT NULL
    EXEC sys.sp_executesql N'
        UPDATE d SET d.PersonId = p.PersonId
        FROM DisciplinaryForm d JOIN Passenger p ON d.PassengerId = p.Id
        WHERE d.PersonId IS NULL AND d.PassengerId IS NOT NULL AND p.PersonId IS NOT NULL;';

-- Create a standalone Person for the remaining disciplinary forms (no auto dedup)
IF COL_LENGTH('DisciplinaryForm','PersonId') IS NOT NULL AND COL_LENGTH('DisciplinaryForm','PersonName') IS NOT NULL
    EXEC sys.sp_executesql N'
        DECLARE @formMap TABLE (FormId INT, PersonId INT);

        MERGE INTO Person AS tgt
        USING (SELECT Id, PersonName, FamilyName, CardNo, CreatedOnUtc, CreatedByCustomerId
               FROM DisciplinaryForm WHERE PersonId IS NULL) AS src
        ON 1 = 0
        WHEN NOT MATCHED THEN
            INSERT (FirstName, LastName, MobileNumber, CardNo, BirthYear, CreatedOnUtc, CreatedByCustomerId)
            VALUES (src.PersonName, src.FamilyName, NULL, src.CardNo, NULL, src.CreatedOnUtc, src.CreatedByCustomerId)
        OUTPUT src.Id, inserted.Id INTO @formMap (FormId, PersonId);

        UPDATE d SET d.PersonId = m.PersonId
        FROM DisciplinaryForm d JOIN @formMap m ON d.Id = m.FormId;';

-- Drop the moved identity columns (with their dependent FKs, indexes and default constraints)
DECLARE @dropTargets TABLE (TableName SYSNAME, ColumnName SYSNAME);
INSERT INTO @dropTargets (TableName, ColumnName) VALUES
    ('DisciplinaryForm', 'PassengerId'),
    ('DisciplinaryForm', 'PersonName'),
    ('DisciplinaryForm', 'FamilyName'),
    ('DisciplinaryForm', 'CardNo'),
    ('Passenger', 'PersonName'),
    ('Passenger', 'CardNo'),
    ('Passenger', 'BirthYear');

DECLARE @t SYSNAME, @c SYSNAME, @sql NVARCHAR(MAX);
DECLARE dropCursor CURSOR LOCAL FAST_FORWARD FOR SELECT TableName, ColumnName FROM @dropTargets;
OPEN dropCursor;
FETCH NEXT FROM dropCursor INTO @t, @c;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF COL_LENGTH(@t, @c) IS NOT NULL
    BEGIN
        -- drop foreign keys defined on the column
        SET @sql = N'';
        SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(@t) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
        FROM sys.foreign_keys fk
        JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
        JOIN sys.columns col ON col.object_id = fkc.parent_object_id AND col.column_id = fkc.parent_column_id
        WHERE fk.parent_object_id = OBJECT_ID(@t) AND col.name = @c;
        IF LEN(@sql) > 0 EXEC sys.sp_executesql @sql;

        -- drop indexes referencing the column
        SET @sql = N'';
        SELECT @sql = @sql + N'DROP INDEX ' + QUOTENAME(i.name) + N' ON ' + QUOTENAME(@t) + N';'
        FROM sys.indexes i
        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
        JOIN sys.columns col ON col.object_id = ic.object_id AND col.column_id = ic.column_id
        WHERE i.object_id = OBJECT_ID(@t) AND col.name = @c AND i.is_primary_key = 0 AND i.type > 0;
        IF LEN(@sql) > 0 EXEC sys.sp_executesql @sql;

        -- drop default constraints on the column
        SET @sql = N'';
        SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(@t) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name) + N';'
        FROM sys.default_constraints dc
        JOIN sys.columns col ON col.object_id = dc.parent_object_id AND col.column_id = dc.parent_column_id
        WHERE dc.parent_object_id = OBJECT_ID(@t) AND col.name = @c;
        IF LEN(@sql) > 0 EXEC sys.sp_executesql @sql;

        -- drop the column itself
        SET @sql = N'ALTER TABLE ' + QUOTENAME(@t) + N' DROP COLUMN ' + QUOTENAME(@c) + N';';
        EXEC sys.sp_executesql @sql;
    END
    FETCH NEXT FROM dropCursor INTO @t, @c;
END
CLOSE dropCursor;
DEALLOCATE dropCursor;

-- Enforce NOT NULL, foreign key and index on Passenger.PersonId
IF COL_LENGTH('Passenger','PersonId') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Passenger') AND name = 'PersonId' AND is_nullable = 1)
        ALTER TABLE Passenger ALTER COLUMN PersonId INT NOT NULL;

    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys fk
                   JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                   JOIN sys.columns c ON c.object_id = fkc.parent_object_id AND c.column_id = fkc.parent_column_id
                   WHERE fk.parent_object_id = OBJECT_ID('Passenger') AND c.name = 'PersonId')
        ALTER TABLE Passenger ADD CONSTRAINT FK_Passenger_Person_PersonId FOREIGN KEY (PersonId) REFERENCES Person(Id);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Passenger') AND name = 'IX_Passenger_PersonId')
        CREATE INDEX IX_Passenger_PersonId ON Passenger(PersonId);
END

-- Enforce NOT NULL, foreign key and index on DisciplinaryForm.PersonId
IF COL_LENGTH('DisciplinaryForm','PersonId') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('DisciplinaryForm') AND name = 'PersonId' AND is_nullable = 1)
        ALTER TABLE DisciplinaryForm ALTER COLUMN PersonId INT NOT NULL;

    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys fk
                   JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                   JOIN sys.columns c ON c.object_id = fkc.parent_object_id AND c.column_id = fkc.parent_column_id
                   WHERE fk.parent_object_id = OBJECT_ID('DisciplinaryForm') AND c.name = 'PersonId')
        ALTER TABLE DisciplinaryForm ADD CONSTRAINT FK_DisciplinaryForm_Person_PersonId FOREIGN KEY (PersonId) REFERENCES Person(Id);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('DisciplinaryForm') AND name = 'IX_DisciplinaryForm_PersonId')
        CREATE INDEX IX_DisciplinaryForm_PersonId ON DisciplinaryForm(PersonId);
END
";
}
