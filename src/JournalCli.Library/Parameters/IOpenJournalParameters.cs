using System;

namespace JournalCli.Library.Parameters
{
    public interface IOpenJournalParameters : ILocationParameter
    {
        string To { get; set; }
        DateTime Date { get; set; }
    }
}