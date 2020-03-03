using System;

namespace JournalCli.Library.Parameters
{
    public interface INewCompiledJournalEntryParameters<T> : ILocationParameter
    {
        DateTime? From { get; set; }
        DateTime To { get; set; }
        string[] Tags { get; set; }
        T AllTags { get; set; }
        T Force { get; set; }
    }
}