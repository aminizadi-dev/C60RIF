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

public partial class RecoveryFormController : BaseAdminController
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IClinicService _clinicService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IExportManager _exportManager;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IRecoveryFormModelFactory _recoveryFormModelFactory;
    protected readonly IRecoveryFormService _recoveryFormService;
    protected readonly IPersonService _personService;
    protected readonly IPermissionService _permissionService;
    protected readonly IWorkContext _workContext;

    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public RecoveryFormController(
        IAgencyService agencyService,
        IClinicService clinicService,
        ICustomerActivityService customerActivityService,
        IExportManager exportManager,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IRecoveryFormModelFactory recoveryFormModelFactory,
        IRecoveryFormService recoveryFormService,
        IPersonService personService,
        IPermissionService permissionService,
        IWorkContext workContext)
    {
        _agencyService = agencyService;
        _clinicService = clinicService;
        _customerActivityService = customerActivityService;
        _exportManager = exportManager;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _recoveryFormModelFactory = recoveryFormModelFactory;
        _recoveryFormService = recoveryFormService;
        _personService = personService;
        _permissionService = permissionService;
        _workContext = workContext;
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
        var model = await _recoveryFormModelFactory.PrepareRecoveryFormSearchModelAsync(new RecoveryFormSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> RecoveryFormList(RecoveryFormSearchModel searchModel)
    {
        //prepare model
        var model = await _recoveryFormModelFactory.PrepareRecoveryFormListModelAsync(searchModel);

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
        var model = await _recoveryFormModelFactory.PrepareRecoveryFormModelAsync(new RecoveryFormModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create(RecoveryFormModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            await ApplyRecoveryNoNormalizationAsync(model);

            if (!ModelState.IsValid)
                return await RecreateModelAsync(model);

            if (await _recoveryFormService.IsRecoveryNoExistsAsync(model.RecoveryNo))
            {
                ModelState.AddModelError(nameof(model.RecoveryNo),
                    await _localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.Duplicate"));
            }

            await ValidateTravelDurationAsync(model);

            if (!ModelState.IsValid)
                return await RecreateModelAsync(model);

            if (model.AntiX2.GetValueOrDefault() <= 0)
                model.AntiX2 = null;
            if (model.ClinicId.GetValueOrDefault() <= 0)
                model.ClinicId = null;

            //fill entity from model
            var recoveryForm = model.ToEntity<RecoveryForm>();
            recoveryForm.CreatedOnUtc = DateTime.UtcNow;
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            var isAdminUser = currentCustomer != null &&
                await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL, currentCustomer);

            //create the shared person identity, then link it to the recovery form
            var person = new Person
            {
                FirstName = model.PersonName,
                MobileNumber = model.MobileNumber,
                CardNo = model.CardNo,
                BirthYear = model.BirthYear,
                CreatedOnUtc = recoveryForm.CreatedOnUtc,
                CreatedByCustomerId = isAdminUser ? currentCustomer.Id : null
            };
            await _personService.InsertPersonAsync(person);
            recoveryForm.PersonId = person.Id;

            if (isAdminUser)
                recoveryForm.CreatedByCustomerId = currentCustomer.Id;

            await _recoveryFormService.InsertRecoveryFormAsync(recoveryForm);

            if (!string.IsNullOrWhiteSpace(model.CardNo) &&
                await _personService.IsCardNoExistsAsync(model.CardNo, person.Id))
            {
                _notificationService.WarningNotification(
                    await _localizationService.GetResourceAsync("Admin.Passengers.Fields.CardNo.DuplicateWarning"));
            }

            //activity log for all admin panel users
            if (isAdminUser)
            {
                await _customerActivityService.InsertActivityAsync(currentCustomer, "AddNewPassenger",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewPassenger"), recoveryForm.Id), recoveryForm);
            }
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = recoveryForm.Id });
        }

        return await RecreateModelAsync(model);
    }

    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a recovery form with the specified id
        var recoveryForm = await _recoveryFormService.GetRecoveryFormByIdAsync(id);
        if (recoveryForm == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _recoveryFormModelFactory.PrepareRecoveryFormModelAsync(null, recoveryForm);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Edit(RecoveryFormModel model, bool continueEditing)
    {
        //try to get a recovery form with the specified id
        var recoveryForm = await _recoveryFormService.GetRecoveryFormByIdAsync(model.Id);
        if (recoveryForm == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            try
            {
                await ApplyRecoveryNoNormalizationAsync(model);

                if (!ModelState.IsValid)
                    return await RecreateModelAsync(model, recoveryForm);

                if (await _recoveryFormService.IsRecoveryNoExistsAsync(model.RecoveryNo, recoveryForm.Id))
                {
                    ModelState.AddModelError(nameof(model.RecoveryNo),
                        await _localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.Duplicate"));
                }

                await ValidateTravelDurationAsync(model);

                if (!ModelState.IsValid)
                    return await RecreateModelAsync(model, recoveryForm);

                if (model.AntiX2.GetValueOrDefault() <= 0)
                    model.AntiX2 = null;
                if (model.ClinicId.GetValueOrDefault() <= 0)
                    model.ClinicId = null;

                //fill entity from model (PersonId is preserved from the loaded entity)
                var personId = recoveryForm.PersonId;
                recoveryForm = model.ToEntity(recoveryForm);
                recoveryForm.PersonId = personId;

                //update the shared person identity
                var person = await _personService.GetPersonByIdAsync(personId);
                if (person != null)
                {
                    person.FirstName = model.PersonName;
                    person.MobileNumber = model.MobileNumber;
                    person.CardNo = model.CardNo;
                    person.BirthYear = model.BirthYear;
                    await _personService.UpdatePersonAsync(person);
                }

                await _recoveryFormService.UpdateRecoveryFormAsync(recoveryForm);

                if (!string.IsNullOrWhiteSpace(model.CardNo) &&
                    await _personService.IsCardNoExistsAsync(model.CardNo, personId))
                {
                    _notificationService.WarningNotification(
                        await _localizationService.GetResourceAsync("Admin.Passengers.Fields.CardNo.DuplicateWarning"));
                }

                //activity log
                await _customerActivityService.InsertActivityAsync("EditPassenger",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditPassenger"), recoveryForm.Id), recoveryForm);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = recoveryForm.Id });
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
            }
        }

        return await RecreateModelAsync(model, recoveryForm);
    }

    private async Task ApplyRecoveryNoNormalizationAsync(RecoveryFormModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.CardNo))
            model.CardNo = DigitHelper.ToEnglishDigits(model.CardNo.Trim());

        model.RecoveryNo = _recoveryFormService.NormalizeRecoveryNo(model.RecoveryNo, model.TravelEndDateUtc);

        if (model.RecoveryNo?.Length > RecoveryNoHelper.MaxRecoveryNoLength)
        {
            ModelState.AddModelError(nameof(model.RecoveryNo),
                await _localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.MaxLength"));
        }
    }

    private async Task ValidateTravelDurationAsync(RecoveryFormModel model)
    {
        if (!model.TravelStartDateUtc.HasValue || !model.TravelEndDateUtc.HasValue)
            return;

        if (model.TravelEndDateUtc.Value <= model.TravelStartDateUtc.Value.AddYears(2))
            return;

        var message = await _localizationService.GetResourceAsync("Admin.Passengers.Fields.TravelDuration.Warning");
        ModelState.AddModelError(nameof(model.TravelEndDateUtc), message);
        _notificationService.ErrorNotification(message);
    }

    private async Task<IActionResult> RecreateModelAsync(RecoveryFormModel model, RecoveryForm recoveryForm = null)
    {
        model = await _recoveryFormModelFactory.PrepareRecoveryFormModelAsync(model, recoveryForm, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a recovery form with the specified id
        var recoveryForm = await _recoveryFormService.GetRecoveryFormByIdAsync(id);
        if (recoveryForm == null)
            return RedirectToAction("List");

        try
        {
            await _recoveryFormService.DeleteRecoveryFormAsync(recoveryForm);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeletePassenger",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeletePassenger"), recoveryForm.Id), recoveryForm);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            _notificationService.ErrorNotification(exc.Message);
            return RedirectToAction("Edit", new { id = recoveryForm.Id });
        }
    }

    [HttpPost, ActionName("ExportExcel")]
    [FormValueRequired("exportexcel-all")]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> ExportExcelAll(RecoveryFormSearchModel model)
    {
        var recoveryForms = await _recoveryFormService.GetAllRecoveryFormsAsync(
            recoveryNo: string.IsNullOrWhiteSpace(model.SearchRecoveryNo)
                ? null
                : _recoveryFormService.NormalizeRecoveryNo(model.SearchRecoveryNo, null),
            personName: model.SearchPersonName,
            cityId: model.SearchCityId,
            agencyId: model.SearchAgencyId,
            clinicId: model.SearchClinicId,
            antiXId: model.SearchAntiXId,
            guideNameAndLegionNo: model.SearchGuideNameAndLegionNo,
            cardNo: string.IsNullOrWhiteSpace(model.SearchCardNo)
                ? null
                : DigitHelper.ToEnglishDigits(model.SearchCardNo.Trim()),
            travelStartDateUtc: model.SearchTravelStartDateUtc,
            travelEndDateUtc: model.SearchTravelEndDateUtc,
            recoveryYear: model.SearchRecoveryYear,
            recoveryMonth: model.SearchRecoveryMonth);

        var bytes = await _exportManager.ExportRecoveryFormsToXlsxAsync(recoveryForms.ToList());

        return File(bytes, MimeTypes.TextXlsx, "recoveryforms.xlsx");
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Passengers.PASSENGERS_VIEW)]
    public virtual async Task<IActionResult> ExportExcelSelected(string selectedIds)
    {
        var recoveryForms = new List<RecoveryForm>();
        if (selectedIds != null)
        {
            var ids = selectedIds
                .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .ToArray();
            recoveryForms.AddRange(await _recoveryFormService.GetRecoveryFormsByIdsAsync(ids));
        }

        var bytes = await _exportManager.ExportRecoveryFormsToXlsxAsync(recoveryForms);

        return File(bytes, MimeTypes.TextXlsx, "recoveryforms.xlsx");
    }

    #endregion
}
