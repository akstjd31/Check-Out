using UnityEngine;
public class ShowTextAction : IEventAction
{
    public void Execute(string eventValue, string targetObject)
    {
        int textId = int.Parse(eventValue);
        Debug.Log("ShowTextAction의" + textId);
        ChatboxManager.Instance.StartChatbox(textId);
    }
}
