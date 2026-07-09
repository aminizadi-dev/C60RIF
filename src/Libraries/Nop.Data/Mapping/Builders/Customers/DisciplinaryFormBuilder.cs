using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a disciplinary form entity builder
/// </summary>
public partial class DisciplinaryFormBuilder : NopEntityBuilder<DisciplinaryForm>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(DisciplinaryForm.PersonId)).AsInt32().NotNullable().ForeignKey<Person>()
            .WithColumn(nameof(DisciplinaryForm.Age)).AsInt32().Nullable()
            .WithColumn(nameof(DisciplinaryForm.IsMarried)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.IsEmployed)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.EducationLevel)).AsInt32().Nullable()
            .WithColumn(nameof(DisciplinaryForm.AgencyId)).AsInt32().Nullable().ForeignKey<Agency>()
            .WithColumn(nameof(DisciplinaryForm.AgencyName)).AsString(500).Nullable()
            .WithColumn(nameof(DisciplinaryForm.LegionNo)).AsString(50).Nullable()
            .WithColumn(nameof(DisciplinaryForm.PreviousSubstanceUseDetails)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(DisciplinaryForm.CurrentSubstanceUseDetails)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(DisciplinaryForm.AbsenceDuration)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.RelapseDuration)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.ReferralReasons)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.ReferralReasonOtherDetails)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(DisciplinaryForm.IsCurrentSubstanceUseBelowHalfGram)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HasSubstanceUseInAnotherBranch)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HasCigaretteUse)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.CigaretteTreatmentStatus)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.HeightCm)).AsInt32().Nullable()
            .WithColumn(nameof(DisciplinaryForm.WeightKg)).AsDecimal(18, 4).Nullable()
            .WithColumn(nameof(DisciplinaryForm.HasOverOrUnderWeight)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HadHealthyDietInPastYear)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.RelapsedDueToWeightIssue)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HadHealthyWeightBeforeRecovery)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.EducationalResources)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.WroteOneCdPerWeek)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.CompletedThirtyCdExam)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.CompletedFortyCdExam)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.AttendedParksDuringFirstSixMonths)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.ParticipatedInAtLeastOneSport)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.ParticipatedInSportsActivitiesOrCompetitions)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HasRegularWorkshopAttendance)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HasRegularLegionAttendance)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.HadWeeklyParticipationAndTravelDeclarationInFirstTrip)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.TookSecondTripExams)).AsBoolean().Nullable()
            .WithColumn(nameof(DisciplinaryForm.ServiceRoles)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.FamilyRelapseFactors)).AsInt32().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.FamilyRelapseFactorOtherDetails)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(DisciplinaryForm.MedicalConditionAndMedicationNotes)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(DisciplinaryForm.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(DisciplinaryForm.CreatedByCustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
    }

    #endregion
}
