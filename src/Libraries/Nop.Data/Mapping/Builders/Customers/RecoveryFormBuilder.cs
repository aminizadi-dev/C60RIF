using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a recovery form entity builder
/// </summary>
public partial class RecoveryFormBuilder : NopEntityBuilder<RecoveryForm>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(RecoveryForm.PersonId)).AsInt32().NotNullable().ForeignKey<Person>()
            .WithColumn(nameof(RecoveryForm.RecoveryNo)).AsString(20).NotNullable()
            .WithColumn(nameof(RecoveryForm.GuideNameAndLegionNo)).AsString(500).NotNullable()
            .WithColumn(nameof(RecoveryForm.ClinicId)).AsInt32().Nullable().ForeignKey<Clinic>()
            .WithColumn(nameof(RecoveryForm.Education)).AsInt32().NotNullable()
            .WithColumn(nameof(RecoveryForm.IsMarried)).AsBoolean().Nullable()
            .WithColumn(nameof(RecoveryForm.IsEmployed)).AsBoolean().Nullable()
            .WithColumn(nameof(RecoveryForm.HasCompanion)).AsBoolean().Nullable()
            .WithColumn(nameof(RecoveryForm.AntiX1)).AsInt32().NotNullable().ForeignKey<AntiX>()
            .WithColumn(nameof(RecoveryForm.AntiX2)).AsInt32().Nullable().ForeignKey<AntiX>()
            .WithColumn(nameof(RecoveryForm.TravelStartDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(RecoveryForm.TravelEndDateUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(RecoveryForm.PictureId)).AsInt32().NotNullable()
            .WithColumn(nameof(RecoveryForm.AgencyId)).AsInt32().NotNullable().ForeignKey<Agency>()
            .WithColumn(nameof(RecoveryForm.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(RecoveryForm.CreatedByCustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
    }

    #endregion
}
