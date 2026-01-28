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

public partial class AntiXController : BaseAdminController
{
    #region Fields

    protected readonly IAntiXModelFactory _antiXModelFactory;
    protected readonly IAntiXService _antiXService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public AntiXController(IAntiXModelFactory antiXModelFactory,
        IAntiXService antiXService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _antiXModelFactory = antiXModelFactory;
        _antiXService = antiXService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region AntiX

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _antiXModelFactory.PrepareAntiXSearchModelAsync(new AntiXSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> AntiXList(AntiXSearchModel searchModel)
    {
        var model = await _antiXModelFactory.PrepareAntiXListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _antiXModelFactory.PrepareAntiXModelAsync(new AntiXModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> Create(AntiXModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var antiX = model.ToEntity<AntiX>();
            await _antiXService.InsertAntiXAsync(antiX);

            await _customerActivityService.InsertActivityAsync("AddNewAntiX",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAntiX"), antiX.Id), antiX);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.AntiX.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = antiX.Id });
        }

        model = await _antiXModelFactory.PrepareAntiXModelAsync(model, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var antiX = await _antiXService.GetAntiXByIdAsync(id);
        if (antiX == null)
            return RedirectToAction("List");

        var model = await _antiXModelFactory.PrepareAntiXModelAsync(null, antiX);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> Edit(AntiXModel model, bool continueEditing)
    {
        var antiX = await _antiXService.GetAntiXByIdAsync(model.Id);
        if (antiX == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            antiX = model.ToEntity(antiX);
            await _antiXService.UpdateAntiXAsync(antiX);

            await _customerActivityService.InsertActivityAsync("EditAntiX",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAntiX"), antiX.Id), antiX);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.AntiX.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = antiX.Id });
        }

        model = await _antiXModelFactory.PrepareAntiXModelAsync(model, antiX, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var antiX = await _antiXService.GetAntiXByIdAsync(id);
        if (antiX == null)
            return RedirectToAction("List");

        try
        {
            await _antiXService.DeleteAntiXAsync(antiX);

            await _customerActivityService.InsertActivityAsync("DeleteAntiX",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAntiX"), antiX.Id), antiX);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.AntiX.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = antiX.Id });
        }
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> PublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var items = (await _antiXService.GetAllAntiXAsync(pageSize: int.MaxValue))
            .Where(item => selectedIds.Contains(item.Id))
            .ToList();

        foreach (var item in items)
        {
            item.Published = true;
            await _antiXService.UpdateAntiXAsync(item);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_ANTIX)]
    public virtual async Task<IActionResult> UnpublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var items = (await _antiXService.GetAllAntiXAsync(pageSize: int.MaxValue))
            .Where(item => selectedIds.Contains(item.Id))
            .ToList();

        foreach (var item in items)
        {
            item.Published = false;
            await _antiXService.UpdateAntiXAsync(item);
        }

        return Json(new { Result = true });
    }

    #endregion
}

