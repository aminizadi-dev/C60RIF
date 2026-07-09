using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a person entity builder
/// </summary>
public partial class PersonBuilder : NopEntityBuilder<Person>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Person.FirstName)).AsString(500).Nullable()
            .WithColumn(nameof(Person.LastName)).AsString(500).Nullable()
            .WithColumn(nameof(Person.MobileNumber)).AsString(50).Nullable()
            .WithColumn(nameof(Person.CardNo)).AsString(50).Nullable()
            .WithColumn(nameof(Person.BirthYear)).AsInt32().Nullable()
            .WithColumn(nameof(Person.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(Person.CreatedByCustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
    }

    #endregion
}
