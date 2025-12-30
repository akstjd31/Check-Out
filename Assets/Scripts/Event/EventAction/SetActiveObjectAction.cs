using UnityEngine;

public class SetActiveObjectAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = GameObject.Find(startValue);
        bool active = bool.Parse(eventValue);
        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.SetActiveObject(active, target);
        }
    }
}