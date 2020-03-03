using System.Management.Automation;
using JetBrains.Annotations;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;

namespace JournalCli.Pwsh
{
    [PublicAPI]
    [Cmdlet(VerbsData.Save, "JournalSnapshot")]
    [Alias("sjs")]
    public class SaveJournalSnapshotCmdlet : JournalCmdletBase, ISaveJournalSnapshotParameters
    {
        [Parameter(Position = 0)]
        [ValidateLength(5, 60)]
        public string Message { get; set; }

        protected override void RunJournalCommand()
        {
            if (string.IsNullOrEmpty(Message))
                Commit(GitCommitType.Manual);
            else
                Commit(Message);
        }
    }
}
