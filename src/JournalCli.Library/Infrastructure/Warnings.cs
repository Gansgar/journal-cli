using System.Collections;
using System.Collections.Generic;

namespace JournalCli.Library.Infrastructure
{
    public class Warnings : IEnumerable<string>
    {
        private readonly List<string> _warnings = new List<string>();

        public void Add(string warning) => _warnings.Add(warning);

        public void Add(Warnings warnings) => _warnings.AddRange(warnings);

        public IEnumerator<string> GetEnumerator() => _warnings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}