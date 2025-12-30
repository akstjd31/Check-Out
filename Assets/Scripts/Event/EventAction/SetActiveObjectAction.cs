using UnityEngine;

public class SetActiveObjectAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = target.Equals("self") ? GameObject.Find(startValue) : GameObject.Find(target);
        bool active = bool.Parse(eventValue);
        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.SetActiveObject(active, target, EventManager.Instance.Delay);
        }

        EventManager.Instance.Delay = 0f;
    }
}