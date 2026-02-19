using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Customers;

public partial class PassengerValidator : BaseNopValidator<PassengerModel>
{
    public PassengerValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.PersonName)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.PersonName.Required"));

        RuleFor(x => x.RecoveryNo)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.RecoveryNo.Required"));

        RuleFor(x => x.GuideNameAndLegionNo)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.GuideNameAndLegionNo.Required"));

        RuleFor(x => x.TravelStartDateUtc)
            .NotNull()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.TravelStartDate.Required"));

        RuleFor(x => x.TravelEndDateUtc)
            .NotNull()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.TravelEndDate.Required"));

        RuleFor(x => x.AntiX1)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.AntiX1.Required"));

        RuleFor(x => x.AgencyId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.Agency.Required"));

        RuleFor(x => x.BirthYear)
            .InclusiveBetween(1300, 1400)
            .When(x => x.BirthYear.HasValue)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Passengers.Fields.BirthYear.Range"));

        SetDatabaseValidationRules<Passenger>();
    }
}

