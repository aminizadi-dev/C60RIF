using Nop.Services.Logging;
using SkiaSharp;

namespace Nop.Services.Media;

/// <summary>
/// Compresses passenger upload images to a configured target size.
/// </summary>
public partial class PassengerPictureCompressionService : IPassengerPictureCompressionService
{
    protected readonly ILogger _logger;

    public PassengerPictureCompressionService(ILogger logger)
    {
        _logger = logger;
    }

    public virtual async Task<byte[]> CompressToTargetSizeAsync(byte[] pictureBinary, string mimeType, int targetBytes)
    {
        if (pictureBinary == null || pictureBinary.Length == 0 || targetBytes <= 0 || pictureBinary.Length <= targetBytes)
            return pictureBinary ?? Array.Empty<byte>();

        var format = GetSupportedFormat(mimeType);
        if (!format.HasValue)
            return pictureBinary;

        try
        {
            using var sourceBitmap = SKBitmap.Decode(pictureBinary);
            if (sourceBitmap == null)
                return pictureBinary;

            var bestResult = pictureBinary;
            var quality = 90;
            var width = sourceBitmap.Width;
            var height = sourceBitmap.Height;

            using var workingBitmap = sourceBitmap.Copy();
            SKBitmap currentBitmap = workingBitmap;

            for (var attempt = 0; attempt < 10; attempt++)
            {
                using var image = SKImage.FromBitmap(currentBitmap);
                var encoded = image.Encode(format.Value, quality).ToArray();

                if (encoded.Length <= targetBytes)
                    return encoded;

                if (encoded.Length < bestResult.Length)
                    bestResult = encoded;

                quality = Math.Max(40, quality - 8);

                var shouldDownscale = attempt >= 2 && attempt % 2 == 0;
                if (!shouldDownscale)
                    continue;

                var nextWidth = Math.Max(320, (int)(width * 0.9f));
                var nextHeight = Math.Max(320, (int)(height * 0.9f));
                if (nextWidth == width && nextHeight == height)
                    continue;

                width = nextWidth;
                height = nextHeight;

                var resized = currentBitmap.Resize(new SKImageInfo(width, height), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
                if (resized == null)
                    continue;

                if (!ReferenceEquals(currentBitmap, workingBitmap))
                    currentBitmap.Dispose();

                currentBitmap = resized;
            }

            if (!ReferenceEquals(currentBitmap, workingBitmap))
                currentBitmap.Dispose();

            return bestResult;
        }
        catch (Exception exception)
        {
            await _logger.WarningAsync($"Passenger picture compression failed for MIME type '{mimeType}'.", exception);
            return pictureBinary;
        }
    }

    protected virtual SKEncodedImageFormat? GetSupportedFormat(string mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
            return null;

        var normalizedMimeType = mimeType.ToLowerInvariant();

        return normalizedMimeType switch
        {
            "image/jpeg" or "image/jpg" or "image/pjpeg" => SKEncodedImageFormat.Jpeg,
            "image/webp" => SKEncodedImageFormat.Webp,
            "image/png" => SKEncodedImageFormat.Png,
            _ => null
        };
    }
}
