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

        public Warnings Warnings { get; set; } = new Warnings();

        

        public void OpenJournalEntry(string filePath) => SystemProcess.Start(filePath);

        protected void Commit(GitCommitType commitType)
        {
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
                    Warnings.Add(MissingGitBinaryWarning);
                    _beenWarned = true;
                }
            }
#endif
        }

        private protected Journal OpenJournal()
        {
            var fileSystem = new FileSystem();
            var ioFactory = new JournalReaderWriterFactory(fileSystem, _locationParameter.Location);
            var markdownFiles = new MarkdownFiles(fileSystem, _locationParameter.Location);
            return Journal.Open(ioFactory, markdownFiles, SystemProcess);
        }

        private protected void Commit(string message)
        {
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
                    Warnings.Add(MissingGitBinaryWarning);
                    _beenWarned = true;
                }
            }
#endif
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

    public abstract class JournalController : ControllerBase
    {
        protected JournalController(ILocationParameter locationParameter) : base(locationParameter) { }

        public abstract void Run();
    }

    public abstract class JournalResultController : ControllerBase
    {
        protected JournalResultController(ILocationParameter locationParameter) : base(locationParameter) { }

        public abstract T Run<T>();
    }
}
