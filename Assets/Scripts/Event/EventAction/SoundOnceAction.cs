using UnityEngine;

public class SoundOnceAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = GameObject.Find(startValue);
        var clip = EventSoundManager.Instance.GetAudioClipByPath(eventValue);
        EventObject evtObj = null;
        
        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            Target t = target.Equals("self") ? Target.Self : Target.Other;
            evtObj.SetAudioOnceSettings(t, clip);
            evtObj.PlaySoundWithDelay(t, EventManager.Instance.Delay);
        }
    }
}