using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.ModelBinding.Binders;

namespace Nop.Web.Framework.Mvc.ModelBinding;

/// <summary>
/// Attribute to mark DateTime properties that should be displayed and edited as Persian (Jalali/Shamsi) dates
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PersianDateAttribute : Attribute, IModelNameProvider, IBinderTypeProviderMetadata
{
    /// <summary>
    /// Gets or sets the model name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the type of the model binder (PersianDateModelBinder)
    /// </summary>
    public Type? BinderType => typeof(PersianDateModelBinder);

    /// <summary>
    /// Gets the binding source
    /// </summary>
    public BindingSource? BindingSource => null;
}

