using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add Passenger table
/// </summary>
[NopUpdateMigration("2024/01/01 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddPassengerTableMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Passenger)).Exists())
        {
            Create.TableFor<Passenger>();
        }
    }
}

