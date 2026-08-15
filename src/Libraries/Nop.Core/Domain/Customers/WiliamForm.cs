using System;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Represents a Wiliam form record.
/// Identity fields (name, family, membership card no) live on the linked <see cref="Person"/>.
/// </summary>
public partial class WiliamForm : PersonFormBase
{
    /// <summary>
    /// Gets or sets the primary AntiX identifier.
    /// </summary>
    public int AntiXId { get; set; }

    /// <summary>
    /// Gets or sets the substance guide name.
    /// </summary>
    public string SubstanceGuideName { get; set; }

    /// <summary>
    /// Gets or sets the substance release date in UTC.
    /// </summary>
    public DateTime? SubstanceReleaseDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the cigarette quit date in UTC.
    /// </summary>
    public DateTime? CigaretteQuitDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the travel duration in MMDD format (e.g. 1120 = 11 months and 20 days).
    /// </summary>
    public string TravelDuration { get; set; }

    /// <summary>
    /// Gets or sets the cigarette guide name.
    /// </summary>
    public string CigaretteGuideName { get; set; }

    /// <summary>
    /// Gets or sets the cigarette release date in UTC.
    /// </summary>
    public DateTime? CigaretteReleaseDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the agency identifier.
    /// </summary>
    public int AgencyId { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }
}
