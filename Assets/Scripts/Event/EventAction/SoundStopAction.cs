public class SoundStopAction : IEventAction
{
    public void Execute(string eventValue, string target)
    {
        EventSoundManager.Instance.StopSound();
    }
}