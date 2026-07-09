using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers;

/// <summary>
/// Person service interface
/// </summary>
public partial interface IPersonService
{
    #region Persons

    /// <summary>
    /// Gets a person
    /// </summary>
    /// <param name="personId">Person identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the person
    /// </returns>
    Task<Person> GetPersonByIdAsync(int personId);

    /// <summary>
    /// Gets persons by identifiers
    /// </summary>
    /// <param name="personIds">Person identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the persons
    /// </returns>
    Task<IList<Person>> GetPersonsByIdsAsync(int[] personIds);

    /// <summary>
    /// Gets persons by card number
    /// </summary>
    /// <param name="cardNo">Card number</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the persons
    /// </returns>
    Task<IList<Person>> GetPersonsByCardNoAsync(string cardNo);

    /// <summary>
    /// Inserts a person
    /// </summary>
    /// <param name="person">Person</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertPersonAsync(Person person);

    /// <summary>
    /// Updates the person
    /// </summary>
    /// <param name="person">Person</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdatePersonAsync(Person person);

    /// <summary>
    /// Deletes the person
    /// </summary>
    /// <param name="person">Person</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeletePersonAsync(Person person);

    /// <summary>
    /// Checks whether a card number already exists on another person
    /// </summary>
    /// <param name="cardNo">Card number</param>
    /// <param name="exceptPersonId">Exclude person identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<bool> IsCardNoExistsAsync(string cardNo, int? exceptPersonId = null);

    #endregion
}
