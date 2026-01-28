namespace Nop.Core.Domain.Customers;

/// <summary>
/// Represents an agency
/// </summary>
public partial class Agency : BaseEntity
{
    /// <summary>
    /// Gets or sets the city identifier
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is published
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}

