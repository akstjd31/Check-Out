using System.Text.RegularExpressions;

public class DelayAction : IEventAction
{
    public void Execute(string eventValue, string target, string startValue)
    {
        string value = Regex.Replace(eventValue, @"\D", "");
        float delay = float.Parse(value);
        EventManager.Instance.Delay = delay;
    }
}