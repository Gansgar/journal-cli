using System;

namespace JournalCli.Library.Parameters
{
    public interface IGetJournalEntriesByTagParameters<T> : ILocationParameter
    {
        string[] Tags { get; set; }
        T IncludeBodies { get; set; }
        T All { get; set; }
        DateTime? From { get; set; }
        DateTime To { get; set; }
    }
}