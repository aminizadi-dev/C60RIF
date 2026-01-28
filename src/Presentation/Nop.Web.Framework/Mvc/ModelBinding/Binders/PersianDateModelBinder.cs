using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nop.Core;

namespace Nop.Web.Framework.Mvc.ModelBinding.Binders;

/// <summary>
/// Represents model binder for Persian date (Jalali/Shamsi) properties
/// Converts Persian date strings (e.g., "1403/10/10") to Gregorian DateTime
/// </summary>
public partial class PersianDateModelBinder : IModelBinder
{
    Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            // For nullable DateTime, null is valid
            if (bindingContext.ModelType == typeof(DateTime?))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            return Task.CompletedTask;
        }

        DateTime? result = null;

        // Try to parse as Gregorian date first (yyyy-MM-dd format from hidden field)
        if (DateTime.TryParse(value, out var gregorianDate))
        {
            result = gregorianDate;
        }
        // Try to parse as Persian date (1403/10/10 format)
        else if (PersianDateTime.TryParse(value, out var persianDateTime))
        {
            result = persianDateTime.ToDateTime();
        }

        if (result.HasValue)
        {
            if (bindingContext.ModelType == typeof(DateTime))
            {
                bindingContext.Result = ModelBindingResult.Success(result.Value);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
        }
        else
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "فرمت تاریخ نامعتبر است");
        }

        return Task.CompletedTask;
    }
}

