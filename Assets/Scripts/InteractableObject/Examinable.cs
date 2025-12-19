using UnityEngine;

// 오브젝트 조사 관련
public class Examinable : Interactable
{
    private void Awake()
    {
    }
    public override void OnFocusEnter()
    {
        promptText = "Press [E] to Examine";
    }

    public override void OnFocusExit()
    {
        promptText = "";
    }

    public override void Interact()
    {
    }
}
