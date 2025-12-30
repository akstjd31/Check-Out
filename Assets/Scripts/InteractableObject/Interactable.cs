using System.Net.Http.Headers;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string[] promptText;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected string interactableName; 
    protected EventObject eventObj;
    protected string currentText;
    protected string currentName;

    // 플레이어가 바라봤을때
    public abstract void OnFocusEnter();

    // 플레이어가 바라본상태에서 빠져나왔을 때
    public abstract void OnFocusExit();

    // 상호작용 기능
    public abstract void Interact();

    public virtual string GetCurrentText() => currentText;

    public virtual string GetCurrentName() => currentName;
    public virtual void SetEventObject(EventObject evtObj) => eventObj = evtObj;
}
