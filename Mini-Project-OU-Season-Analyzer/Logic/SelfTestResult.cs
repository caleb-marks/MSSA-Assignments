namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Self-check results
internal sealed record SelfTestResult(string Report, int Passed, int Total)
{
    // Check complete success
    public bool AllPassed => Passed == Total;
}
