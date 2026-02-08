using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add HasCompanion column to Passenger table
/// </summary>
[NopUpdateMigration("2026/02/08 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddHasCompanionToPassengerMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Passenger)).Exists())
            return;

        if (!Schema.Table(nameof(Passenger)).Column(nameof(Passenger.HasCompanion)).Exists())
        {
            Alter.Table(nameof(Passenger))
                .AddColumn(nameof(Passenger.HasCompanion)).AsBoolean().Nullable();
        }
    }
}
