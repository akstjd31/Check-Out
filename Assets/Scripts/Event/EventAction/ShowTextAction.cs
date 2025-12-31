using UnityEngine;
public class ShowTextAction : IEventAction
{
    public void Execute(string eventValue, string targetObject, string startValue)
    {
        int textId = int.Parse(eventValue);
        //debug.Log("ShowTextAction의" + textId);
        ChatboxManager.Instance.StartChatbox(textId);
    }
}
