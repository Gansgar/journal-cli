using System.Collections.Generic;
using JournalCli.Library.Infrastructure;

namespace JournalCli.Library
{
    public class MetaJournalEntry : IJournalEntry
    {
        private readonly IJournalReader _reader;

        public MetaJournalEntry(IJournalReader reader)
        {
            _reader = reader;
            Headers = reader.Headers;
            Tags = reader.FrontMatter.Tags;
            EntryName = reader.EntryName;
        }

        public string EntryName { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public IReadOnlyCollection<string> Headers { get; }
        public override string ToString() => EntryName;
        public IJournalReader GetReader() => _reader;
    }
}