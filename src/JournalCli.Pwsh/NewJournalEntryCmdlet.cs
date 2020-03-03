using System;
using System.Collections.Generic;
using System.Management.Automation;
using JetBrains.Annotations;
using JournalCli.Library.Controllers;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using NodaTime;

namespace JournalCli.Pwsh
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "JournalEntry")]
    [Alias("nj")]
    public class NewJournalEntryCmdlet : JournalCmdletBase, INewJournalEntryParameters
    {
        [Parameter]
        public int DateOffset { get; set; }

        [Parameter]
        public IEnumerable<string> Tags { get; set; }

        [Parameter]
        public string Readme { get; set; }

        [Parameter]
        public DateTime? Date { get; set; }

        protected override void RunJournalCommand()
        {
            var controller = new NewJournalEntryController(this);
            var entryDate = Date == null ? Today.PlusDays(DateOffset) : LocalDate.FromDateTime(Date.Value).PlusDays(DateOffset);

            if (controller.IsAfterMidnight())
            {
                var dayPrior = entryDate.Minus(Period.FromDays(1));
                var question = $"Did you mean to create an entry for '{dayPrior}' or '{entryDate}'?";
                var result = Choice("It's after midnight!", question, 0, dayPrior.DayOfWeek.ToChoiceString(), entryDate.DayOfWeek.ToChoiceString());
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
        }
    }
}