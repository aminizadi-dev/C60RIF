using System;
using System.Globalization;
using Nop.Core;

namespace Nop.Services.Customers;

/// <summary>
/// Normalizes recovery numbers based on Persian travel end year rules
/// </summary>
public static class RecoveryNoHelper
{
    public const int RecoveryYear1405 = 1405;
    public const string Prefix1405 = "05";
    public const int MaxRecoveryNoLength = 20;

    /// <summary>
    /// Applies year-based prefix rules (e.g. 05 for Persian year 1405)
    /// </summary>
    public static string Normalize(string recoveryNo, DateTime? travelEndDateUtc)
    {
        var normalized = DigitHelper.ToEnglishDigits(recoveryNo)?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(normalized) || !travelEndDateUtc.HasValue)
            return normalized;

        var persianYear = new PersianCalendar().GetYear(travelEndDateUtc.Value);

        if (persianYear == RecoveryYear1405 && !normalized.StartsWith(Prefix1405, StringComparison.Ordinal))
            normalized = Prefix1405 + normalized;

        return normalized;
    }
}
