namespace JournalCli.Library.Parameters
{
    public interface IGetJournalReadmeEntriesParameters<T> : ILocationParameter 
    {
        T IncludeFuture { get; set; }
        string Period { get; set; }
        int Duration { get; set; }
        T All { get; set; }
    }
}