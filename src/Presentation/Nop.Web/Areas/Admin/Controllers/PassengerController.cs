using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class PassengerController : BaseAdminController
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IClinicService _clinicService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IExportManager _exportManager;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPassengerModelFactory _passengerModelFactory;
    protected readonly IPassengerService _passengerService;
    protected readonly IPermissionService _permissionService;

    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public PassengerController(
        IAgencyService agencyService,
        IClinicService clinicService,
        ICustomerActivityService customerActivityService,
        IExportManager exportManager,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPassengerModelFactory passengerModelFactory,
        IPassengerService passengerService,
        IPermissionService permissionService)
    {
        _agencyService = agencyService;
        _clinicService = clinicService;
        _customerActivityService = customerActivityService;
        _exportManager = exportManager;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _passengerModelFactory = passengerModelFactory;
        _passengerService = passengerService;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerSearchModelAsync(new PassengerSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> PassengerList(PassengerSearchModel searchModel)
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerListModelAsync(searchModel);

        return Json(model);
    }

    [HttpGet]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> GetAgenciesByCityId(int cityId)
    {
        if (cityId == 0)
            return Json(Array.Empty<object>());

        var agencies = cityId == -1
            ? (await _agencyService.GetAllAgenciesAsync(pageIndex: 0, pageSize: int.MaxValue)).ToList()
            : await _agencyService.GetAgenciesByCityIdAsync(cityId, showHidden: true);

        var result = agencies.Select(agency => new { id = agency.Id, name = agency.Name, cityId = agency.CityId });

        return Json(result);
    }

    [HttpGet]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> GetClinicsByCityId(int cityId)
    {
        if (cityId == 0)
            return Json(Array.Empty<object>());

        var clinics = cityId == -1
            ? (await _clinicService.GetAllClinicsAsync(pageIndex: 0, pageSize: int.MaxValue)).ToList()
            : await _clinicService.GetClinicsByCityIdAsync(cityId, showHidden: true);

        var result = clinics.Select(clinic => new { id = clinic.Id, name = clinic.Name, cityId = clinic.CityId });

        return Json(result);
    }

    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerModelAsync(new PassengerModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create(PassengerModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            if (await _passengerService.IsRecoveryNoExistsAsync(model.RecoveryNo))
            {
                ModelState.AddModelError(nameof(model.RecoveryNo),
                    await _localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.Duplicate"));
            }

            if (!ModelState.IsValid)
                return await RecreateModelAsync(model);

            if (model.AntiX2.GetValueOrDefault() <= 0)
                model.AntiX2 = null;

            //fill entity from model
            var passenger = model.ToEntity<Passenger>();
            passenger.CreatedOnUtc = DateTime.UtcNow;

            await _passengerService.InsertPassengerAsync(passenger);

            if (!string.IsNullOrWhiteSpace(model.CardNo) &&
                await _passengerService.IsCardNoExistsAsync(model.CardNo, passenger.Id))
            {
                _notificationService.WarningNotification(
                    await _localizationService.GetResourceAsync("Admin.Passengers.Fields.CardNo.DuplicateWarning"));
            }

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewPassenger",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewPassenger"), passenger.Id), passenger);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = passenger.Id });
        }

        return await RecreateModelAsync(model);
    }

    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a passenger with the specified id
        var passenger = await _passengerService.GetPassengerByIdAsync(id);
        if (passenger == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _passengerModelFactory.PreparePassengerModelAsync(null, passenger);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Edit(PassengerModel model, bool continueEditing)
    {
        //try to get a passenger with the specified id
        var passenger = await _passengerService.GetPassengerByIdAsync(model.Id);
        if (passenger == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            try
            {
                if (await _passengerService.IsRecoveryNoExistsAsync(model.RecoveryNo, passenger.Id))
                {
                    ModelState.AddModelError(nameof(model.RecoveryNo),
                        await _localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.Duplicate"));
                }

                if (!ModelState.IsValid)
                    return await RecreateModelAsync(model, passenger);

                if (model.AntiX2.GetValueOrDefault() <= 0)
                    model.AntiX2 = null;

                //fill entity from model
                passenger = model.ToEntity(passenger);

                await _passengerService.UpdatePassengerAsync(passenger);

                if (!string.IsNullOrWhiteSpace(model.CardNo) &&
                    await _passengerService.IsCardNoExistsAsync(model.CardNo, passenger.Id))
                {
                    _notificationService.WarningNotification(
                        await _localizationService.GetResourceAsync("Admin.Passengers.Fields.CardNo.DuplicateWarning"));
                }

                //activity log
                await _customerActivityService.InsertActivityAsync("EditPassenger",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditPassenger"), passenger.Id), passenger);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = passenger.Id });
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
            }
        }

        return await RecreateModelAsync(model, passenger);
    }

    private async Task<IActionResult> RecreateModelAsync(PassengerModel model, Passenger passenger = null)
    {
        model = await _passengerModelFactory.PreparePassengerModelAsync(model, passenger, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a passenger with the specified id
        var passenger = await _passengerService.GetPassengerByIdAsync(id);
        if (passenger == null)
            return RedirectToAction("List");

        try
        {
            await _passengerService.DeletePassengerAsync(passenger);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeletePassenger",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeletePassenger"), passenger.Id), passenger);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            _notificationService.ErrorNotification(exc.Message);
            return RedirectToAction("Edit", new { id = passenger.Id });
        }
    }

    [HttpPost, ActionName("ExportExcel")]
    [FormValueRequired("exportexcel-all")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> ExportExcelAll(PassengerSearchModel model)
    {
        var passengers = await _passengerService.GetAllPassengersAsync(
            recoveryNo: model.SearchRecoveryNo,
            personName: model.SearchPersonName,
            cityId: model.SearchCityId,
            agencyId: model.SearchAgencyId,
            clinicId: model.SearchClinicId,
            antiXId: model.SearchAntiXId,
            guideNameAndLegionNo: model.SearchGuideNameAndLegionNo,
            cardNo: model.SearchCardNo,
            travelStartDateUtc: model.SearchTravelStartDateUtc,
            travelEndDateUtc: model.SearchTravelEndDateUtc,
            recoveryYear: model.SearchRecoveryYear,
            recoveryMonth: model.SearchRecoveryMonth);

        var bytes = await _exportManager.ExportPassengersToXlsxAsync(passengers.ToList());

        return File(bytes, MimeTypes.TextXlsx, "passengers.xlsx");
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> ExportExcelSelected(string selectedIds)
    {
        var passengers = new List<Passenger>();
        if (selectedIds != null)
        {
            var ids = selectedIds
                .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .ToArray();
            passengers.AddRange(await _passengerService.GetPassengersByIdsAsync(ids));
        }

        var bytes = await _exportManager.ExportPassengersToXlsxAsync(passengers);

        return File(bytes, MimeTypes.TextXlsx, "passengers.xlsx");
    }

    #endregion
}

