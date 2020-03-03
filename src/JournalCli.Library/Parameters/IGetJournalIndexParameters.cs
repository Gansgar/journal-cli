using System;

namespace JournalCli.Library.Parameters
{
    public interface IGetJournalIndexParameters<T> : ILocationParameter
    {
        string OrderBy { get; set; }
        string Direction { get; set; }
        T IncludeBodies { get; set; }
        DateTime? From { get; set; }
        DateTime To { get; set; }
    }
}