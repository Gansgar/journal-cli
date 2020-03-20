using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;

namespace JournalCli.Library.Controllers
{
    public class GetJournalIndexController<T> : JournalResultController
    {
        private readonly IGetJournalIndexParameters<T> _parameters;

        public GetJournalIndexController(IGetJournalIndexParameters<T> parameters) : base(parameters) => _parameters = parameters;

        public override U Run<U>()
            where U : class, IJournalEntry
        {
            var dateRange = DateRange.GetRangeOrNull(_parameters.From, _parameters.To);
            var journal = OpenJournal();

            if (IncludeBodies)
            {
                return CreateIndex<CompleteJournalEntry>(journal, dateRange);
            }
            else
            {
                return CreateIndex<MetaJournalEntry>(journal, dateRange);
            }
        }

        public IEnumerable<JournalIndexEntry<IJournalEntry>> Results { get; private set; }

        private bool IncludeBodies => _parameters.ToBool(_parameters.IncludeBodies);

        private IEnumerable<JournalIndexEntry<U>> CreateIndex<U>(Journal journal, DateRange dateRange)
            where U : class, IJournalEntry
        {
            var index = journal.CreateIndex<U>(dateRange);

            switch (_parameters.OrderBy)
            {
                case var order when order == "Name" && _parameters.Direction == "Ascending":
                    return index.OrderBy(x => x.Tag);

                case var order when order == "Name" && _parameters.Direction == "Descending":
                    return index.OrderByDescending(x => x.Tag);

                case var order when order == "Count" && _parameters.Direction == "Descending":
                    return index.OrderByDescending(x => x.Entries.Count);

                case var order when order == "Count" && _parameters.Direction == "Ascending":
                    return index.OrderBy(x => x.Entries.Count);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
