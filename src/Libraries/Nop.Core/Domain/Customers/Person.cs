using System;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Represents a person (the shared identity across all forms: recovery, disciplinary, william).
/// A person is the aggregate root that different forms reference through PersonId.
/// </summary>
public partial class Person : BaseEntity
{
    /// <summary>
    /// Gets or sets the first name (given name). For legacy recovery records this may hold the full name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name (family name).
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber { get; set; }

    /// <summary>
    /// Gets or sets the national card number. Not guaranteed unique.
    /// </summary>
    public string CardNo { get; set; }

    /// <summary>
    /// Gets or sets the birth year (Persian year).
    /// </summary>
    public int? BirthYear { get; set; }

    /// <summary>
    /// Gets or sets the date and time the person record was created (UTC).
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier who created this record.
    /// </summary>
    public int? CreatedByCustomerId { get; set; }
}
