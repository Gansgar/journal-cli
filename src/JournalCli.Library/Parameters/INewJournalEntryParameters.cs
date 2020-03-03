using System;
using System.Collections.Generic;

namespace JournalCli.Library.Parameters
{
    public interface INewJournalEntryParameters : ILocationParameter
    {
        int DateOffset { get; set; }
        IEnumerable<string> Tags { get; set; }
        string Readme { get; set; }
        DateTime? Date { get; set; }
    }
}