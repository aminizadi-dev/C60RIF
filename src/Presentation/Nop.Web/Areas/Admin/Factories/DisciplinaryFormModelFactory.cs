using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the disciplinary form model factory implementation
/// </summary>
public partial class DisciplinaryFormModelFactory : IDisciplinaryFormModelFactory
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IDisciplinaryFormService _disciplinaryFormService;
    protected readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public DisciplinaryFormModelFactory(
        IAgencyService agencyService,
        IDateTimeHelper dateTimeHelper,
        IDisciplinaryFormService disciplinaryFormService,
        ILocalizationService localizationService)
    {
        _agencyService = agencyService;
        _dateTimeHelper = dateTimeHelper;
        _disciplinaryFormService = disciplinaryFormService;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    public virtual async Task<DisciplinaryFormSearchModel> PrepareDisciplinaryFormSearchModelAsync(DisciplinaryFormSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);
        searchModel.SetGridPageSize();
        return await Task.FromResult(searchModel);
    }

    public virtual async Task<DisciplinaryFormListModel> PrepareDisciplinaryFormListModelAsync(DisciplinaryFormSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var searchCardNo = string.IsNullOrWhiteSpace(searchModel.SearchCardNo)
            ? null
            : DigitHelper.ToEnglishDigits(searchModel.SearchCardNo.Trim());

        var forms = await _disciplinaryFormService.GetAllDisciplinaryFormsAsync(
            personName: searchModel.SearchPersonName,
            familyName: searchModel.SearchFamilyName,
            cardNo: searchCardNo,
            agencyName: searchModel.SearchAgencyName,
            createdFromUtc: searchModel.SearchCreatedFromUtc,
            createdToUtc: searchModel.SearchCreatedToUtc,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var agencyIds = forms
            .Where(form => form.AgencyId.HasValue && form.AgencyId.Value > 0)
            .Select(form => form.AgencyId!.Value)
            .Distinct()
            .ToArray();
        var agencies = await _agencyService.GetAgenciesByIdsAsync(agencyIds);
        var agencyNames = agencies.ToDictionary(agency => agency.Id, agency => agency.Name);

        var model = await new DisciplinaryFormListModel().PrepareToGridAsync(searchModel, forms, () =>
        {
            return forms.SelectAwait(async form =>
            {
                var formModel = form.ToModel<DisciplinaryFormModel>();
                ApplyEntityFlagsToModel(form, formModel);
                formModel.AgencyName = form.AgencyId.HasValue &&
                    agencyNames.TryGetValue(form.AgencyId.Value, out var agencyName)
                    ? agencyName
                    : form.AgencyName;

                var createdOnUser = await _dateTimeHelper.ConvertToUserTimeAsync(form.CreatedOnUtc, DateTimeKind.Utc);
                formModel.CreatedOnPersian = new PersianDateTime(createdOnUser) { EnglishNumber = true }.ToString("yyyy/MM/dd");

                return formModel;
            });
        });

        return model;
    }

    public virtual async Task<DisciplinaryFormModel> PrepareDisciplinaryFormModelAsync(DisciplinaryFormModel model, DisciplinaryForm disciplinaryForm, bool excludeProperties = false)
    {
        model ??= new DisciplinaryFormModel();

        if (disciplinaryForm != null)
        {
            if (!excludeProperties)
            {
                model = disciplinaryForm.ToModel(model);
                ApplyEntityFlagsToModel(disciplinaryForm, model);
            }

            model.Id = disciplinaryForm.Id;
        }

        await PrepareEducationLevelsAsync(model);
        await PrepareAgenciesAsync(model);
        return model;
    }

    public virtual void ApplyModelFlagsToEntity(DisciplinaryFormModel model, DisciplinaryForm entity)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(entity);

        entity.ReferralReasons = DisciplinaryReferralReasonFlags.None;
        if (model.ReferralReasonRetravelConsumptionLessThanOneCcOt)
            entity.ReferralReasons |= DisciplinaryReferralReasonFlags.RetravelConsumptionLessThanOneCcOt;
        if (model.ReferralReasonWritingCd)
            entity.ReferralReasons |= DisciplinaryReferralReasonFlags.WritingCd;
        if (model.ReferralReasonOther)
            entity.ReferralReasons |= DisciplinaryReferralReasonFlags.Other;

        entity.EducationalResources = EducationalResourceFlags.None;
        if (model.EducationalResourceJahanbiniBooklet)
            entity.EducationalResources |= EducationalResourceFlags.JahanbiniBooklet;
        if (model.EducationalResourceSixtyDegreeBook)
            entity.EducationalResources |= EducationalResourceFlags.SixtyDegreeBook;
        if (model.EducationalResourceEshghBook)
            entity.EducationalResources |= EducationalResourceFlags.EshghBook;
        if (model.EducationalResourceTwelveArticles)
            entity.EducationalResources |= EducationalResourceFlags.TwelveArticles;

        entity.ServiceRoles = ServiceRoleFlags.None;
        if (model.ServiceRolePublications)
            entity.ServiceRoles |= ServiceRoleFlags.Publications;
        if (model.ServiceRoleOt)
            entity.ServiceRoles |= ServiceRoleFlags.Ot;
        if (model.ServiceRoleSite)
            entity.ServiceRoles |= ServiceRoleFlags.Site;
        if (model.ServiceRoleGuard)
            entity.ServiceRoles |= ServiceRoleFlags.Guard;
        if (model.ServiceRoleSecretary)
            entity.ServiceRoles |= ServiceRoleFlags.Secretary;
        if (model.ServiceRoleGuide)
            entity.ServiceRoles |= ServiceRoleFlags.Guide;
        if (model.ServiceRoleNewcomer)
            entity.ServiceRoles |= ServiceRoleFlags.Newcomer;
        if (model.ServiceRoleMarzban)
            entity.ServiceRoles |= ServiceRoleFlags.Marzban;

        entity.FamilyRelapseFactors = FamilyRelapseFactorFlags.None;
        if (model.FamilyRelapseFactorCigaretteUse)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.CigaretteUse;
        if (model.FamilyRelapseFactorSickness)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.Sickness;
        if (model.FamilyRelapseFactorFinancialAndMaritalIssues)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.FinancialAndMaritalIssues;
        if (model.FamilyRelapseFactorFamilyIssues)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.FamilyIssues;
        if (model.FamilyRelapseFactorEconomicIssues)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.EconomicIssues;
        if (model.FamilyRelapseFactorOverweight)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.Overweight;
        if (model.FamilyRelapseFactorOther)
            entity.FamilyRelapseFactors |= FamilyRelapseFactorFlags.Other;
    }

    public virtual void ApplyEntityFlagsToModel(DisciplinaryForm entity, DisciplinaryFormModel model)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(model);

        model.ReferralReasonRetravelConsumptionLessThanOneCcOt = entity.ReferralReasons.HasFlag(DisciplinaryReferralReasonFlags.RetravelConsumptionLessThanOneCcOt);
        model.ReferralReasonWritingCd = entity.ReferralReasons.HasFlag(DisciplinaryReferralReasonFlags.WritingCd);
        model.ReferralReasonOther = entity.ReferralReasons.HasFlag(DisciplinaryReferralReasonFlags.Other);

        model.EducationalResourceJahanbiniBooklet = entity.EducationalResources.HasFlag(EducationalResourceFlags.JahanbiniBooklet);
        model.EducationalResourceSixtyDegreeBook = entity.EducationalResources.HasFlag(EducationalResourceFlags.SixtyDegreeBook);
        model.EducationalResourceEshghBook = entity.EducationalResources.HasFlag(EducationalResourceFlags.EshghBook);
        model.EducationalResourceTwelveArticles = entity.EducationalResources.HasFlag(EducationalResourceFlags.TwelveArticles);

        model.ServiceRolePublications = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Publications);
        model.ServiceRoleOt = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Ot);
        model.ServiceRoleSite = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Site);
        model.ServiceRoleGuard = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Guard);
        model.ServiceRoleSecretary = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Secretary);
        model.ServiceRoleGuide = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Guide);
        model.ServiceRoleNewcomer = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Newcomer);
        model.ServiceRoleMarzban = entity.ServiceRoles.HasFlag(ServiceRoleFlags.Marzban);

        model.FamilyRelapseFactorCigaretteUse = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.CigaretteUse);
        model.FamilyRelapseFactorSickness = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.Sickness);
        model.FamilyRelapseFactorFinancialAndMaritalIssues = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.FinancialAndMaritalIssues);
        model.FamilyRelapseFactorFamilyIssues = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.FamilyIssues);
        model.FamilyRelapseFactorEconomicIssues = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.EconomicIssues);
        model.FamilyRelapseFactorOverweight = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.Overweight);
        model.FamilyRelapseFactorOther = entity.FamilyRelapseFactors.HasFlag(FamilyRelapseFactorFlags.Other);
    }

    #endregion

    #region Utilities

    protected virtual async Task PrepareEducationLevelsAsync(DisciplinaryFormModel model)
    {
        model.AvailableEducationLevels.Add(new SelectListItem
        {
            Value = string.Empty,
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });

        foreach (EducationLevel level in Enum.GetValues(typeof(EducationLevel)))
        {
            if (level == EducationLevel.Unknown)
                continue;

            model.AvailableEducationLevels.Add(new SelectListItem
            {
                Value = ((int)level).ToString(),
                Text = await _localizationService.GetLocalizedEnumAsync(level),
                Selected = model.EducationLevel.HasValue && model.EducationLevel.Value == (int)level
            });
        }
    }

    protected virtual async Task PrepareAgenciesAsync(DisciplinaryFormModel model)
    {
        model.AvailableAgencies.Add(new SelectListItem
        {
            Value = "0",
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select")
        });

        var agencies = await _agencyService.GetAllAgenciesAsync(pageIndex: 0, pageSize: int.MaxValue);
        foreach (var agency in agencies)
        {
            model.AvailableAgencies.Add(new SelectListItem
            {
                Value = agency.Id.ToString(),
                Text = agency.Name,
                Selected = model.AgencyId.HasValue && model.AgencyId.Value == agency.Id
            });
        }
    }

    #endregion
}
