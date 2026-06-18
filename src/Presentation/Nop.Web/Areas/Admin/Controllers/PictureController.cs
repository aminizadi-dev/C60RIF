using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Core.Http.Extensions;
using Nop.Services.Configuration;
using Nop.Services.Media;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class PictureController : BaseAdminController
{
    #region Fields

    protected readonly IPictureService _pictureService;
    protected readonly IPassengerPictureCompressionService _passengerPictureCompressionService;
    protected readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public PictureController(IPictureService pictureService,
        IPassengerPictureCompressionService passengerPictureCompressionService,
        ISettingService settingService)
    {
        _pictureService = pictureService;
        _passengerPictureCompressionService = passengerPictureCompressionService;
        _settingService = settingService;
    }

    #endregion

    #region Methods

    [HttpPost]
    //do not validate request token (XSRF)
    [IgnoreAntiforgeryToken]
    public virtual async Task<IActionResult> AsyncUpload()
    {
        //if (!await _permissionService.Authorize(StandardPermission.UploadPictures))
        //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

        var httpPostedFile = await Request.GetFirstOrDefaultFileAsync();
        if (httpPostedFile == null)
            return Json(new { success = false, message = "No file uploaded" });

        var picture = await _pictureService.InsertPictureAsync(httpPostedFile);

        //when returning JSON the mime-type must be set to text/plain
        //otherwise some browsers will pop-up a "Save As" dialog.

        if (picture == null)
            return Json(new { success = false, message = "Wrong file format" });

        var imageUrl = (await _pictureService.GetPictureUrlAsync(picture, 100)).Url;
        await _pictureService.EnsurePictureBinaryNotStoredInDatabaseAsync(picture.Id);

        return Json(new
        {
            success = true,
            pictureId = picture.Id,
            imageUrl
        });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public virtual async Task<IActionResult> AsyncUploadPassenger()
    {
        var httpPostedFile = await Request.GetFirstOrDefaultFileAsync();
        if (httpPostedFile == null)
            return Json(new { success = false, message = "No file uploaded" });

        var picture = await _pictureService.InsertPictureAsync(httpPostedFile);
        if (picture == null)
            return Json(new { success = false, message = "Wrong file format" });

        var targetUploadSizeKb = await _settingService.GetSettingByKeyAsync<int>(
            _settingService.GetSettingKey(new PassengerPictureSettings(), settings => settings.TargetUploadSizeKb),
            300);

        var targetUploadBytes = Math.Max(1, targetUploadSizeKb) * 1024;
        var uploadedBinary = await _pictureService.LoadPictureBinaryAsync(picture);

        if (uploadedBinary.Length > targetUploadBytes)
        {
            var compressedBinary = await _passengerPictureCompressionService
                .CompressToTargetSizeAsync(uploadedBinary, picture.MimeType, targetUploadBytes);

            if (compressedBinary.Length > 0 && compressedBinary.Length < uploadedBinary.Length)
            {
                picture = await _pictureService.UpdatePictureAsync(
                    picture.Id,
                    compressedBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    isNew: true,
                    validateBinary: false);
            }
        }

        var passengerImageUrl = (await _pictureService.GetPictureUrlAsync(picture, 100)).Url;
        await _pictureService.EnsurePictureBinaryNotStoredInDatabaseAsync(picture.Id);

        return Json(new
        {
            success = true,
            pictureId = picture.Id,
            imageUrl = passengerImageUrl
        });
    }

    #endregion
}