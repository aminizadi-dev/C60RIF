using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add Agency table
/// </summary>
[NopUpdateMigration("2024/02/02 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddAgencyTableMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Agency)).Exists())
        {
            Create.TableFor<Agency>();
        }
    }
}

