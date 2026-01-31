using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to make Passenger.AntiX2 nullable
/// </summary>
[NopUpdateMigration("2026/01/31 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class UpdatePassengerAntiX2NullableMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Passenger)).Exists())
            return;

        if (Schema.Table(nameof(Passenger)).Column(nameof(Passenger.AntiX2)).Exists())
        {
            Alter.Table(nameof(Passenger))
                .AlterColumn(nameof(Passenger.AntiX2)).AsInt32().Nullable();

            Execute.Sql($"UPDATE [{nameof(Passenger)}] SET [{nameof(Passenger.AntiX2)}] = NULL WHERE [{nameof(Passenger.AntiX2)}] = 0");
        }
    }
}

