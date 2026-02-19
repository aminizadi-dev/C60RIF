using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a passenger entity builder
/// </summary>
public partial class PassengerBuilder : NopEntityBuilder<Passenger>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Passenger.RecoveryNo)).AsInt32().NotNullable()
            .WithColumn(nameof(Passenger.PersonName)).AsString(500).Nullable()
            .WithColumn(nameof(Passenger.GuideNameAndLegionNo)).AsString(500).NotNullable()
            .WithColumn(nameof(Passenger.ClinicId)).AsInt32().NotNullable().ForeignKey<Clinic>()
            .WithColumn(nameof(Passenger.BirthYear)).AsInt32().Nullable()
            .WithColumn(nameof(Passenger.Education)).AsInt32().NotNullable()
            .WithColumn(nameof(Passenger.IsMarried)).AsBoolean().NotNullable()
            .WithColumn(nameof(Passenger.IsEmployed)).AsBoolean().NotNullable()
            .WithColumn(nameof(Passenger.HasCompanion)).AsBoolean().Nullable()
            .WithColumn(nameof(Passenger.CardNo)).AsString(50).Nullable()
            .WithColumn(nameof(Passenger.AntiX1)).AsInt32().NotNullable().ForeignKey<AntiX>()
            .WithColumn(nameof(Passenger.AntiX2)).AsInt32().Nullable().ForeignKey<AntiX>()
            .WithColumn(nameof(Passenger.TravelStartDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(Passenger.TravelEndDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(Passenger.PictureId)).AsInt32().NotNullable()
            .WithColumn(nameof(Passenger.AgencyId)).AsInt32().NotNullable().ForeignKey<Agency>()
            .WithColumn(nameof(Passenger.CreatedOnUtc)).AsDateTime2().NotNullable();
    }

    #endregion
}

