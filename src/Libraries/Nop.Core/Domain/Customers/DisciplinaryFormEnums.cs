using System;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Absence duration when leaving first-trip treatment (mutually exclusive: pick one or none).
/// </summary>
public enum DisciplinaryReferralAbsenceDuration
{
    None = 0,
    LessThanSixMonths = 1,
    MoreThanSixMonths = 2
}

/// <summary>
/// Relapse timing after recovery (mutually exclusive: pick one or none).
/// </summary>
public enum DisciplinaryReferralRelapseDuration
{
    None = 0,
    LessThanOneYearAfterRecovery = 1,
    MoreThanOneYearAfterRecovery = 2
}

/// <summary>
/// Additional disciplinary referral reasons (multi-select; combinable with absence/relapse choices).
/// </summary>
[Flags]
public enum DisciplinaryReferralReasonFlags
{
    None = 0,
    RetravelConsumptionLessThanOneCcOt = 1,
    WritingCd = 2,
    Other = 4
}

/// <summary>
/// Cigarette treatment history (single-select: exactly one option).
/// </summary>
public enum CigaretteTreatmentStatus
{
    Unknown = 0,
    NoActionTaken = 1,
    AttendedWilliamWhiteUnsuccessful = 2,
    HasCigaretteRelease = 3
}

/// <summary>
/// Educational resources used during treatment.
/// </summary>
[Flags]
public enum EducationalResourceFlags
{
    None = 0,
    JahanbiniBooklet = 1,
    SixtyDegreeBook = 2,
    EshghBook = 4,
    TwelveArticles = 8
}

/// <summary>
/// Service roles held by the passenger (multi-select).
/// </summary>
[Flags]
public enum ServiceRoleFlags
{
    None = 0,
    Publications = 1,
    Ot = 2,
    Site = 4,
    Guard = 8,
    Secretary = 16,
    Guide = 32,
    Newcomer = 64,
    Marzban = 128
}

/// <summary>
/// Family-perceived relapse factors.
/// </summary>
[Flags]
public enum FamilyRelapseFactorFlags
{
    None = 0,
    CigaretteUse = 1,
    Sickness = 2,
    FinancialAndMaritalIssues = 4,
    FamilyIssues = 8,
    EconomicIssues = 16,
    Overweight = 32,
    Other = 64
}
