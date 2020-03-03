using CommandLine;

namespace JournalCli.GenericCli
{
    public abstract class JournalOptionsBase
    {
        //[Option('l', "Location")]
        public string Location { get; set; }
    }
}