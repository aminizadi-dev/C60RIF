using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/07/09 18:00:00", "4.90", UpdateMigrationType.Data)]
public class AddWiliamFormTableMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(WiliamForm));
        var personTableName = NameCompatibilityManager.GetTableName(typeof(Person));

        // The form references the Person aggregate via FK. On older databases that
        // still do not have Person, create it first to avoid FK creation failures.
        if (!Schema.Table(personTableName).Exists())
            Create.TableFor<Person>();

        if (Schema.Table(tableName).Exists())
            return;

        Create.TableFor<WiliamForm>();

        var personIdIndexName = "IX_WiliamForm_PersonId";
        if (Schema.Table(tableName).Column(nameof(WiliamForm.PersonId)).Exists() &&
            !Schema.Table(tableName).Index(personIdIndexName).Exists())
        {
            Create.Index(personIdIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(WiliamForm.PersonId)).Ascending();
        }

        var antiXIdIndexName = "IX_WiliamForm_AntiXId";
        if (Schema.Table(tableName).Column(nameof(WiliamForm.AntiXId)).Exists() &&
            !Schema.Table(tableName).Index(antiXIdIndexName).Exists())
        {
            Create.Index(antiXIdIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(WiliamForm.AntiXId)).Ascending();
        }

        var agencyIdIndexName = "IX_WiliamForm_AgencyId";
        if (Schema.Table(tableName).Column(nameof(WiliamForm.AgencyId)).Exists() &&
            !Schema.Table(tableName).Index(agencyIdIndexName).Exists())
        {
            Create.Index(agencyIdIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(WiliamForm.AgencyId)).Ascending();
        }

        var createdOnIndexName = "IX_WiliamForm_CreatedOnUtc";
        if (Schema.Table(tableName).Column(nameof(WiliamForm.CreatedOnUtc)).Exists() &&
            !Schema.Table(tableName).Index(createdOnIndexName).Exists())
        {
            Create.Index(createdOnIndexName)
                .OnTable(tableName)
                .OnColumn(nameof(WiliamForm.CreatedOnUtc)).Descending();
        }
    }
}
