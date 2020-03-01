using System.Collections.Generic;
using JournalCli.Library.Infrastructure;

namespace JournalCli.Library
{
    public interface IJournalEntry
    {
        string EntryName { get; }
        IReadOnlyCollection<string> Tags { get; }
        string ToString();
        IJournalReader GetReader();
    }
}