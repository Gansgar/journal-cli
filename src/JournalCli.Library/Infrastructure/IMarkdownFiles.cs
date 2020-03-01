using System.Collections.Generic;

namespace JournalCli.Library.Infrastructure
{
    public interface IMarkdownFiles
    {
        List<string> FindAll();
    }
}