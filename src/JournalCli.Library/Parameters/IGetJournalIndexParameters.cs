using System;

namespace JournalCli.Library.Parameters
{
    public interface IGetJournalIndexParameters<TSwitch> : ISwitchParameter<TSwitch>, ILocationParameter
    {
        string OrderBy { get; set; }
        string Direction { get; set; }
        TSwitch IncludeBodies { get; set; }
        DateTime? From { get; set; }
        DateTime To { get; set; }
    }
}