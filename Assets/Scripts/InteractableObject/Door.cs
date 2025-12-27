using Unity.AI.Navigation;
using UnityEngine;

enum DoorType
{
    Closed = 0, Open
}

public class Door : Interactable
{
    [SerializeField] private Animator anim;
    [SerializeField] private DoorType currentDoorType;
    [SerializeField] private NavMeshLink navMeshLink;
    [SerializeField] private AudioClip[] clips;         // 0: 문 여는 소리, 1: 문 닫는 소리

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
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

    // 문 애니메이션 이벤트
    public void OnDoorSoundEvent()
    {
        audioSource.PlayOneShot(clips[(int)currentDoorType]);
    }

    // 문이 완전 열렸을 때 네브메쉬링크 사용이 가능하게끔 설정
    public void NavMeshLinkEnabled()
    {
        if (navMeshLink != null)
            navMeshLink.enabled = true;
    }

    public void NavMeshLinkDisabled()
    {
        if (navMeshLink != null)
            navMeshLink.enabled = false;
    }

    public void SetNavMeshLink(NavMeshLink navMeshLink)
    {
        this.navMeshLink = navMeshLink;
        this.navMeshLink.enabled = false;
    }

    public bool HasNavMeshLink() => navMeshLink != null;

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
