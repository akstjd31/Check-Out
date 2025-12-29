using Unity.VisualScripting;
using UnityEngine;

enum StartEventType
{
    EnterCollider,
    Interaction
}

public class EventObject : MonoBehaviour
{
    [SerializeField] private StartEventType startType;
    [SerializeField] private GameObject targetObj;
    private AudioSource audioSource;
    private Animator anim;
    private string startValue;
    private void Awake()
    {
        audioSource = this.AddComponent<AudioSource>();
        anim = this.AddComponent<Animator>();
        audioSource.volume = 0.2f;
    }

    private void Start()
    {
        string cloneName = "(Clone)";
        // 뒤에 클론 붙어있다면 제거 후 저장
        if (this.name.Contains(cloneName))
            startValue = this.name.Substring(0, this.name.Length - cloneName.Length);
        else
            startValue = this.name;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        Debug.Log("이벤트를 실행!");
        EventManager.Instance.OnEventTriggered(StartEventTypeToString(startType), startValue);
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
    public void SetAudioSettings(AudioClip clip, bool isLoop)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
    }

    public void PlaySoundWithDelay(float delay) => audioSource.PlayDelayed(delay);

    public void SetActiveObject(bool active, string targetName)
    {
        // 타겟 오브젝트의 존재 여부 & 매개변수 비교
        if (targetObj != null && targetObj.name.Equals(targetName))
            targetObj.SetActive(active);

    }

    public void StopSound()
    {
        Debug.Log("소리 멈춤");
        audioSource.Stop();
    }

    public void PlayAnimationByName(string name) => anim.Play(name);
}
