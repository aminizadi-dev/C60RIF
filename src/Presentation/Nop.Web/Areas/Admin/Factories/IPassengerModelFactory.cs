using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the passenger model factory
/// </summary>
public partial interface IPassengerModelFactory
{
    /// <summary>
    /// Prepare passenger search model
    /// </summary>
    /// <param name="searchModel">Passenger search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger search model
    /// </returns>
    Task<PassengerSearchModel> PreparePassengerSearchModelAsync(PassengerSearchModel searchModel);

    /// <summary>
    /// Prepare paged passenger list model
    /// </summary>
    /// <param name="searchModel">Passenger search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger list model
    /// </returns>
    Task<PassengerListModel> PreparePassengerListModelAsync(PassengerSearchModel searchModel);

    /// <summary>
    /// Prepare passenger model
    /// </summary>
    /// <param name="model">Passenger model</param>
    /// <param name="passenger">Passenger</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the passenger model
    /// </returns>
    Task<PassengerModel> PreparePassengerModelAsync(PassengerModel model, Passenger passenger, bool excludeProperties = false);
}

