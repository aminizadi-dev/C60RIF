using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/06/28 12:00:00", "4.90", UpdateMigrationType.Data)]
public class AddDisciplinaryFormTableMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(DisciplinaryForm));

        if (Schema.Table(tableName).Exists())
            return;

        Create.TableFor<DisciplinaryForm>();

        var passengerIdColumn = nameof(DisciplinaryForm.PassengerId);
        var passengerIdIndexName = "IX_DisciplinaryForm_PassengerId";

        if (Schema.Table(tableName).Column(passengerIdColumn).Exists() &&
            !Schema.Table(tableName).Index(passengerIdIndexName).Exists())
        {
            Create.Index(passengerIdIndexName)
                .OnTable(tableName)
                .OnColumn(passengerIdColumn).Ascending()
                .WithOptions().Unique();
        }

        var cardNoIndexName = "IX_DisciplinaryForm_CardNo";
        if (Schema.Table(tableName).Column(nameof(DisciplinaryForm.CardNo)).Exists() &&
            !Schema.Table(tableName).Index(cardNoIndexName).Exists())
        {
            Create.Index(cardNoIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(DisciplinaryForm.CardNo)).Ascending();
        }

        var createdOnIndexName = "IX_DisciplinaryForm_CreatedOnUtc";
        if (Schema.Table(tableName).Column(nameof(DisciplinaryForm.CreatedOnUtc)).Exists() &&
            !Schema.Table(tableName).Index(createdOnIndexName).Exists())
        {
            Create.Index(createdOnIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(DisciplinaryForm.CreatedOnUtc)).Descending();
        }

        var agencyIdIndexName = "IX_DisciplinaryForm_AgencyId";
        if (Schema.Table(tableName).Column(nameof(DisciplinaryForm.AgencyId)).Exists() &&
            !Schema.Table(tableName).Index(agencyIdIndexName).Exists())
        {
            Create.Index(agencyIdIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(DisciplinaryForm.AgencyId)).Ascending();
        }
    }
}
