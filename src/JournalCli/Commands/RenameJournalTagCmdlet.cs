﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using JetBrains.Annotations;

namespace JournalCli.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Rename, "JournalTags", ConfirmImpact = ConfirmImpact.High)]
    public class RenameJournalTagCmdlet : JournalCmdletBase
    {
        [Parameter]
        public SwitchParameter DryRun { get; set; }

        [Parameter]
        public SwitchParameter NoBackups { get; set; }

        [Parameter(Mandatory = true)]
        public string OldName { get; set; }

        [Parameter(Mandatory = true)]
        public string NewName { get; set; }

        protected override void ProcessRecord()
        {
            if (!DryRun && NoBackups)
            {
                var warning = "***** Hey, you! *****\r\n" +
                    $"This function will rename all '{OldName}' tags to '{NewName}' and you've disabled local backups. " + 
                    "Consider creating a full backup before proceeding, by running 'Backup-Journal'.";
                WriteHost(warning, ConsoleColor.Red);
            }

            if (!DryRun && !ShouldContinue("Do you want to continue?", $"Renaming '{OldName}' tags to '{NewName}'..."))
                return;

            var index = Journal.CreateIndex(GetResolvedRootDirectory(), false);
            var journalEntries = index.SingleOrDefault(x => x.Tag == OldName);

            if (journalEntries == null)
                throw new PSInvalidOperationException($"No entries found with the tag '{OldName}'");

            if (DryRun)
            {
                const string header = "Tags in these file(s) would be renamed:";
                WriteHost(header, ConsoleColor.Cyan);
                WriteHost(new string('=', header.Length), ConsoleColor.Cyan);
            }
            else
            {
                const string header = "Tags in these file(s) have been renamed:";
                WriteHost(header, ConsoleColor.Red);
                WriteHost(new string('=', header.Length), ConsoleColor.Red);
            }

            var counter = 1;
            foreach (var journalEntry in journalEntries.Entries)
            {
                if (DryRun)
                {
                    WriteHost($"{counter++.ToString().PadLeft(3)}) {journalEntry.FilePath}", ConsoleColor.Cyan);
                    continue;
                }

                var file = new JournalEntryFile(journalEntry.FilePath);
                var currentTags = file.GetTags().ToList();
                var oldItemIndex = currentTags.IndexOf(OldName);
                currentTags[oldItemIndex] = NewName;

                file.WriteTags(currentTags, !NoBackups);
                WriteHost($"{counter++.ToString().PadLeft(3)}) {journalEntry.FilePath}", ConsoleColor.Red);
            }
        }
    }
}
