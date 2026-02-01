using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class ClinicController : BaseAdminController
{
    #region Fields

    protected readonly IClinicModelFactory _clinicModelFactory;
    protected readonly IClinicService _clinicService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public ClinicController(IClinicModelFactory clinicModelFactory,
        IClinicService clinicService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _clinicModelFactory = clinicModelFactory;
        _clinicService = clinicService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Clinics

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _clinicModelFactory.PrepareClinicSearchModelAsync(new ClinicSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> ClinicList(ClinicSearchModel searchModel)
    {
        var model = await _clinicModelFactory.PrepareClinicListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _clinicModelFactory.PrepareClinicModelAsync(new ClinicModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> Create(ClinicModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var clinic = model.ToEntity<Clinic>();
            await _clinicService.InsertClinicAsync(clinic);

            await _customerActivityService.InsertActivityAsync("AddNewClinic",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewClinic"), clinic.Id), clinic);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Clinics.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = clinic.Id });
        }

        model = await _clinicModelFactory.PrepareClinicModelAsync(model, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(id);
        if (clinic == null)
            return RedirectToAction("List");

        var model = await _clinicModelFactory.PrepareClinicModelAsync(null, clinic);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> Edit(ClinicModel model, bool continueEditing)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(model.Id);
        if (clinic == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            clinic = model.ToEntity(clinic);
            await _clinicService.UpdateClinicAsync(clinic);

            await _customerActivityService.InsertActivityAsync("EditClinic",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditClinic"), clinic.Id), clinic);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Clinics.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = clinic.Id });
        }

        model = await _clinicModelFactory.PrepareClinicModelAsync(model, clinic, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(id);
        if (clinic == null)
            return RedirectToAction("List");

        try
        {
            await _clinicService.DeleteClinicAsync(clinic);

            await _customerActivityService.InsertActivityAsync("DeleteClinic",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteClinic"), clinic.Id), clinic);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Clinics.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = clinic.Id });
        }
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> PublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var clinics = (await _clinicService.GetAllClinicsAsync(pageSize: int.MaxValue))
            .Where(clinic => selectedIds.Contains(clinic.Id))
            .ToList();

        foreach (var clinic in clinics)
        {
            clinic.Published = true;
            await _clinicService.UpdateClinicAsync(clinic);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CLINICS)]
    public virtual async Task<IActionResult> UnpublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var clinics = (await _clinicService.GetAllClinicsAsync(pageSize: int.MaxValue))
            .Where(clinic => selectedIds.Contains(clinic.Id))
            .ToList();

        foreach (var clinic in clinics)
        {
            clinic.Published = false;
            await _clinicService.UpdateClinicAsync(clinic);
        }

        return Json(new { Result = true });
    }

    #endregion
}

