using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Customers;

public partial class DisciplinaryFormValidator : BaseNopValidator<DisciplinaryFormModel>
{
    public DisciplinaryFormValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.PersonName)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.DisciplinaryForms.Fields.PersonName.Required"));

        SetDatabaseValidationRules<Nop.Core.Domain.Customers.DisciplinaryForm>();
    }
}
