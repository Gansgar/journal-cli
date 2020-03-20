using System.Linq;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using NodaTime;

namespace JournalCli.Library.Controllers
{
    public class NewJournalEntryController : JournalController
    {
        private readonly INewJournalEntryParameters _parameters;
        private readonly LocalDate _entryDate;

        public NewJournalEntryController(INewJournalEntryParameters parameters, LocalDate entryDate) : base(parameters)
        {
            _parameters = parameters;
            _entryDate = entryDate;
        }

        public override void Run()
        {
            var journal = OpenJournal();
            Commit(GitCommitType.PreNewJournalEntry);
            journal.CreateNewEntry(_entryDate, _parameters.Tags.ToArray(), _parameters.Readme);
            Commit(GitCommitType.PostNewJournalEntry);
        }
    }
}