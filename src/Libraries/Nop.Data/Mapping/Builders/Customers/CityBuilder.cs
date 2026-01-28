using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a city entity builder
/// </summary>
public partial class CityBuilder : NopEntityBuilder<City>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(City.Name)).AsString(500).NotNullable()
            .WithColumn(nameof(City.Published)).AsBoolean().NotNullable()
            .WithColumn(nameof(City.DisplayOrder)).AsInt32().NotNullable();
    }

    #endregion
}

