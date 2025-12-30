using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum StartEventType
{
    EnterCollider,
    Interaction
}

public enum Target
{
    Self, Other
}

public class EventObject : MonoBehaviour
{
    [SerializeField] private StartEventType currentStartType;
    [SerializeField] private GameObject targetObj;
    private EventObject targetEvtObj;
    private float delay;
    private Interactable interactable;
    private AudioSource audioSource;
    private Animator anim;
    public string StartType { get; private set; }
    public string StartValue { get; private set; }
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();


        StartValue = this.name;

        if (currentStartType.Equals(StartEventType.Interaction) && this.TryGetComponent<Interactable>(out interactable))
        {
            interactable.SetEventObject(this);
        }

        StartType = StartEventTypeToString(currentStartType);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player") || interactable != null) return;

        if (currentStartType.Equals(StartEventType.EnterCollider))
        {
            Debug.Log("이벤트를 실행!");
            EventManager.Instance.OnEventTriggered(StartType, StartValue);
        }
    }

    // 시작 이벤트 타입을 스트링으로 변환
    private string StartEventTypeToString(StartEventType type)
    {
        switch (type)
        {
            case StartEventType.EnterCollider:
                return "enterCollider";
            case StartEventType.Interaction:
                return "interaction";
        }

        return null;
    }

    // 사운드 설정
    public void SetAudioLoopSettings(Target target, AudioClip clip)
    {
        if (target.Equals(Target.Self))
        {
            audioSource.clip = clip;
            audioSource.loop = true;
        }
        else
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();
            targetEvtObj.SetAudioLoopSettings(Target.Self, clip);
        }
    }

    public void SetAudioOnceSettings(Target target, AudioClip clip)
    {
        if (target.Equals(Target.Self))
            audioSource.clip = clip;
        else
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();
            targetEvtObj.SetAudioOnceSettings(Target.Self, clip);
        }
    }

    public void PlaySoundWithDelay(Target target, float delay)
    {
        if (target.Equals(Target.Self))
            audioSource.PlayDelayed(delay);
        else
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();
            targetEvtObj.PlaySoundWithDelay(Target.Self, delay);
        }
    }

    public void SetActiveObject(Target target, bool active, float delay)
    {
        StartCoroutine(SetActiveWithDelay(target, active, delay));
    }

    private IEnumerator SetActiveWithDelay(Target target, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target.Equals(Target.Self))
            this.gameObject.SetActive(active);
        else
            targetObj.gameObject.SetActive(active);
    }

    public void StopSound()
    {
        Debug.Log("소리 멈춤");
        audioSource.Stop();
    }

    public void PlayAnimationByName(string name) => anim.Play(name);
}
