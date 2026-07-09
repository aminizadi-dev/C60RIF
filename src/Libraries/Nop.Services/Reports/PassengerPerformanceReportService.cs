using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Services.Customers;

namespace Nop.Services.Reports;

/// <summary>
/// Passenger performance report service
/// </summary>
public partial class PassengerPerformanceReportService : IPassengerPerformanceReportService
{
    protected readonly ICustomerService _customerService;
    protected readonly IRepository<RecoveryForm> _passengerRepository;

    public PassengerPerformanceReportService(
        ICustomerService customerService,
        IRepository<RecoveryForm> passengerRepository)
    {
        _customerService = customerService;
        _passengerRepository = passengerRepository;
    }

    /// <inheritdoc />
    public virtual async Task<PassengerPerformanceReportSummary> GetSummaryAsync()
    {
        var (dayStart, dayEnd) = PersianDateRangeHelper.GetPersianDayRangeUtc();
        var (weekStart, weekEnd) = PersianDateRangeHelper.GetPersianWeekRangeUtc();
        var (monthStart, monthEnd) = PersianDateRangeHelper.GetPersianMonthRangeUtc();
        var (yearStart, yearEnd) = PersianDateRangeHelper.GetPersianYearRangeUtc();
        var persianYear = new PersianDateTime(DateTime.Now).Year.ToString();

        var query = _passengerRepository.Table;

        return new PassengerPerformanceReportSummary
        {
            CountToday = await CountInRangeAsync(query, dayStart, dayEnd),
            CountThisWeek = await CountInRangeAsync(query, weekStart, weekEnd),
            CountThisMonth = await CountInRangeAsync(query, monthStart, monthEnd),
            CountThisYear = await CountInRangeAsync(query, yearStart, yearEnd),
            CountAllTime = await query.CountAsync(),
            CountUnattributed = await query.CountAsync(p => p.CreatedByCustomerId == null),
            CurrentPersianYear = persianYear
        };
    }

    /// <inheritdoc />
    public virtual async Task<IList<PassengerPerformanceReportLine>> GetUserReportLinesAsync()
    {
        var (dayStart, dayEnd) = PersianDateRangeHelper.GetPersianDayRangeUtc();
        var (weekStart, weekEnd) = PersianDateRangeHelper.GetPersianWeekRangeUtc();
        var (monthStart, monthEnd) = PersianDateRangeHelper.GetPersianMonthRangeUtc();
        var (yearStart, yearEnd) = PersianDateRangeHelper.GetPersianYearRangeUtc();

        var customers = await GetPerformanceReportCustomersAsync();
        var query = _passengerRepository.Table;

        var todayByUser = await CountGroupedByCreatorAsync(query, dayStart, dayEnd);
        var weekByUser = await CountGroupedByCreatorAsync(query, weekStart, weekEnd);
        var monthByUser = await CountGroupedByCreatorAsync(query, monthStart, monthEnd);
        var yearByUser = await CountGroupedByCreatorAsync(query, yearStart, yearEnd);
        var allTimeByUser = await CountGroupedByCreatorAsync(query, null, null);

        var lines = new List<PassengerPerformanceReportLine>();

        foreach (var customer in customers.OrderBy(c => c.Email))
        {
            lines.Add(new PassengerPerformanceReportLine
            {
                CustomerId = customer.Id,
                FullName = await _customerService.GetCustomerFullNameAsync(customer),
                Email = customer.Email,
                CountToday = todayByUser.GetValueOrDefault(customer.Id),
                CountThisWeek = weekByUser.GetValueOrDefault(customer.Id),
                CountThisMonth = monthByUser.GetValueOrDefault(customer.Id),
                CountThisYear = yearByUser.GetValueOrDefault(customer.Id),
                CountAllTime = allTimeByUser.GetValueOrDefault(customer.Id)
            });
        }

        var summary = await GetSummaryAsync();
        lines.Add(new PassengerPerformanceReportLine
        {
            CustomerId = null,
            FullName = null,
            Email = null,
            IsSystemTotal = true,
            CountToday = summary.CountToday,
            CountThisWeek = summary.CountThisWeek,
            CountThisMonth = summary.CountThisMonth,
            CountThisYear = summary.CountThisYear,
            CountAllTime = summary.CountAllTime,
            CountUnattributed = summary.CountUnattributed
        });

        return lines;
    }

    protected virtual async Task<IList<Customer>> GetPerformanceReportCustomersAsync()
    {
        var customers = await _customerService.GetAllCustomersAsync(
            customerRoleIds: [NopCustomerDefaults.PassengerPerformanceReportRoleId],
            pageIndex: 0,
            pageSize: int.MaxValue);

        return customers
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .Where(c => !c.Deleted && c.Active)
            .ToList();
    }

    protected virtual async Task<Dictionary<int, int>> CountGroupedByCreatorAsync(
        IQueryable<RecoveryForm> query,
        DateTime? rangeStartUtc,
        DateTime? rangeEndUtc)
    {
        var filtered = ApplyRange(query, rangeStartUtc, rangeEndUtc)
            .Where(p => p.CreatedByCustomerId != null);

        var groups = await filtered
            .GroupBy(p => p.CreatedByCustomerId.Value)
            .Select(g => new { CustomerId = g.Key, Count = g.Count() })
            .ToListAsync();

        return groups.ToDictionary(x => x.CustomerId, x => x.Count);
    }

    protected virtual async Task<int> CountInRangeAsync(
        IQueryable<RecoveryForm> query,
        DateTime rangeStartUtc,
        DateTime rangeEndUtc)
    {
        return await ApplyRange(query, rangeStartUtc, rangeEndUtc).CountAsync();
    }

    protected static IQueryable<RecoveryForm> ApplyRange(
        IQueryable<RecoveryForm> query,
        DateTime? rangeStartUtc,
        DateTime? rangeEndUtc)
    {
        if (rangeStartUtc.HasValue)
            query = query.Where(p => p.CreatedOnUtc >= rangeStartUtc.Value);

        if (rangeEndUtc.HasValue)
            query = query.Where(p => p.CreatedOnUtc < rangeEndUtc.Value);

        return query;
    }
}
