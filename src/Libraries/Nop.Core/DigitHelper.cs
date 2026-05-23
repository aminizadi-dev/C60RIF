namespace Nop.Core;

/// <summary>
/// Converts Persian/Arabic digits to English digits
/// </summary>
public static class DigitHelper
{
    /// <summary>
    /// Converts Persian and Arabic-Indic digits to English (0-9)
    /// </summary>
    public static string ToEnglishDigits(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        input = input.Replace("ي", "ی").Replace("ك", "ک");

        return input
            .Replace(",", "")
            .Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9");
    }
}
