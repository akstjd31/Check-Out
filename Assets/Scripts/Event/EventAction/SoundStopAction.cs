using UnityEngine;

public class SoundStopAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = target.Equals("self") ? GameObject.Find(startValue) : GameObject.Find(target);

        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.StopSound();
        }
    }
}