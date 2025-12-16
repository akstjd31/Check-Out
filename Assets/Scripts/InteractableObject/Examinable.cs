using UnityEngine;

// 오브젝트 조사 관련
public class Examinable : Interactable
{
    public override void OnFocusEnter()
    {
        text.text = "Press [E] to Examine";
    }

    public override void OnFocusExit()
    {
        text.text = "";
    }

    public override void Interact()
    {
    }
}
