using System.Collections.Generic;

namespace JournalCli.Pwsh.Infrastructure
{
    public interface IMarkdownFiles
    {
        List<string> FindAll();
    }
}