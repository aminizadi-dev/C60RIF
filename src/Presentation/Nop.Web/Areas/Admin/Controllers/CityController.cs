using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
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

public partial class CityController : BaseAdminController
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IClinicService _clinicService;
    protected readonly ICityModelFactory _cityModelFactory;
    protected readonly ICityService _cityService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public CityController(IAgencyService agencyService,
        IClinicService clinicService,
        ICityModelFactory cityModelFactory,
        ICityService cityService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _agencyService = agencyService;
        _clinicService = clinicService;
        _cityModelFactory = cityModelFactory;
        _cityService = cityService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Cities

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _cityModelFactory.PrepareCitySearchModelAsync(new CitySearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> CityList(CitySearchModel searchModel)
    {
        var model = await _cityModelFactory.PrepareCityListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _cityModelFactory.PrepareCityModelAsync(new CityModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Create(CityModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var city = model.ToEntity<City>();
            await _cityService.InsertCityAsync(city);

            await _customerActivityService.InsertActivityAsync("AddNewCity",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewCity"), city.Id), city);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = city.Id });
        }

        model = await _cityModelFactory.PrepareCityModelAsync(model, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var city = await _cityService.GetCityByIdAsync(id);
        if (city == null)
            return RedirectToAction("List");

        var model = await _cityModelFactory.PrepareCityModelAsync(null, city);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Edit(CityModel model, bool continueEditing)
    {
        var city = await _cityService.GetCityByIdAsync(model.Id);
        if (city == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            city = model.ToEntity(city);
            await _cityService.UpdateCityAsync(city);

            await _customerActivityService.InsertActivityAsync("EditCity",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditCity"), city.Id), city);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = city.Id });
        }

        model = await _cityModelFactory.PrepareCityModelAsync(model, city, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var city = await _cityService.GetCityByIdAsync(id);
        if (city == null)
            return RedirectToAction("List");

        try
        {
            await _cityService.DeleteCityAsync(city);

            await _customerActivityService.InsertActivityAsync("DeleteCity",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteCity"), city.Id), city);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Cities.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = city.Id });
        }
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> PublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var cities = (await _cityService.GetAllCitiesAsync(pageSize: int.MaxValue))
            .Where(city => selectedIds.Contains(city.Id))
            .ToList();

        foreach (var city in cities)
        {
            city.Published = true;
            await _cityService.UpdateCityAsync(city);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> UnpublishSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var cities = (await _cityService.GetAllCitiesAsync(pageSize: int.MaxValue))
            .Where(city => selectedIds.Contains(city.Id))
            .ToList();

        foreach (var city in cities)
        {
            city.Published = false;
            await _cityService.UpdateCityAsync(city);
        }

        return Json(new { Result = true });
    }

    #endregion

    #region Agencies

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Agencies(AgencySearchModel searchModel)
    {
        var city = await _cityService.GetCityByIdAsync(searchModel.CityId)
            ?? throw new ArgumentException("No city found with the specified id");

        var model = await _cityModelFactory.PrepareAgencyListModelAsync(searchModel, city);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyCreatePopup(int cityId)
    {
        var city = await _cityService.GetCityByIdAsync(cityId);
        if (city == null)
            return RedirectToAction("List");

        var model = await _cityModelFactory.PrepareAgencyModelAsync(new AgencyModel(), city, null);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyCreatePopup(AgencyModel model)
    {
        var city = await _cityService.GetCityByIdAsync(model.CityId);
        if (city == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            var agency = model.ToEntity<Agency>();

            await _agencyService.InsertAgencyAsync(agency);

            await _customerActivityService.InsertActivityAsync("AddNewAgency",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAgency"), agency.Id), agency);

            ViewBag.RefreshPage = true;

            return View(model);
        }

        model = await _cityModelFactory.PrepareAgencyModelAsync(model, city, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyEditPopup(int id)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(id);
        if (agency == null)
            return RedirectToAction("List");

        var city = await _cityService.GetCityByIdAsync(agency.CityId);
        if (city == null)
            return RedirectToAction("List");

        var model = await _cityModelFactory.PrepareAgencyModelAsync(null, city, agency);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyEditPopup(AgencyModel model)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(model.Id);
        if (agency == null)
            return RedirectToAction("List");

        var city = await _cityService.GetCityByIdAsync(agency.CityId);
        if (city == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            agency = model.ToEntity(agency);
            await _agencyService.UpdateAgencyAsync(agency);

            await _customerActivityService.InsertActivityAsync("EditAgency",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAgency"), agency.Id), agency);

            ViewBag.RefreshPage = true;

            return View(model);
        }

        model = await _cityModelFactory.PrepareAgencyModelAsync(model, city, agency, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> AgencyDelete(int id)
    {
        var agency = await _agencyService.GetAgencyByIdAsync(id);
        if (agency == null)
            return Json(new { Result = false });

        await _agencyService.DeleteAgencyAsync(agency);

        await _customerActivityService.InsertActivityAsync("DeleteAgency",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAgency"), agency.Id), agency);

        return Json(new { Result = true });
    }

    #endregion

    #region Clinics

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> Clinics(ClinicSearchModel searchModel)
    {
        var city = await _cityService.GetCityByIdAsync(searchModel.CityId)
            ?? throw new ArgumentException("No city found with the specified id");

        var model = await _cityModelFactory.PrepareClinicListModelAsync(searchModel, city);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> ClinicCreatePopup(int cityId)
    {
        var city = await _cityService.GetCityByIdAsync(cityId);
        if (city == null)
            return RedirectToAction("List");

        var model = await _cityModelFactory.PrepareClinicModelAsync(new ClinicModel(), city, null);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> ClinicCreatePopup(ClinicModel model)
    {
        var city = await _cityService.GetCityByIdAsync(model.CityId);
        if (city == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            var clinic = model.ToEntity<Clinic>();

            await _clinicService.InsertClinicAsync(clinic);

            await _customerActivityService.InsertActivityAsync("AddNewClinic",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewClinic"), clinic.Id), clinic);

            ViewBag.RefreshPage = true;

            return View(model);
        }

        model = await _cityModelFactory.PrepareClinicModelAsync(model, city, null, true);

        return View(model);
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> ClinicEditPopup(int id)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(id);
        if (clinic == null)
            return RedirectToAction("List");

        var city = await _cityService.GetCityByIdAsync(clinic.CityId);
        if (city == null)
            return RedirectToAction("List");

        var model = await _cityModelFactory.PrepareClinicModelAsync(null, city, clinic);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> ClinicEditPopup(ClinicModel model)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(model.Id);
        if (clinic == null)
            return RedirectToAction("List");

        var city = await _cityService.GetCityByIdAsync(clinic.CityId);
        if (city == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            clinic = model.ToEntity(clinic);
            await _clinicService.UpdateClinicAsync(clinic);

            await _customerActivityService.InsertActivityAsync("EditClinic",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditClinic"), clinic.Id), clinic);

            ViewBag.RefreshPage = true;

            return View(model);
        }

        model = await _cityModelFactory.PrepareClinicModelAsync(model, city, clinic, true);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_CITIES)]
    public virtual async Task<IActionResult> ClinicDelete(int id)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(id);
        if (clinic == null)
            return RedirectToAction("List");

        await _clinicService.DeleteClinicAsync(clinic);

        await _customerActivityService.InsertActivityAsync("DeleteClinic",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteClinic"), clinic.Id), clinic);

        return new NullJsonResult();
    }

    #endregion
}

