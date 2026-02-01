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

public partial class AgencyController : BaseAdminController
{
    #region Fields

    protected readonly IAgencyModelFactory _agencyModelFactory;
    protected readonly IAgencyService _agencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public AgencyController(IAgencyModelFactory agencyModelFactory,
        IAgencyService agencyService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _agencyModelFactory = agencyModelFactory;
        _agencyService = agencyService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Agencies

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _agencyModelFactory.PrepareAgencySearchModelAsync(new AgencySearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyList(AgencySearchModel searchModel)
    {
        var model = await _agencyModelFactory.PrepareAgencyListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _agencyModelFactory.PrepareAgencyModelAsync(new AgencyModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Create(AgencyModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var agency = model.ToEntity<Agency>();
            await _agencyService.InsertAgencyAsync(agency);

            await _customerActivityService.InsertActivityAsync("AddNewAgency",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAgency"), agency.Id), agency);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Agencies.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = agency.Id });
        }

        model = await _agencyModelFactory.PrepareAgencyModelAsync(model, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(id);
        if (agency == null)
            return RedirectToAction("List");

        var model = await _agencyModelFactory.PrepareAgencyModelAsync(null, agency);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Edit(AgencyModel model, bool continueEditing)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(model.Id);
        if (agency == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            agency = model.ToEntity(agency);
            await _agencyService.UpdateAgencyAsync(agency);

            await _customerActivityService.InsertActivityAsync("EditAgency",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAgency"), agency.Id), agency);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Agencies.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = agency.Id });
        }

        model = await _agencyModelFactory.PrepareAgencyModelAsync(model, agency, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(id);
        if (agency == null)
            return RedirectToAction("List");

        try
        {
            await _agencyService.DeleteAgencyAsync(agency);

            await _customerActivityService.InsertActivityAsync("DeleteAgency",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAgency"), agency.Id), agency);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Agencies.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = agency.Id });
        }
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> PublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var agencies = (await _agencyService.GetAllAgenciesAsync(pageSize: int.MaxValue))
            .Where(agency => selectedIds.Contains(agency.Id))
            .ToList();

        foreach (var agency in agencies)
        {
            agency.Published = true;
            await _agencyService.UpdateAgencyAsync(agency);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> UnpublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var agencies = (await _agencyService.GetAllAgenciesAsync(pageSize: int.MaxValue))
            .Where(agency => selectedIds.Contains(agency.Id))
            .ToList();

        foreach (var agency in agencies)
        {
            agency.Published = false;
            await _agencyService.UpdateAgencyAsync(agency);
        }

        return Json(new { Result = true });
    }

    #endregion
}

