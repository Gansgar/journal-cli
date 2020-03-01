using System.Collections.Generic;
using JournalCli.Pwsh.Infrastructure;

namespace JournalCli.Pwsh.Core
{
    public interface IJournalEntry
    {
        string EntryName { get; }
        IReadOnlyCollection<string> Tags { get; }
        string ToString();
        IJournalReader GetReader();
    }
}