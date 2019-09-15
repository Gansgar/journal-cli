﻿using System.Management.Automation;
using JournalCli.Core;
using JournalCli.Infrastructure;

namespace JournalCli.Cmdlets
{
    public abstract class JournalCmdletBase : CmdletBase
    {
        private readonly string _error = $"{nameof(RootDirectory)} was not provided and no default location exists. One or the other is required";

        [Parameter]
        public string RootDirectory { get; set; }

        protected override void ProcessRecord()
        {
            if (!string.IsNullOrEmpty(RootDirectory))
            {
                RootDirectory = ResolvePath(RootDirectory);
                return;
            }

            var encryptedStore = EncryptedStoreFactory.Create<UserSettings>();
            var settings = UserSettings.Load(encryptedStore);

            if (string.IsNullOrEmpty(settings.DefaultJournalRoot))
                throw new PSInvalidOperationException(_error);

            RootDirectory = settings.DefaultJournalRoot;
        }
    }
}