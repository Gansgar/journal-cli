namespace JournalCli.Pwsh.Infrastructure
{
    public enum GitCommitType
    {
        PreNewJournalEntry,
        PostNewJournalEntry,
        PreAppendJournalEntry,
        PostAppendJournalEntry,
        PreRenameTag,
        PostRenameTag,
        Manual,
        PreOpenJournalEntry,
        PostOpenJournalEntry
    }
}