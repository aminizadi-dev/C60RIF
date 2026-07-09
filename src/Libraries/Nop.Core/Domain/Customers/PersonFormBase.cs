using System;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Base class for all person-related forms (recovery, disciplinary, william).
/// Holds the fields that every form genuinely shares: the link to the person and audit info.
/// Note: this inheritance exists at the C# level only for DRY; each concrete form is mapped to its own table.
/// </summary>
public abstract partial class PersonFormBase : BaseEntity
{
    /// <summary>
    /// Gets or sets the related person identifier (required link to the shared identity aggregate).
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the date and time the form was created (UTC).
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier who created this record.
    /// </summary>
    public int? CreatedByCustomerId { get; set; }
}
