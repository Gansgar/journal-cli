namespace JournalCli.Library.Parameters
{
    public interface IRenameJournalTagParameters<T> : ILocationParameter
    {
        T DryRun { get; set; }
        string OldName { get; set; }
        string NewName { get; set; }
    }
}