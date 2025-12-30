using UnityEngine;

public class PlayAnimAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = target.Equals("self") ? GameObject.Find(startValue) : GameObject.Find(target);
        var clip = EventAnimManager.Instance.GetAnimClipByPath(eventValue);
        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.PlayAnimationByName(clip.name);
        }
    }
}