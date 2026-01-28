using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add AgencyId column to Passenger table
/// </summary>
[NopUpdateMigration("2024/02/03 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddAgencyIdToPassengerMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (Schema.Table(nameof(Passenger)).Exists())
        {
            if (!Schema.Table(nameof(Passenger)).Column(nameof(Passenger.AgencyId)).Exists())
            {
                Alter.Table(nameof(Passenger))
                    .AddColumn(nameof(Passenger.AgencyId)).AsInt32().NotNullable().ForeignKey<Agency>();
            }
        }
    }
}

