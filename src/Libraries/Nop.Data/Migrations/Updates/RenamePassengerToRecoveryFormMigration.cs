using FluentMigrator;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Renames the Passenger table to RecoveryForm (semantic alignment with DisciplinaryForm / WilliamForm)
/// and renames Person.NationalCardNo to CardNo for naming consistency.
/// </summary>
[NopUpdateMigration("2026/07/07 11:00:00", "4.90", UpdateMigrationType.Data)]
public class RenamePassengerToRecoveryFormMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        IfDatabase("SqlServer").Execute.Sql(RenameSql);
    }

    private const string RenameSql = @"
SET NOCOUNT ON;

-- 1. Rename the recovery-form table
IF OBJECT_ID(N'Passenger', N'U') IS NOT NULL AND OBJECT_ID(N'RecoveryForm', N'U') IS NULL
    EXEC sys.sp_rename N'Passenger', N'RecoveryForm';

-- 2. Rename FK constraints that still carry the old ""Passenger"" prefix
IF OBJECT_ID(N'RecoveryForm', N'U') IS NOT NULL
BEGIN
    DECLARE @fk SYSNAME, @fkSql NVARCHAR(MAX);
    DECLARE fkCursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT name FROM sys.foreign_keys
        WHERE parent_object_id = OBJECT_ID(N'RecoveryForm') AND name LIKE N'FK_Passenger%';
    OPEN fkCursor;
    FETCH NEXT FROM fkCursor INTO @fk;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @fkSql = N'EXEC sys.sp_rename N''' + REPLACE(@fk, '''', '''''') + N''', N''' +
            REPLACE(REPLACE(@fk, N'FK_Passenger', N'FK_RecoveryForm'), '''', '''''') + N''', N''OBJECT'';';
        EXEC sys.sp_executesql @fkSql;
        FETCH NEXT FROM fkCursor INTO @fk;
    END
    CLOSE fkCursor;
    DEALLOCATE fkCursor;

    --rename indexes that still carry the old ""Passenger"" prefix
    DECLARE @idx SYSNAME, @idxSql NVARCHAR(MAX);
    DECLARE idxCursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT name FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'RecoveryForm') AND name LIKE N'IX_Passenger%';
    OPEN idxCursor;
    FETCH NEXT FROM idxCursor INTO @idx;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @idxSql = N'EXEC sys.sp_rename N''RecoveryForm.' + REPLACE(@idx, '''', '''''') + N''', N''' +
            REPLACE(REPLACE(@idx, N'IX_Passenger', N'IX_RecoveryForm'), '''', '''''') + N''', N''INDEX'';';
        EXEC sys.sp_executesql @idxSql;
        FETCH NEXT FROM idxCursor INTO @idx;
    END
    CLOSE idxCursor;
    DEALLOCATE idxCursor;
END

-- 3. Rename Person.NationalCardNo -> CardNo (column was created by AddPersonAggregateMigration)
IF COL_LENGTH(N'Person', N'NationalCardNo') IS NOT NULL AND COL_LENGTH(N'Person', N'CardNo') IS NULL
    EXEC sys.sp_rename N'Person.NationalCardNo', N'CardNo', N'COLUMN';
";
}
