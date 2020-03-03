using System;

namespace JournalCli.Library.Parameters
{
    public interface IGetJournalFilesParameters : ILocationParameter
    {
        DateTime? From { get; set; }
        DateTime To { get; set; }
        string[] Tags { get; set; }
        string SortDirection { get; set; }
        int Limit { get; set; }
    }
}