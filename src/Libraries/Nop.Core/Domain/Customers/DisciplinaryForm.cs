using System;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Represents disciplinary form data for a passenger.
/// </summary>
public partial class DisciplinaryForm : BaseEntity
{
    #region Passenger link

    /// <summary>
    /// Gets or sets the related passenger identifier (optional one-to-one link).
    /// </summary>
    public int? PassengerId { get; set; }

    #endregion

    #region Personal info

    /// <summary>
    /// Gets or sets the form snapshot of person name.
    /// </summary>
    public string PersonName { get; set; }

    /// <summary>
    /// Gets or sets the form snapshot of card number.
    /// </summary>
    public string CardNo { get; set; }

    public string FamilyName { get; set; }

    public int? Age { get; set; }

    public bool? IsMarried { get; set; }

    public bool? IsEmployed { get; set; }

    public EducationLevel? EducationLevel { get; set; }

    /// <summary>
    /// Gets or sets the agency identifier.
    /// </summary>
    public int? AgencyId { get; set; }

    public string AgencyName { get; set; }

    public string LegionNo { get; set; }

    /// <summary>
    /// Gets or sets previous substance type and amount.
    /// </summary>
    public string PreviousSubstanceUseDetails { get; set; }

    /// <summary>
    /// Gets or sets current substance type and amount.
    /// </summary>
    public string CurrentSubstanceUseDetails { get; set; }

    #endregion

    #region Disciplinary referral reasons

    /// <summary>
    /// Gets or sets absence duration (less than or more than six months; mutually exclusive).
    /// </summary>
    public DisciplinaryReferralAbsenceDuration AbsenceDuration { get; set; }

    /// <summary>
    /// Gets or sets relapse timing after recovery (mutually exclusive).
    /// </summary>
    public DisciplinaryReferralRelapseDuration RelapseDuration { get; set; }

    /// <summary>
    /// Gets or sets additional referral reasons (re-travel, writing CD, other).
    /// </summary>
    public DisciplinaryReferralReasonFlags ReferralReasons { get; set; }

    /// <summary>
    /// Gets or sets optional details for "other" referral reason.
    /// </summary>
    public string ReferralReasonOtherDetails { get; set; }

    #endregion

    #region Travel status

    /// <summary>
    /// Gets or sets whether current substance use amount is lower than half gram.
    /// </summary>
    public bool? IsCurrentSubstanceUseBelowHalfGram { get; set; }

    /// <summary>
    /// Gets or sets whether substance was used in another branch during travel.
    /// </summary>
    public bool? HasSubstanceUseInAnotherBranch { get; set; }

    #endregion

    #region Cigarette usage

    public bool? HasCigaretteUse { get; set; }

    /// <summary>
    /// Gets or sets cigarette treatment status (single-select).
    /// </summary>
    public CigaretteTreatmentStatus CigaretteTreatmentStatus { get; set; }

    #endregion

    #region Weight status

    public int? HeightCm { get; set; }

    public decimal? WeightKg { get; set; }

    public bool? HasOverOrUnderWeight { get; set; }

    public bool? HadHealthyDietInPastYear { get; set; }

    public bool? RelapsedDueToWeightIssue { get; set; }

    public bool? HadHealthyWeightBeforeRecovery { get; set; }

    #endregion

    #region Educational resources

    /// <summary>
    /// Gets or sets selected educational resources.
    /// </summary>
    public EducationalResourceFlags EducationalResources { get; set; }

    public bool? WroteOneCdPerWeek { get; set; }

    public bool? CompletedThirtyCdExam { get; set; }

    public bool? CompletedFortyCdExam { get; set; }

    #endregion

    #region Parks and sports

    public bool? AttendedParksDuringFirstSixMonths { get; set; }

    public bool? ParticipatedInAtLeastOneSport { get; set; }

    public bool? ParticipatedInSportsActivitiesOrCompetitions { get; set; }

    #endregion

    #region Order and discipline

    public bool? HasRegularWorkshopAttendance { get; set; }

    public bool? HasRegularLegionAttendance { get; set; }

    public bool? HadWeeklyParticipationAndTravelDeclarationInFirstTrip { get; set; }

    public bool? TookSecondTripExams { get; set; }

    public ServiceRoleFlags ServiceRoles { get; set; }

    #endregion

    #region Family relapse factors

    /// <summary>
    /// Gets or sets selected family-perceived relapse factors.
    /// </summary>
    public FamilyRelapseFactorFlags FamilyRelapseFactors { get; set; }

    public string FamilyRelapseFactorOtherDetails { get; set; }

    #endregion

    #region Medical notes

    /// <summary>
    /// Gets or sets notes about diseases and medications.
    /// </summary>
    public string MedicalConditionAndMedicationNotes { get; set; }

    #endregion

    #region Audit

    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier who created this record.
    /// </summary>
    public int? CreatedByCustomerId { get; set; }

    #endregion
}
