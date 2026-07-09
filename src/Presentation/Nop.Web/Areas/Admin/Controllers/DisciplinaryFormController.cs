using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class DisciplinaryFormController : BaseAdminController
{
    #region Fields

    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IAntiXService _antiXService;
    protected readonly IDisciplinaryFormModelFactory _disciplinaryFormModelFactory;
    protected readonly IDisciplinaryFormService _disciplinaryFormService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IRecoveryFormService _recoveryFormService;
    protected readonly IPersonService _personService;
    protected readonly IPermissionService _permissionService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public DisciplinaryFormController(
        ICustomerActivityService customerActivityService,
        IAntiXService antiXService,
        IDisciplinaryFormModelFactory disciplinaryFormModelFactory,
        IDisciplinaryFormService disciplinaryFormService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IRecoveryFormService recoveryFormService,
        IPersonService personService,
        IPermissionService permissionService,
        IWorkContext workContext)
    {
        _customerActivityService = customerActivityService;
        _antiXService = antiXService;
        _disciplinaryFormModelFactory = disciplinaryFormModelFactory;
        _disciplinaryFormService = disciplinaryFormService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _recoveryFormService = recoveryFormService;
        _personService = personService;
        _permissionService = permissionService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    public virtual IActionResult Index() => RedirectToAction("List");

    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        var model = await _disciplinaryFormModelFactory.PrepareDisciplinaryFormSearchModelAsync(new DisciplinaryFormSearchModel());
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_VIEW)]
    public virtual async Task<IActionResult> DisciplinaryFormList(DisciplinaryFormSearchModel searchModel)
    {
        var model = await _disciplinaryFormModelFactory.PrepareDisciplinaryFormListModelAsync(searchModel);
        return Json(model);
    }

    [HttpGet]
    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_VIEW)]
    public virtual async Task<IActionResult> GetRecoveryFormByCardNo(string cardNo)
    {
        if (string.IsNullOrWhiteSpace(cardNo))
            return Json(new { found = false });

        cardNo = DigitHelper.ToEnglishDigits(cardNo.Trim());

        var person = (await _personService.GetPersonsByCardNoAsync(cardNo)).FirstOrDefault();
        if (person == null)
            return Json(new { found = false });

        //find the person's recovery form (if any) to prefill form-specific values
        var recoveryForm = await _recoveryFormService.GetRecoveryFormByPersonIdAsync(person.Id);
        var existingForm = (await _disciplinaryFormService.GetDisciplinaryFormsByPersonIdAsync(person.Id)).FirstOrDefault();

        AntiX antiX = null;
        AntiX antiX2 = null;
        if (recoveryForm != null)
        {
            antiX = await _antiXService.GetAntiXByIdAsync(recoveryForm.AntiX1);
            antiX2 = recoveryForm.AntiX2.HasValue ? await _antiXService.GetAntiXByIdAsync(recoveryForm.AntiX2.Value) : null;
        }

        return Json(new
        {
            found = true,
            personId = person.Id,
            recoveryFormId = recoveryForm?.Id,
            personName = person.FirstName,
            familyName = person.LastName,
            cardNo = person.CardNo,
            legionNo = recoveryForm?.GuideNameAndLegionNo,
            educationLevel = recoveryForm != null ? (int?)recoveryForm.Education : null,
            agencyId = recoveryForm?.AgencyId,
            previousSubstanceUseDetails = antiX?.Name,
            currentSubstanceUseDetails = antiX2?.Name,
            hasExistingForm = existingForm != null,
            existingFormId = existingForm?.Id
        });
    }

    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create()
    {
        var model = await _disciplinaryFormModelFactory.PrepareDisciplinaryFormModelAsync(new DisciplinaryFormModel(), null);
        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create(DisciplinaryFormModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            try
            {
                NormalizeModel(model);
                var form = model.ToEntity<DisciplinaryForm>();
                _disciplinaryFormModelFactory.ApplyModelFlagsToEntity(model, form);
                await SetCreatedByCustomerAsync(form);
                form.PersonId = await ResolvePersonIdAsync(model, null);

                await _disciplinaryFormService.InsertDisciplinaryFormAsync(form);

                await _customerActivityService.InsertActivityAsync("AddNewDisciplinaryForm",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewDisciplinaryForm"), form.Id), form);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.DisciplinaryForms.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = form.Id });
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
            }
        }

        return await RecreateModelAsync(model);
    }

    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var form = await _disciplinaryFormService.GetDisciplinaryFormByIdAsync(id);
        if (form == null)
            return RedirectToAction("List");

        var model = await _disciplinaryFormModelFactory.PrepareDisciplinaryFormModelAsync(null, form);
        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Edit(DisciplinaryFormModel model, bool continueEditing)
    {
        var form = await _disciplinaryFormService.GetDisciplinaryFormByIdAsync(model.Id);
        if (form == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            try
            {
                NormalizeModel(model);
                var loadedPersonId = form.PersonId;
                form = model.ToEntity(form);
                _disciplinaryFormModelFactory.ApplyModelFlagsToEntity(model, form);
                form.PersonId = await ResolvePersonIdAsync(model, loadedPersonId);

                await _disciplinaryFormService.UpdateDisciplinaryFormAsync(form);

                await _customerActivityService.InsertActivityAsync("EditDisciplinaryForm",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditDisciplinaryForm"), form.Id), form);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.DisciplinaryForms.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = form.Id });
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
            }
        }

        return await RecreateModelAsync(model, form);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.DisciplinaryForms.DISCIPLINARY_FORMS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var form = await _disciplinaryFormService.GetDisciplinaryFormByIdAsync(id);
        if (form == null)
            return RedirectToAction("List");

        await _disciplinaryFormService.DeleteDisciplinaryFormAsync(form);

        await _customerActivityService.InsertActivityAsync("DeleteDisciplinaryForm",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteDisciplinaryForm"), form.Id), form);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.DisciplinaryForms.Deleted"));
        return RedirectToAction("List");
    }

    #endregion

    #region Utilities

    protected virtual void NormalizeModel(DisciplinaryFormModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.CardNo))
            model.CardNo = DigitHelper.ToEnglishDigits(model.CardNo.Trim());

        if (model.RecoveryFormId <= 0)
            model.RecoveryFormId = null;

        if (model.AgencyId <= 0)
            model.AgencyId = null;
    }

    /// <summary>
    /// Resolves the person the disciplinary form should be linked to.
    /// Priority: (1) update the already linked person when editing, (2) reuse the person of a matched recovery form,
    /// (3) create a new person from the form's identity fields.
    /// </summary>
    protected virtual async Task<int> ResolvePersonIdAsync(DisciplinaryFormModel model, int? existingPersonId)
    {
        //editing: update the identity of the already linked person
        if (existingPersonId.HasValue && existingPersonId.Value > 0)
        {
            var person = await _personService.GetPersonByIdAsync(existingPersonId.Value);
            if (person != null)
            {
                person.FirstName = model.PersonName;
                person.LastName = model.FamilyName;
                person.MobileNumber = model.MobileNumber;
                person.CardNo = model.CardNo;
                await _personService.UpdatePersonAsync(person);
                return person.Id;
            }
        }

        //link to the person of an existing recovery form when provided
        if (model.RecoveryFormId.HasValue && model.RecoveryFormId.Value > 0)
        {
            var recoveryForm = await _recoveryFormService.GetRecoveryFormByIdAsync(model.RecoveryFormId.Value);
            if (recoveryForm != null && recoveryForm.PersonId > 0)
                return recoveryForm.PersonId;
        }

        //otherwise create a new person
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        var createdByCustomerId = currentCustomer != null &&
            await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL, currentCustomer)
            ? (int?)currentCustomer.Id
            : null;

        var newPerson = new Person
        {
            FirstName = model.PersonName,
            LastName = model.FamilyName,
            MobileNumber = model.MobileNumber,
            CardNo = model.CardNo,
            CreatedOnUtc = DateTime.UtcNow,
            CreatedByCustomerId = createdByCustomerId
        };
        await _personService.InsertPersonAsync(newPerson);
        return newPerson.Id;
    }

    protected virtual async Task SetCreatedByCustomerAsync(DisciplinaryForm form)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        if (currentCustomer != null &&
            await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL, currentCustomer))
        {
            form.CreatedByCustomerId = currentCustomer.Id;
        }
    }

    protected virtual async Task<IActionResult> RecreateModelAsync(DisciplinaryFormModel model, DisciplinaryForm form = null)
    {
        model = await _disciplinaryFormModelFactory.PrepareDisciplinaryFormModelAsync(model, form, excludeProperties: true);
        return View(model);
    }

    #endregion
}
