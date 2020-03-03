using System;
using CommandLine;
using JournalCli.Library.Parameters;

namespace JournalCli.GenericCli
{
    [Verb("add", HelpText = "Add journal entry content")]
    public class AddJournalEntryContentOptions : JournalOptionsBase, IAddJournalEntryContentParameters
    {
        public DateTime Date { get; set; }

        public int DateOffset { get; set; }

        public string Header { get; set; }

        public string[] Body { get; set; }

        public string[] Tags { get; set; }
    }
}