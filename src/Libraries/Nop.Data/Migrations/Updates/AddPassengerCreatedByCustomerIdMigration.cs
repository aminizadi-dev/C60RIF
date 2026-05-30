using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

[NopUpdateMigration("2026/05/23 10:00:00", "4.90", UpdateMigrationType.Data)]
public class AddPassengerCreatedByCustomerIdMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        var passengerTableName = NameCompatibilityManager.GetTableName(typeof(Passenger));
        var customerTableName = NameCompatibilityManager.GetTableName(typeof(Customer));
        var columnName = nameof(Passenger.CreatedByCustomerId);

        if (!Schema.Table(passengerTableName).Column(columnName).Exists())
        {
            Alter.Table(passengerTableName)
                .AddColumn(columnName).AsInt32().Nullable()
                .ForeignKey(customerTableName, "Id");
        }
    }
}
