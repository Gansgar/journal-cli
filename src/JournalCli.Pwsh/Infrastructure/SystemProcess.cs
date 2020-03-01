using System.Diagnostics;

namespace JournalCli.Pwsh.Infrastructure
{
    internal class SystemProcess : ISystemProcess
    {
        public void Start(string filePath)
        {
            Process.Start(new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
        }
    }
}
