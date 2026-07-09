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
        var personTableName = NameCompatibilityManager.GetTableName(typeof(Person));

        //the disciplinary form references the Person aggregate via a foreign key.
        //on databases that predate the Person table (which is otherwise created later by
        //AddPersonAggregateMigration) we must create it here first, otherwise
        //Create.TableFor<DisciplinaryForm> fails while building the FK to a missing table.
        if (!Schema.Table(personTableName).Exists())
            Create.TableFor<Person>();

        if (Schema.Table(tableName).Exists())
            return;

        Create.TableFor<DisciplinaryForm>();

        var personIdColumn = nameof(DisciplinaryForm.PersonId);
        var personIdIndexName = "IX_DisciplinaryForm_PersonId";

        if (Schema.Table(tableName).Column(personIdColumn).Exists() &&
            !Schema.Table(tableName).Index(personIdIndexName).Exists())
        {
            Create.Index(personIdIndexName)
                .OnTable(tableName)
                .OnColumn(personIdColumn).Ascending();
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
