using System.Management.Automation;
using JetBrains.Annotations;
using JournalCli.Library;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;

namespace JournalCli.Pwsh
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "JournalDefaultLocation")]
    [Alias("Set-DefaultJournalLocation")]
    public class SetJournalDefaultLocationCmdlet : CmdletBase, ISetJournalDefaultLocationParameters
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Location { get; set; }

        protected override void ProcessRecord()
        {
            if (MyInvocation.InvocationName == "Set-DefaultJournalLocation")
                WriteWarning("'Set-DefaultJournalLocation' is obsolete and will be removed in a future release. Use 'Set-JournalDefaultLocation' instead.");

            var encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            var settings = UserSettings.Load(encryptedStore);
            settings.DefaultJournalRoot = Location;
            settings.Save(encryptedStore);
        }
    }
}
