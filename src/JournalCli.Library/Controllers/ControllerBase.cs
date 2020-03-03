using System;
using System.IO.Abstractions;
using JournalCli.Library.Infrastructure;
using JournalCli.Library.Parameters;
using Git = LibGit2Sharp;

namespace JournalCli.Library.Controllers
{
    public abstract class ControllerBase
    {
        private readonly ILocationParameter _locationParameter;
#if !DEBUG
        private bool _beenWarned;

        private const string MissingGitBinaryWarning = "You're missing a native binary that's required to enable git integration. " +
            "Click here for more information:\r\n\r\nhttps://journalcli.me/docs/faq#i-got-a-missing-git-binary-warning-whats-that-about\r\n";
#endif
        protected ControllerBase(ILocationParameter locationParameter) => _locationParameter = locationParameter;

        public bool IsAfterMidnight()
        {
            var hour = Now.Time().Hour;
            return hour >= 0 && hour <= 4;
        }

        public void OpenJournalEntry(string filePath) => SystemProcess.Start(filePath);

        protected Warnings Commit(GitCommitType commitType)
        {
            var warnings = new Warnings();
#if !DEBUG
            try
            {
                ValidateGitRepo();
                var message = GitCommitMessage.Get(commitType);
                CommitCore(message);
            }
            catch (TypeInitializationException e) when (e.InnerException is DllNotFoundException)
            {
                if (!_beenWarned)
                {
                    warnings.Add(MissingGitBinaryWarning);
                    _beenWarned = true;
                }
            }
#endif
            return warnings;
        }

        private protected Journal OpenJournal(string location)
        {
            var fileSystem = new FileSystem();
            var ioFactory = new JournalReaderWriterFactory(fileSystem, location);
            var markdownFiles = new MarkdownFiles(fileSystem, location);
            return Journal.Open(ioFactory, markdownFiles, SystemProcess);
        }

        private protected Warnings Commit(string message)
        {
            var warnings = new Warnings();
#if !DEBUG
            try
            {
                ValidateGitRepo();
                CommitCore(message);
            }
            catch (TypeInitializationException e) when (e.InnerException is DllNotFoundException)
            {
                if (!_beenWarned)
                {
                    warnings.Add(MissingGitBinaryWarning);
                    _beenWarned = true;
                }
            }
#endif
            return warnings;
        }

        private protected ISystemProcess SystemProcess { get; } = SystemProcessFactory.Create();

        private void CommitCore(string message)
        {
            using (var repo = new Git.Repository(_locationParameter.Location))
            {
                var statusOptions = new Git.StatusOptions
                {
                    DetectRenamesInIndex = true,
                    DetectRenamesInWorkDir = true,
                    IncludeIgnored = false,
                    IncludeUntracked = true,
                    RecurseUntrackedDirs = true,
                    RecurseIgnoredDirs = false
                };

                if (!repo.RetrieveStatus(statusOptions).IsDirty)
                    return;

                Git.Commands.Stage(repo, "*");

                var author = new Git.Signature("JournalCli", "@journalCli", DateTime.Now);
                var committer = author;

                var options = new Git.CommitOptions { PrettifyMessage = true };
                var commit = repo.Commit(message, author, committer, options);
            }
        }

        private void ValidateGitRepo()
        {
            if (Git.Repository.IsValid(_locationParameter.Location))
                return;

            Git.Repository.Init(_locationParameter.Location);
            using (var repo = new Git.Repository(_locationParameter.Location))
            {
                Git.Commands.Stage(repo, "*");

                var author = new Git.Signature("JournalCli", "@journalCli", DateTime.Now);
                var committer = author;

                var options = new Git.CommitOptions { PrettifyMessage = true };
                var commit = repo.Commit("Initial commit", author, committer, options);
            }
        }
    }
}
