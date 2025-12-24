using UnityEngine;

enum DoorType
{
    Closed = 0, Open
}

public class Door : Interactable
{
    [SerializeField] private Animator anim;
    [SerializeField] private DoorType currentDoorType;

    private void Awake()
    {
        anim = this.transform.root.GetComponent<Animator>();
    }

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
        switch (currentDoorType)
        {
            case DoorType.Open:
                anim.SetBool("isOpen", false);
                currentDoorType = DoorType.Closed;
                break;
            case DoorType.Closed:
                anim.SetBool("isOpen", true);
                currentDoorType = DoorType.Open;
                break;
        }

        promptText = doorText[(int)currentDoorType];
    }
}
