using System;

namespace JournalCli.Library.Parameters
{
    public interface IAddJournalEntryContentParameters : ILocationParameter
    {
        DateTime Date { get; set; }
        int DateOffset { get; set; }
        string Header { get; set; }
        string[] Body { get; set; }
        string[] Tags { get; set; }
    }
}