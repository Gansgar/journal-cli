namespace JournalCli.Library.Parameters
{
    public interface IBackupJournalParameters<T> : ILocationParameter
    {
        string BackupLocation { get; set; }
        string Password { get; set; }
        T SaveParameters { get; set; }
    }
}