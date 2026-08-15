using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a Wiliam form entity builder.
/// </summary>
public partial class WiliamFormBuilder : NopEntityBuilder<WiliamForm>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration.
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WiliamForm.PersonId)).AsInt32().NotNullable().ForeignKey<Person>()
            .WithColumn(nameof(WiliamForm.AntiXId)).AsInt32().NotNullable().ForeignKey<AntiX>()
            .WithColumn(nameof(WiliamForm.SubstanceGuideName)).AsString(500).NotNullable()
            .WithColumn(nameof(WiliamForm.SubstanceReleaseDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(WiliamForm.CigaretteQuitDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(WiliamForm.TravelDuration)).AsString(4).Nullable()
            .WithColumn(nameof(WiliamForm.CigaretteGuideName)).AsString(500).NotNullable()
            .WithColumn(nameof(WiliamForm.CigaretteReleaseDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(WiliamForm.AgencyId)).AsInt32().NotNullable().ForeignKey<Agency>()
            .WithColumn(nameof(WiliamForm.Phone)).AsString(50).Nullable()
            .WithColumn(nameof(WiliamForm.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(WiliamForm.CreatedByCustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
    }

    #endregion
}
