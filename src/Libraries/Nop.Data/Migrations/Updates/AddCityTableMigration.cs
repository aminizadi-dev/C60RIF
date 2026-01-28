using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add City table
/// </summary>
[NopUpdateMigration("2024/02/01 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddCityTableMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(City)).Exists())
        {
            Create.TableFor<City>();
        }
    }
}

