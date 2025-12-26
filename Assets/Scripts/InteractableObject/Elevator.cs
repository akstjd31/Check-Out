using UnityEngine;

public class Elevator : Interactable
{
    public override void OnFocusEnter()
    {
        currentText = promptText[0];
    }

    public override void OnFocusExit()
    {
        currentText = "";
    }

    public override void Interact()
    {
        // 씬 이동
        GameManager.Instance.ChangeState(GameState.Loading);
    }
}
