using System.Management.Automation;
using JetBrains.Annotations;
using JournalCli.Library;
using JournalCli.Library.Infrastructure;

namespace JournalCli.Pwsh
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "JournalDefaultLocation")]
    [OutputType(typeof(string))]
    [Alias("Get-DefaultJournalLocation")]
    public class GetJournalDefaultLocationCmdlet : CmdletBase
    {
        protected override void ProcessRecord()
        {
            if (MyInvocation.InvocationName == "Get-DefaultJournalLocation")
                WriteWarning("'Get-DefaultJournalLocation' is obsolete and will be removed in a future release. Use 'Get-JournalDefaultLocation' instead.");

            var encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            var settings = UserSettings.Load(encryptedStore);
            WriteObject(settings.DefaultJournalRoot);
        }
    }
}