using System;
using System.Collections.Generic;
using System.Text;
using JournalCli.Library.Controllers;
using JournalCli.Library.Parameters;

namespace JournalCli.GenericCli
{
    public class GetJournalIndexCommand : JournalCommandBase
    {
        private readonly IGetJournalIndexParameters<bool> _parameters;
        public GetJournalIndexCommand(IGetJournalIndexParameters<bool> parameters) : base(parameters) => _parameters = parameters;

        public override int Run()
        {
            var controller = new GetJournalIndexController<bool>(_parameters);
            throw new NotImplementedException();
        }
    }
}
