using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Customers;

public partial class ClinicValidator : BaseNopValidator<ClinicModel>
{
    public ClinicValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Clinics.Fields.Name.Required"));

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Configuration.Clinics.Fields.City.Required"));

        SetDatabaseValidationRules<Clinic>();
    }
}

