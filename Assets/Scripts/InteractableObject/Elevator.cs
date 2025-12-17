using UnityEngine;

public class Elevator : Interactable
{
    public override void OnFocusEnter()
    {
        promptText = "Press [E] to Use Elevator";
    }

    public override void OnFocusExit()
    {
        promptText = "";
    }

    public override void Interact()
    {
        // 씬 이동
        GameManager.Instance.ChangeState(GameState.Loading);
    }
}
