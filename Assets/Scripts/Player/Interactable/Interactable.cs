using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected string promptText;

    // 플레이어가 바라봤을때
    public abstract void OnFocusEnter();

    // 플레이어가 바라본상태에서 빠져나왔을 때
    public abstract void OnFocusExit();

    // 상호작용 기능
    public abstract void Interact();
}
