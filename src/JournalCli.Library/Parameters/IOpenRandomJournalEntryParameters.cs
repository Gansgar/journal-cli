using System;

namespace JournalCli.Library.Parameters
{
    public interface IOpenRandomJournalEntryParameters : ILocationParameter 
    {
        DateTime? From { get; set; }
        DateTime To { get; set; }
        int Year { get; set; }
        string[] Tags { get; set; }
    }
}