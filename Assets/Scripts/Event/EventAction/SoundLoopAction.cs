using UnityEngine;

public class SoundLoopAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        GameObject obj = GameObject.Find(startValue);
        var clip = EventSoundManager.Instance.GetAudioClipByPath(eventValue);
        EventObject evtObj = null;

        if (obj != null && obj.TryGetComponent<EventObject>(out evtObj))
        {
            evtObj.SetAudioLoopSettings(clip);
            evtObj.PlaySoundWithDelay(EventManager.Instance.Delay);
        }

        EventManager.Instance.Delay = 0f;
    }
}