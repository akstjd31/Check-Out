using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] protected string promptText = "";

    public void OnHandleItem()
    {
    }

    public void Use()
    {

    }

    public virtual string GetPromptText() => promptText;
}
