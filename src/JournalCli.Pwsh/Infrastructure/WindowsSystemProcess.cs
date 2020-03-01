using System.Diagnostics;

namespace JournalCli.Pwsh.Infrastructure
{
    internal class WindowsSystemProcess : ISystemProcess
    {
        public void Start(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = "\"" + filePath + "\"",
                UseShellExecute = true
            });
        }
    }
}