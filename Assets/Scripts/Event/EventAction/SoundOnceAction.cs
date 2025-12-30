using UnityEngine;

public class SoundOnceAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = target.Equals("self") ? GameObject.Find(startValue) : GameObject.Find(target);
        var clip = EventSoundManager.Instance.GetAudioClipByPath(eventValue);
        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.SetAudioOnceSettings(clip);
            evtObj.PlaySoundWithDelay(EventManager.Instance.Delay);
        }

        EventManager.Instance.Delay = 0f;
    }
}