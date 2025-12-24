public class SetSwitchOnAction : IEventAction
{
    public void Execute(string eventValue, string target)
    {
        int switchId = int.Parse(eventValue);
        SwitchManager.Instance.SetSwitch(switchId, true);
    }
}