using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/07/03 21:00:00", "4.90", UpdateMigrationType.Data)]
public class AddDisciplinaryFormAgencyIdMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(DisciplinaryForm));
        var agencyTableName = NameCompatibilityManager.GetTableName(typeof(Agency));
        var columnName = nameof(DisciplinaryForm.AgencyId);

        if (!Schema.Table(tableName).Exists() || Schema.Table(tableName).Column(columnName).Exists())
            return;

        Alter.Table(tableName)
            .AddColumn(columnName).AsInt32().Nullable()
            .ForeignKey(agencyTableName, "Id");

        const string indexName = "IX_DisciplinaryForm_AgencyId";
        if (!Schema.Table(tableName).Index(indexName).Exists())
        {
            Create.Index(indexName)
                .OnTable(tableName)
                .OnColumn(columnName).Ascending();
        }
    }
}
