namespace JournalCli.Pwsh.Infrastructure
{
    internal interface IJournalReaderWriterFactory
    {
        IJournalReader CreateReader(string filePath);
        IJournalWriter CreateWriter();
    }
}