using UnityEngine;

public class CheckSwitchOnCondition : IEventCondition
{
    public bool Check(string eventValue)
    {
        int switchId = int.Parse(eventValue);
        return SwitchManager.Instance.GetSwitch(switchId);
    }
}
