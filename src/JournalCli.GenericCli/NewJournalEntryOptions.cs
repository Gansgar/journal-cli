using System;
using System.Collections.Generic;
using CommandLine;
using JournalCli.Library.Parameters;

namespace JournalCli.GenericCli
{
    [Verb("new", HelpText = "Create a new journal entry")]
    public class NewJournalEntryOptions : JournalOptionsBase, INewJournalEntryParameters
    {
        [Option('o', "date-offset")]
        public int DateOffset { get; set; }

        [Option('t', "tags")]
        public IEnumerable<string> Tags { get; set; }

        [Option('r', "readme")]
        public string Readme { get; set; }

        [Option('d', "date")]
        public DateTime? Date { get; set; }
    }
}