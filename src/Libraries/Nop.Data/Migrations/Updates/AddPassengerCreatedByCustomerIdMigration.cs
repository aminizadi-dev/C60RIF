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
        //this migration predates the Passenger -> RecoveryForm rename (2026/07/07), so on databases
        //that have not been renamed yet the physical table is still called "Passenger". Resolve
        //whichever name currently exists so it works on both legacy and already-renamed databases.
        var passengerTableName = Schema.Table("RecoveryForm").Exists()
            ? "RecoveryForm"
            : Schema.Table("Passenger").Exists()
                ? "Passenger"
                : null;

        if (passengerTableName == null)
            return;

        var customerTableName = NameCompatibilityManager.GetTableName(typeof(Customer));
        var columnName = nameof(RecoveryForm.CreatedByCustomerId);

        if (!Schema.Table(passengerTableName).Column(columnName).Exists())
        {
            Alter.Table(passengerTableName)
                .AddColumn(columnName).AsInt32().Nullable()
                .ForeignKey(customerTableName, "Id");
        }
    }
}
