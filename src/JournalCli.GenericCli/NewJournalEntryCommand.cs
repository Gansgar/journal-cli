using JetBrains.Annotations;
using JournalCli.Library.Controllers;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using NodaTime;

namespace JournalCli.GenericCli
{
    [PublicAPI]
    public class NewJournalEntryCommand : JournalCommandBase
    {
        private readonly INewJournalEntryParameters _opts;
        public NewJournalEntryCommand(INewJournalEntryParameters opts) : base(opts) => _opts = opts;

        public override int Run()
        {
            var entryDate = _opts.Date == null ? Today.PlusDays(_opts.DateOffset) : LocalDate.FromDateTime(_opts.Date.Value).PlusDays(_opts.DateOffset);

            if (Now.IsAfterMidnight())
            {
                var dayPrior = entryDate.Minus(Period.FromDays(1));
                var question = $"Did you mean to create an entry for '{dayPrior}' or '{entryDate}'?";
                var result = Choice("It's after midnight!", question, dayPrior.DayOfWeek.ToString(), entryDate.DayOfWeek.ToString());
                if (result == 0)
                    entryDate = dayPrior;
            }

            var controller = new NewJournalEntryController(_opts, entryDate);

            try
            {
                controller.Run();

                foreach (var warning in controller.Warnings)
                    WriteWarning(warning);
            }
            catch (JournalEntryAlreadyExistsException e)
            {
                var question = $"An entry for {entryDate} already exists. Do you want to open it instead?";
                if (YesOrNo(question))
                {
                    controller.OpenJournalEntry(e.EntryFilePath);
                }
            }

            return 0;
        }
    }
}