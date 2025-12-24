using UnityEngine;

public class CheckSwitchOffCondition : IEventCondition
{
    public bool Check(string eventValue)
    {
        int switchId = int.Parse(eventValue);
        return SwitchManager.Instance.GetSwitch(switchId);
    }
}
