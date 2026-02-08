using Nop.Core.Domain.Customers;
using Nop.Data.Migrations;
using FluentMigrator;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/02/03 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class UpdatePassengerMaritalEmploymentToBoolMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Passenger)).Exists())
            return;

        var table = Schema.Table(nameof(Passenger));

        if (!table.Column(nameof(Passenger.IsMarried)).Exists())
        {
            Alter.Table(nameof(Passenger))
                .AddColumn(nameof(Passenger.IsMarried)).AsBoolean().NotNullable().WithDefaultValue(false);
        }

        if (!table.Column(nameof(Passenger.IsEmployed)).Exists())
        {
            Alter.Table(nameof(Passenger))
                .AddColumn(nameof(Passenger.IsEmployed)).AsBoolean().NotNullable().WithDefaultValue(false);
        }

        var hasMaritalStatus = table.Column("MaritalStatus").Exists();
        if (hasMaritalStatus)
        {
            Execute.Sql($"UPDATE [{nameof(Passenger)}] SET [{nameof(Passenger.IsMarried)}] = CASE WHEN [MaritalStatus] = 20 THEN 1 ELSE 0 END");
            Delete.Column("MaritalStatus").FromTable(nameof(Passenger));
        }

        var hasEmploymentStatus = table.Column("EmploymentStatus").Exists();
        if (hasEmploymentStatus)
        {
            Execute.Sql($"UPDATE [{nameof(Passenger)}] SET [{nameof(Passenger.IsEmployed)}] = CASE WHEN [EmploymentStatus] = 10 THEN 1 ELSE 0 END");
            Delete.Column("EmploymentStatus").FromTable(nameof(Passenger));
        }
    }
}

