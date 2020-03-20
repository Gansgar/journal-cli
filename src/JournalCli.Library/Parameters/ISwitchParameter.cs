namespace JournalCli.Library.Parameters
{
    public interface ISwitchParameter<in TSwitch>
    {
        bool ToBool(TSwitch value);
    }
}