namespace Nop.Web.Framework.Validators;

/// <summary>
/// Represents default values related to validation
/// </summary>
public static partial class NopValidationDefaults
{
    /// <summary>
    /// Gets the name of a rule set used to validate model
    /// </summary>
    public static string ValidationRuleSet => "Validate";

    /// <summary>
    /// Gets the name of a locale used in not-null validation
    /// </summary>
    public static string NotNullValidationLocaleName => "Admin.Common.Validation.NotEmpty";

    /// <summary>
    /// Gets the name of a locale used when an attempted value is invalid for a field
    /// </summary>
    public static string AttemptedValueIsInvalidLocaleName => "Admin.Common.Validation.AttemptedValueIsInvalid";

    /// <summary>
    /// Gets the name of a locale used when a value is invalid for a field
    /// </summary>
    public static string ValueIsInvalidLocaleName => "Admin.Common.Validation.ValueIsInvalid";

    /// <summary>
    /// Gets the name of a locale used when a value must be a number
    /// </summary>
    public static string ValueMustBeANumberLocaleName => "Admin.Common.Validation.ValueMustBeANumber";

    /// <summary>
    /// Gets the name of a locale used when a non-nullable field is missing
    /// </summary>
    public static string MissingRequiredFieldLocaleName => "Admin.Common.Validation.MissingRequiredField";
}