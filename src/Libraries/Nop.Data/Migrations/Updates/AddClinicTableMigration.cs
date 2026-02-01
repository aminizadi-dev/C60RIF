using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add Clinic table
/// </summary>
[NopUpdateMigration("2026/01/31 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddClinicTableMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Clinic)).Exists())
        {
            Create.TableFor<Clinic>();
        }
    }
}

