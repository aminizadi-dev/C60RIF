namespace Nop.Services.Media;

/// <summary>
/// Provides passenger image compression for upload workflow.
/// </summary>
public partial interface IPassengerPictureCompressionService
{
    /// <summary>
    /// Compress the image to be near the target byte size.
    /// </summary>
    /// <param name="pictureBinary">Source image bytes</param>
    /// <param name="mimeType">Source MIME type</param>
    /// <param name="targetBytes">Target max bytes</param>
    /// <returns>Compressed image bytes or source bytes if compression is not possible</returns>
    Task<byte[]> CompressToTargetSizeAsync(byte[] pictureBinary, string mimeType, int targetBytes);
}
