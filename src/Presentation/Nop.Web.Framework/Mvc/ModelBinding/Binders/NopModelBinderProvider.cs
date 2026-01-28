using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Nop.Web.Framework.Models;

namespace Nop.Web.Framework.Mvc.ModelBinding.Binders;

/// <summary>
/// Represents a model binder provider for specific properties
/// </summary>
public partial class NopModelBinderProvider : IModelBinderProvider
{
    IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.PropertyName == nameof(BaseNopModel.CustomProperties) && context.Metadata.ModelType == typeof(Dictionary<string, string>))
            return new CustomPropertiesModelBinder();

        if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string)) 
            return new StringModelBinder();

        // Check for PersianDate attribute on DateTime properties
        if ((context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?)) &&
            context.Metadata is DefaultModelMetadata defaultMetadata &&
            defaultMetadata.Attributes.Attributes.Any(a => a is PersianDateAttribute))
        {
            return new PersianDateModelBinder();
        }

        return null;
    }
}