﻿using System;
using System.IO.Abstractions;
using System.Management.Automation;
using ICSharpCode.SharpZipLib.Zip;
using JetBrains.Annotations;
using JournalCli.Core;
using JournalCli.Infrastructure;

namespace JournalCli.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Backup, "Journal")]
    [Alias("bj")]
    public class BackupJournalCmdlet : JournalCmdletBase
    {
        [Parameter]
        public string BackupLocation { get; set; }

        // TODO: Collect this in a more secure way
        // https://github.com/refactorsaurusrex/journal-cli/issues/16
        [Parameter]
        public string Password { get; set; }

        [Parameter]
        public SwitchParameter SaveParameters { get; set; }

        protected override void RunJournalCommand()
        {
            var fileSystem = new FileSystem();
            var encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            var settings = UserSettings.Load(encryptedStore);

            if (string.IsNullOrWhiteSpace(BackupLocation))
            {
                if (string.IsNullOrWhiteSpace(settings.BackupLocation))
                    throw new PSInvalidOperationException("Backup location not provided and no location was previously saved.");
            }
            else
            {
                settings.BackupLocation = ResolvePath(BackupLocation);
            }

            if (!string.IsNullOrWhiteSpace(Password))
                settings.BackupPassword = Password;

            if (SaveParameters)
                encryptedStore.Save(settings);

            if (!fileSystem.Directory.Exists(settings.BackupLocation))
                fileSystem.Directory.CreateDirectory(settings.BackupLocation);

            var fileName = $"{DateTime.Now:yyyy.MM.dd.H.mm.ss.FFF}.zip";
            var destinationPath = fileSystem.Path.Combine(settings.BackupLocation, fileName);

            var zip = new FastZip { CreateEmptyDirectories = true, Password = settings.BackupPassword };
            zip.CreateZip(destinationPath, Location, true, null);
        }
    }
}
