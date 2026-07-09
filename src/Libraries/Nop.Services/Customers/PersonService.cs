using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data;

namespace Nop.Services.Customers;

/// <summary>
/// Person service
/// </summary>
public partial class PersonService : IPersonService
{
    #region Fields

    protected readonly IRepository<Person> _personRepository;

    #endregion

    #region Ctor

    public PersonService(IRepository<Person> personRepository)
    {
        _personRepository = personRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a person
    /// </summary>
    public virtual async Task<Person> GetPersonByIdAsync(int personId)
    {
        if (personId <= 0)
            return null;

        return await _personRepository.GetByIdAsync(personId);
    }

    /// <summary>
    /// Gets persons by identifiers
    /// </summary>
    public virtual async Task<IList<Person>> GetPersonsByIdsAsync(int[] personIds)
    {
        if (personIds == null || personIds.Length == 0)
            return new List<Person>();

        var persons = await _personRepository.GetAllAsync(query =>
            query.Where(p => personIds.Contains(p.Id)));

        return persons.ToList();
    }

    /// <summary>
    /// Gets persons by card number
    /// </summary>
    public virtual async Task<IList<Person>> GetPersonsByCardNoAsync(string cardNo)
    {
        if (string.IsNullOrWhiteSpace(cardNo))
            return new List<Person>();

        cardNo = DigitHelper.ToEnglishDigits(cardNo.Trim());

        var persons = await _personRepository.GetAllAsync(query =>
            query.Where(p => p.CardNo == cardNo));

        return persons.ToList();
    }

    /// <summary>
    /// Inserts a person
    /// </summary>
    public virtual async Task InsertPersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        NormalizePerson(person);

        if (person.CreatedOnUtc == default)
            person.CreatedOnUtc = DateTime.UtcNow;

        await _personRepository.InsertAsync(person);
    }

    /// <summary>
    /// Updates the person
    /// </summary>
    public virtual async Task UpdatePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        NormalizePerson(person);

        await _personRepository.UpdateAsync(person);
    }

    /// <summary>
    /// Deletes the person
    /// </summary>
    public virtual async Task DeletePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        await _personRepository.DeleteAsync(person);
    }

    /// <summary>
    /// Checks whether a card number already exists on another person
    /// </summary>
    public virtual async Task<bool> IsCardNoExistsAsync(string cardNo, int? exceptPersonId = null)
    {
        if (string.IsNullOrWhiteSpace(cardNo))
            return false;

        cardNo = DigitHelper.ToEnglishDigits(cardNo.Trim());

        var query = _personRepository.Table.Where(p => p.CardNo == cardNo);

        if (exceptPersonId.HasValue)
            query = query.Where(p => p.Id != exceptPersonId.Value);

        return await query.AnyAsync();
    }

    #endregion

    #region Utilities

    protected virtual void NormalizePerson(Person person)
    {
        person.FirstName = person.FirstName?.Trim();
        person.LastName = person.LastName?.Trim();

        person.CardNo = string.IsNullOrWhiteSpace(person.CardNo)
            ? null
            : DigitHelper.ToEnglishDigits(person.CardNo.Trim());

        person.MobileNumber = string.IsNullOrWhiteSpace(person.MobileNumber)
            ? null
            : DigitHelper.ToEnglishDigits(person.MobileNumber.Trim());
    }

    #endregion
}
