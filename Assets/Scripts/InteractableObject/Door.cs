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
        anim = this.GetComponent<Animator>();
    }


    // 문을 바라보는 상태
    public override void OnFocusEnter()
    {
        currentText = currentDoorType.Equals(DoorType.Closed) ? promptText[0] : promptText[1];
    }

    // 문에서 벗어날 때
    public override void OnFocusExit()
    {
        currentText = "";
    }

    // 실제 상호작용
    public override void Interact()
    {
        string newText = "";
        // 애니메이션 수행
        switch (currentDoorType)
        {
            case DoorType.Open:
                anim.SetBool("isOpen", false);
                currentDoorType = DoorType.Closed;
                newText = promptText[0];
                break;
            case DoorType.Closed:
                anim.SetBool("isOpen", true);
                currentDoorType = DoorType.Open;
                newText = promptText[1];
                break;
        }

        currentText = newText;
    }
}
