using System;
using JournalCli.Library.Parameters;

namespace JournalCli.GenericCli
{
    public class GetJournalIndexOptions : JournalOptionsBase, IGetJournalIndexParameters<bool>
    {
        public string OrderBy { get; set; }
        public string Direction { get; set; }
        public bool IncludeBodies { get; set; }
        public DateTime? From { get; set; }
        public DateTime To { get; set; }
        bool ISwitchParameter<bool>.ToBool(bool value) => IncludeBodies;
    }
}