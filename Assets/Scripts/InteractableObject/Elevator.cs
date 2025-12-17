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
        GameManager.Instance.ChangeState(GameState.Loading);
    }
}
