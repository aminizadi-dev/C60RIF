using System.Data;
using FluentMigrator;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data.Migrations;

namespace Nop.Data.Migrations.Updates;

/// <summary>
/// Migration to add AntiX foreign keys to Passenger
/// </summary>
[NopUpdateMigration("2024/02/06 00:00:00", "5.0.0", UpdateMigrationType.Data)]
public class AddAntiXForeignKeysToPassengerMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Passenger)).Exists())
            return;

        if (!Schema.Table(nameof(AntiX)).Exists())
            return;

        Execute.Sql(@"
IF EXISTS (SELECT 1 FROM [Passenger])
BEGIN
    SET IDENTITY_INSERT [AntiX] ON;

    INSERT INTO [AntiX] ([Id], [Name], [Published], [DisplayOrder])
    SELECT DISTINCT v.IdValue, CONCAT('AntiX ', v.IdValue), 1, 0
    FROM (
        SELECT [AntiX1] AS IdValue FROM [Passenger] WHERE [AntiX1] IS NOT NULL AND [AntiX1] >= 0
        UNION
        SELECT [AntiX2] AS IdValue FROM [Passenger] WHERE [AntiX2] IS NOT NULL AND [AntiX2] >= 0
    ) v
    WHERE NOT EXISTS (SELECT 1 FROM [AntiX] ax WHERE ax.[Id] = v.IdValue);

    SET IDENTITY_INSERT [AntiX] OFF;
END
");

        if (!Schema.Table(nameof(Passenger)).Constraint("FK_Passenger_AntiX1").Exists())
        {
            Create.ForeignKey("FK_Passenger_AntiX1")
                .FromTable(nameof(Passenger)).ForeignColumn(nameof(Passenger.AntiX1))
                .ToTable(nameof(AntiX)).PrimaryColumn(nameof(BaseEntity.Id))
                .OnDelete(Rule.None);
        }

        if (!Schema.Table(nameof(Passenger)).Constraint("FK_Passenger_AntiX2").Exists())
        {
            Create.ForeignKey("FK_Passenger_AntiX2")
                .FromTable(nameof(Passenger)).ForeignColumn(nameof(Passenger.AntiX2))
                .ToTable(nameof(AntiX)).PrimaryColumn(nameof(BaseEntity.Id))
                .OnDelete(Rule.None);
        }
    }
}

