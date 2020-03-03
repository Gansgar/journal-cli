using JournalCli.Library.Controllers;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using NodaTime;

namespace JournalCli.GenericCli
{
    public class NewJournalEntryCommand : CommandBase
    {
        private readonly INewJournalEntryParameters _opts;
        public NewJournalEntryCommand(INewJournalEntryParameters opts) : base(opts) => _opts = opts;

        public override int Run()
        {
            // Need to get location here
            var controller = new NewJournalEntryController(_opts);
            var entryDate = _opts.Date == null ? Today.PlusDays(_opts.DateOffset) : LocalDate.FromDateTime(_opts.Date.Value).PlusDays(_opts.DateOffset);

            if (controller.IsAfterMidnight())
            {
                var dayPrior = entryDate.Minus(Period.FromDays(1));
                var question = $"Did you mean to create an entry for '{dayPrior}' or '{entryDate}'?";
                var result = Choice("It's after midnight!", question, dayPrior.DayOfWeek.ToString(), entryDate.DayOfWeek.ToString());
                if (result == 0)
                    entryDate = dayPrior;
            }

            try
            {
                var warnings = controller.CreateNewJournalEntry(entryDate);

                foreach (var warning in warnings)
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