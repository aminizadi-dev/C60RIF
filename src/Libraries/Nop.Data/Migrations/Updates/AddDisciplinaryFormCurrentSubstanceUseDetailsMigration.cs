using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/07/03 21:30:00", "4.90", UpdateMigrationType.Data)]
public class AddDisciplinaryFormCurrentSubstanceUseDetailsMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(DisciplinaryForm));
        var columnName = nameof(DisciplinaryForm.CurrentSubstanceUseDetails);

        if (!Schema.Table(tableName).Exists() || Schema.Table(tableName).Column(columnName).Exists())
            return;

        Alter.Table(tableName)
            .AddColumn(columnName).AsString(int.MaxValue).Nullable();
    }
}
