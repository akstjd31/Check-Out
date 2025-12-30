using UnityEngine;

// 오브젝트 조사 관련
public class Examinable : Interactable
{
    private void Awake()
    {
    }
    public override void OnFocusEnter()
    {
        currentText = promptText[0];
        currentName = interactableName;
    }

    public override void OnFocusExit()
    {
        currentText = "";
        currentName = "";
    }

    public override void Interact()
    {
    }
}
