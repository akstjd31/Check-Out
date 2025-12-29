using System.Text.RegularExpressions;

public class CoolDownAction : IEventAction
{
    public void Execute(string eventValue, string target)
    {
        string value = Regex.Replace(eventValue, @"\D", "");
        float cooldown = float.Parse(value);

        EventManager.Instance.Cooldown = cooldown;
    }
}