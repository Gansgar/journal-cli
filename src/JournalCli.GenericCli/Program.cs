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
                    (NewJournalEntryOptions opts) => RunCommand(new NewJournalEntryCommand(opts)), 
                    (AddJournalEntryContentOptions opts) => RunCommand(new AddJournalEntryContentCommand(opts)),
                    errs => 1);
        }

        private static int RunCommand(CommandBase command)
        {
            try
            {
                return command.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                return 1;
            }
        }
    }
}
