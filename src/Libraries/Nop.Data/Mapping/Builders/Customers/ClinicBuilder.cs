using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a clinic entity builder
/// </summary>
public partial class ClinicBuilder : NopEntityBuilder<Clinic>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Clinic.CityId)).AsInt32().NotNullable().ForeignKey<City>()
            .WithColumn(nameof(Clinic.Name)).AsString(500).NotNullable()
            .WithColumn(nameof(Clinic.Published)).AsBoolean().NotNullable()
            .WithColumn(nameof(Clinic.DisplayOrder)).AsInt32().NotNullable();
    }

    #endregion
}

