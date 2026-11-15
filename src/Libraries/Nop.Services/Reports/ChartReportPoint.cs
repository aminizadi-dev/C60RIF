namespace Nop.Services.Reports;

/// <summary>
/// One bar on a passenger chart report
/// </summary>
public partial class ChartReportPoint
{
    public string Label { get; set; }

    public int Count { get; set; }
}
