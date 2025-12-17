using UnityEngine;

enum DoorType
{
    Closed = 0, Open
}

public class Door : Interactable
{
    [SerializeField] private DoorType currentDoorType;
    string[] doorText = new string[2]
    {
        "Press [E] to Open",
        "Press [E] to Close"
    };

    // 문을 바라보는 상태
    public override void OnFocusEnter()
    {
        promptText = doorText[(int)currentDoorType];
    }

    // 문에서 벗어날 때
    public override void OnFocusExit()
    {
        promptText = "";
    }

    // 실제 상호작용
    public override void Interact()
    {
        // 애니메이션 수행
        currentDoorType = currentDoorType.Equals(DoorType.Closed) ? DoorType.Open : DoorType.Closed;
        promptText = doorText[(int)currentDoorType];
    }
}
