public class DelayAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        float delay = float.Parse(eventValue);
        EventManager.Instance.Delay = delay;
    }
}