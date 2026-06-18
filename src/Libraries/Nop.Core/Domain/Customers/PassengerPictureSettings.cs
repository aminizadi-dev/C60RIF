using Nop.Core.Configuration;

namespace Nop.Core.Domain.Customers;

/// <summary>
/// Passenger picture upload settings
/// </summary>
public partial class PassengerPictureSettings : ISettings
{
    /// <summary>
    /// Target maximum size (KB) for passenger upload after compression.
    /// </summary>
    public int TargetUploadSizeKb { get; set; } = 300;
}
