using System;

namespace JournalCli.Library.Parameters
{
    public interface IOpenJournalEntryParameters<T> : ILocationParameter
    {
        IJournalEntry Entry { get; set; }
        T Last { get; set; }
        string EntryName { get; set; }
        DateTime Date { get; set; }
        int DateOffset { get; set; }
        T Wait { get; set; }
    }
}