namespace JournalCli.Library.Parameters
{
    public interface ISaveJournalSnapshotParameters : ILocationParameter
    {
        string Message { get; set; }
    }
}
