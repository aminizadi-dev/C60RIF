using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents an agency entity builder
/// </summary>
public partial class AgencyBuilder : NopEntityBuilder<Agency>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Agency.CityId)).AsInt32().NotNullable().ForeignKey<City>()
            .WithColumn(nameof(Agency.Name)).AsString(500).NotNullable()
            .WithColumn(nameof(Agency.Published)).AsBoolean().NotNullable()
            .WithColumn(nameof(Agency.DisplayOrder)).AsInt32().NotNullable();
    }

    #endregion
}

