using System.Linq;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using NodaTime;

namespace JournalCli.Library.Controllers
{
    public class NewJournalEntryController : ControllerBase
    {
        private readonly INewJournalEntryParameters _parameters;

        public NewJournalEntryController(INewJournalEntryParameters parameters) : base(parameters) => _parameters = parameters;

        public Warnings CreateNewJournalEntry(LocalDate entryDate)
        {
            var journal = OpenJournal(_parameters.Location);
            var warnings1 = Commit(GitCommitType.PreNewJournalEntry);
            journal.CreateNewEntry(entryDate, _parameters.Tags.ToArray(), _parameters.Readme);
            var warnings2 = Commit(GitCommitType.PostNewJournalEntry);

            warnings1.Add(warnings2);
            return warnings1;
        }
    }
}