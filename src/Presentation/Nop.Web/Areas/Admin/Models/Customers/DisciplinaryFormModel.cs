using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a disciplinary form model
/// </summary>
public partial record DisciplinaryFormModel : BaseNopEntityModel
{
    public DisciplinaryFormModel()
    {
        AvailableEducationLevels = new List<SelectListItem>();
        AvailableAgencies = new List<SelectListItem>();
    }

    #region Person link

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.PassengerId")]
    public int? RecoveryFormId { get; set; }

    public int PersonId { get; set; }

    #endregion

    #region Personal info

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.PersonName")]
    public string PersonName { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.MobileNumber")]
    public string MobileNumber { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.FamilyName")]
    public string FamilyName { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.CardNo")]
    public string CardNo { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.Age")]
    public int? Age { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.IsMarried")]
    public bool? IsMarried { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.IsEmployed")]
    public bool? IsEmployed { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.EducationLevel")]
    public int? EducationLevel { get; set; }

    public IList<SelectListItem> AvailableEducationLevels { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.Agency")]
    public int? AgencyId { get; set; }

    public IList<SelectListItem> AvailableAgencies { get; set; }

    public string AgencyName { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.LegionNo")]
    public string LegionNo { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.PreviousSubstanceUseDetails")]
    public string PreviousSubstanceUseDetails { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.CurrentSubstanceUseDetails")]
    public string CurrentSubstanceUseDetails { get; set; }

    #endregion

    #region Disciplinary referral reasons

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.AbsenceDuration")]
    public int AbsenceDuration { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.RelapseDuration")]
    public int RelapseDuration { get; set; }

    public bool ReferralReasonRetravelConsumptionLessThanOneCcOt { get; set; }

    public bool ReferralReasonWritingCd { get; set; }

    public bool ReferralReasonOther { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.ReferralReasonOtherDetails")]
    public string ReferralReasonOtherDetails { get; set; }

    #endregion

    #region Travel status

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.IsCurrentSubstanceUseBelowHalfGram")]
    public bool? IsCurrentSubstanceUseBelowHalfGram { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HasSubstanceUseInAnotherBranch")]
    public bool? HasSubstanceUseInAnotherBranch { get; set; }

    #endregion

    #region Cigarette usage

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HasCigaretteUse")]
    public bool? HasCigaretteUse { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.CigaretteTreatmentStatus")]
    public int CigaretteTreatmentStatus { get; set; }

    #endregion

    #region Weight status

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HeightCm")]
    public int? HeightCm { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.WeightKg")]
    public decimal? WeightKg { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HasOverOrUnderWeight")]
    public bool? HasOverOrUnderWeight { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HadHealthyDietInPastYear")]
    public bool? HadHealthyDietInPastYear { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.RelapsedDueToWeightIssue")]
    public bool? RelapsedDueToWeightIssue { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HadHealthyWeightBeforeRecovery")]
    public bool? HadHealthyWeightBeforeRecovery { get; set; }

    #endregion

    #region Educational resources

    public bool EducationalResourceJahanbiniBooklet { get; set; }

    public bool EducationalResourceSixtyDegreeBook { get; set; }

    public bool EducationalResourceEshghBook { get; set; }

    public bool EducationalResourceTwelveArticles { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.WroteOneCdPerWeek")]
    public bool? WroteOneCdPerWeek { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.CompletedThirtyCdExam")]
    public bool? CompletedThirtyCdExam { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.CompletedFortyCdExam")]
    public bool? CompletedFortyCdExam { get; set; }

    #endregion

    #region Parks and sports

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.AttendedParksDuringFirstSixMonths")]
    public bool? AttendedParksDuringFirstSixMonths { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.ParticipatedInAtLeastOneSport")]
    public bool? ParticipatedInAtLeastOneSport { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.ParticipatedInSportsActivitiesOrCompetitions")]
    public bool? ParticipatedInSportsActivitiesOrCompetitions { get; set; }

    #endregion

    #region Order and discipline

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HasRegularWorkshopAttendance")]
    public bool? HasRegularWorkshopAttendance { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HasRegularLegionAttendance")]
    public bool? HasRegularLegionAttendance { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.HadWeeklyParticipationAndTravelDeclarationInFirstTrip")]
    public bool? HadWeeklyParticipationAndTravelDeclarationInFirstTrip { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.TookSecondTripExams")]
    public bool? TookSecondTripExams { get; set; }

    public bool ServiceRolePublications { get; set; }

    public bool ServiceRoleOt { get; set; }

    public bool ServiceRoleSite { get; set; }

    public bool ServiceRoleGuard { get; set; }

    public bool ServiceRoleSecretary { get; set; }

    public bool ServiceRoleGuide { get; set; }

    public bool ServiceRoleNewcomer { get; set; }

    public bool ServiceRoleMarzban { get; set; }

    #endregion

    #region Family relapse factors

    public bool FamilyRelapseFactorCigaretteUse { get; set; }

    public bool FamilyRelapseFactorSickness { get; set; }

    public bool FamilyRelapseFactorFinancialAndMaritalIssues { get; set; }

    public bool FamilyRelapseFactorFamilyIssues { get; set; }

    public bool FamilyRelapseFactorEconomicIssues { get; set; }

    public bool FamilyRelapseFactorOverweight { get; set; }

    public bool FamilyRelapseFactorOther { get; set; }

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.FamilyRelapseFactorOtherDetails")]
    public string FamilyRelapseFactorOtherDetails { get; set; }

    #endregion

    #region Medical notes

    [NopResourceDisplayName("Admin.DisciplinaryForms.Fields.MedicalConditionAndMedicationNotes")]
    public string MedicalConditionAndMedicationNotes { get; set; }

    #endregion

    #region Display

    public string CreatedOnPersian { get; set; }

    #endregion
}
