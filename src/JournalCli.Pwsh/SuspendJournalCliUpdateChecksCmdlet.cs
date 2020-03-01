using System;
using System.Management.Automation;
using JetBrains.Annotations;
using JournalCli.Library;
using JournalCli.Library.Infrastructure;

namespace JournalCli.Pwsh
{
    [PublicAPI]
    [Cmdlet(VerbsLifecycle.Suspend, "JournalCliUpdateChecks")]
    public class SuspendJournalCliUpdateChecksCmdlet : CmdletBase
    {
        [Parameter] 
        [ValidateRange(1, 365)]
        public int Days { get; set; } = 45;

        protected override void ProcessRecord()
        {
            var encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            var settings = UserSettings.Load(encryptedStore);
            settings.NextUpdateCheck = DateTime.Now.AddDays(Days);
            settings.Save(encryptedStore);
        }
    }
}
