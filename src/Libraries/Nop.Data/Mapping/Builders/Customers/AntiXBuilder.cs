using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents an AntiX entity builder
/// </summary>
public partial class AntiXBuilder : NopEntityBuilder<AntiX>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AntiX.Name)).AsString(500).NotNullable()
            .WithColumn(nameof(AntiX.Published)).AsBoolean().NotNullable()
            .WithColumn(nameof(AntiX.DisplayOrder)).AsInt32().NotNullable();
    }

    #endregion
}

