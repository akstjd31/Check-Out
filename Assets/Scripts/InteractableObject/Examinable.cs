using UnityEngine;

// 오브젝트 조사 관련
public class Examinable : Interactable
{
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
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
        if (eventObj != null)
            EventManager.Instance.OnEventTriggered(eventObj.StartType, eventObj.StartValue);
    }
}
