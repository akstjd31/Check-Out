public class SoundOnceAction : IEventAction
{
    public void Execute(string eventValue, string target)
    {
        SoundManager.Instance.PlaySound(eventValue);
    }
}