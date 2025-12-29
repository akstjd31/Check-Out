public class SoundLoopAction : IEventAction
{
    public void Execute(string eventValue, string target)
    {
        EventSoundManager.Instance.AudioSourceSettingByPath(eventValue, true);
        EventSoundManager.Instance.PlaySoundWithDelay(EventManager.Instance.Delay);
        EventManager.Instance.Delay = 0f;
    }
}