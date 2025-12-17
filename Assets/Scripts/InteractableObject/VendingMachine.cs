using UnityEngine;

public class VendingMachine : Interactable
{
    public override void OnFocusEnter()
    {
        promptText = "Press [E] to Open Shop";
    }

    public override void OnFocusExit()
    {
        promptText = "";
    }

    public override void Interact()
    {
        // 상점 UI 열기
    }
}
