using UnityEngine;

public class Chest : Interactable
{
    public override void OnFocusEnter()
    {
        promptText = "Press [E] to Open Storage";
    }

    public override void OnFocusExit()
    {
        promptText = "";
    }

    public override void Interact()
    {
        // 창고 UI 열기
    }
}
