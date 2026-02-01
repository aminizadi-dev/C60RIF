using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Customers;

public partial class AgencyValidator : BaseNopValidator<AgencyModel>
{
    public AgencyValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Cities.Agencies.Fields.Name.Required"));
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Cities.Agencies.Fields.City.Required"));

        SetDatabaseValidationRules<Agency>();
    }
}

