using UnityEngine;

enum DoorType
{
    Closed, Open
}

public class Door : Interactable
{
    [SerializeField] private DoorType doorType;
    // 문을 바라보는 상태
    public override void OnFocusEnter()
    {
        if (doorType.Equals(DoorType.Closed))
            text.text = "Press E to Open";
        else
            text.text = "Press E to Close";
    }

    // 문에서 벗어날 때
    public override void OnFocusExit()
    {
        text.text = "";
    }

    // 실제 상호작용
    public override void Interact()
    {
        
    }
}
