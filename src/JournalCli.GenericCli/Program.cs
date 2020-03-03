using System;
using CommandLine;

namespace JournalCli.GenericCli
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return Parser.Default.ParseArguments(args, 
                    typeof(NewJournalEntryOptions), 
                    typeof(AddJournalEntryContentOptions))
                .MapResult(
                    (NewJournalEntryOptions opts) => new NewJournalEntryCommand(opts).Run(), 
                    (AddJournalEntryContentOptions opts) => RunAddJournalEntryContentCommand(opts),
                    errs => 1);
        }

        private static int RunAddJournalEntryContentCommand(AddJournalEntryContentOptions opts)
        {
            throw new NotImplementedException();
        }
    }


}
