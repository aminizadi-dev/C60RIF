using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
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
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPassengerModelFactory _passengerModelFactory;
    protected readonly IPassengerService _passengerService;
    protected readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public PassengerController(
        IAgencyService agencyService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPassengerModelFactory passengerModelFactory,
        IPassengerService passengerService,
        IPermissionService permissionService)
    {
        _agencyService = agencyService;
        _customerActivityService = customerActivityService;
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

    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerSearchModelAsync(new PassengerSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> PassengerList(PassengerSearchModel searchModel)
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerListModelAsync(searchModel);

        return Json(model);
    }

    [HttpGet]
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
    public virtual async Task<IActionResult> GetAgenciesByCityId(int cityId)
    {
        if (cityId <= 0)
            return Json(Array.Empty<object>());

        var agencies = await _agencyService.GetAgenciesByCityIdAsync(cityId, showHidden: true);
        var result = agencies.Select(agency => new { id = agency.Id, name = agency.Name });

        return Json(result);
    }

    [CheckPermission(StandardPermission.Customers.CUSTOMERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _passengerModelFactory.PreparePassengerModelAsync(new PassengerModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create(PassengerModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            //fill entity from model
            var passenger = model.ToEntity<Passenger>();
            passenger.CreatedOnUtc = DateTime.UtcNow;

            await _passengerService.InsertPassengerAsync(passenger);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewPassenger",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewPassenger"), passenger.Id), passenger);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Passengers.Passengers.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = passenger.Id });
        }

        //prepare model
        model = await _passengerModelFactory.PreparePassengerModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Customers.CUSTOMERS_VIEW)]
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
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_CREATE_EDIT_DELETE)]
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
                //fill entity from model
                passenger = model.ToEntity(passenger);

                await _passengerService.UpdatePassengerAsync(passenger);

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

        //prepare model
        model = await _passengerModelFactory.PreparePassengerModelAsync(model, passenger, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Customers.CUSTOMERS_CREATE_EDIT_DELETE)]
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

    #endregion
}

