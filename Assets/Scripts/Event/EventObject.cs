using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum StartEventType
{
    EnterCollider,
    Interaction
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
    }

    private void Start()
    {
        string cloneName = "(Clone)";
        // 뒤에 클론 붙어있다면 제거 후 저장
        if (this.name.Contains(cloneName))
            StartValue = this.name.Substring(0, this.name.Length - cloneName.Length);
        else
            StartValue = this.name;

        if (currentStartType.Equals(StartEventType.Interaction) && this.TryGetComponent<Interactable>(out interactable))
        {
            interactable.SetEventObject(this);
        }

        StartType = StartEventTypeToString(currentStartType);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player") && interactable != null) return;

        Debug.Log("이벤트를 실행!");
        EventManager.Instance.OnEventTriggered(StartType, StartValue);
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
    public void SetAudioLoopSettings(AudioClip clip)
    {
        if (targetObj != null)
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();
            if (targetEvtObj != null)
            {
                targetEvtObj.SetAudioLoopSettings(clip);
            }
        }
        else
        {
            audioSource.clip = clip;
            audioSource.loop = true;
        }
    }

    public void SetAudioOnceSettings(AudioClip clip)
    {
        if (targetObj != null)
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();

            if (targetEvtObj != null)
            {
                targetEvtObj.SetAudioOnceSettings(clip);
                targetEvtObj.PlaySoundWithDelay(0);
            }
        }
        else
        {
            audioSource.clip = clip;
        }
    }

    public void PlaySoundWithDelay(float delay)
    {
        if (targetObj != null)
        {
            targetEvtObj = targetObj.GetComponent<EventObject>();

                if (targetEvtObj != null)
            {
                targetEvtObj.PlaySoundWithDelay(delay);
            }
        }
        else
            audioSource.PlayDelayed(delay);
    }

    public void SetActiveObject(bool active, float delay)
    {
        StartCoroutine(SetActiveWithDelay(active, delay));
    }

    private IEnumerator SetActiveWithDelay(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        // 타겟 오브젝트의 존재 여부 & 매개변수 비교
        if (targetObj != null)
            targetObj.gameObject.SetActive(active);
        else
            this.gameObject.SetActive(active);
    }

    public void StopSound()
    {
        Debug.Log("소리 멈춤");
        audioSource.Stop();
    }

    public void PlayAnimationByName(string name) => anim.Play(name);
}
