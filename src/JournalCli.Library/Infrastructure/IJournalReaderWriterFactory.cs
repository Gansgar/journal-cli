namespace JournalCli.Library.Infrastructure
{
    internal interface IJournalReaderWriterFactory
    {
        IJournalReader CreateReader(string filePath);
        IJournalWriter CreateWriter();
    }
}