using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AudioSourceType 
{
    Move = 0, Sanity, Stamina, Item
}

[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerSoundController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private List<AudioSource> audioSourceList;
    private StatController stat;
    [SerializeField] private GameObject soundSensorObj;
    private PlayerStateMachine state;
    private SphereCollider distanceCollider;

    [Header("Value")]
    [SerializeField] private float soundDistance;

    private void Awake()
    {
        state = this.GetComponent<PlayerStateMachine>();
        stat = this.GetComponent<StatController>();
        distanceCollider = soundSensorObj.GetComponent<SphereCollider>();

        distanceCollider.radius = 0.5f * soundDistance;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        stat.OnDeath += SanityEndLoop;
    }

    private void OnDisable()
    {
        stat.OnDeath -= SanityEndLoop;
    }

    private void Update()
    {
        if (stat != null)
            SanityClipToCompare();
    }

    // 정신력 감소량에 따른 클립 변화 체크
    public void SanityClipToCompare()
    {
        AudioClip clip = SoundManager.Instance.GetSanityClip(stat.CurrentSanityPercent);
        if (!audioSourceList[(int)AudioSourceType.Sanity].clip.Equals(clip))
        {
            audioSourceList[(int)AudioSourceType.Sanity].clip = clip;
            audioSourceList[(int)AudioSourceType.Sanity].Play();
        }
    }

    // 죽었을 떄 루핑 꺼주기
    public void SanityEndLoop()
    {
        audioSourceList[(int)AudioSourceType.Sanity].volume = 0.3f;
        audioSourceList[(int)AudioSourceType.Sanity].loop = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<SirenModel>(out var sirenModel) &&
            other.TryGetComponent<SirenController>(out var sirenController) &&
            (state.CurrentState == PlayerState.Run || state.CurrentState == PlayerState.Walk))
        {
            if (sirenModel.monsterState == Monster.MonsterState.WanderingAround)
            {
                sirenController.GetTransform(transform);
                sirenModel.ChangeState(Monster.MonsterState.Alert);
            }
        }

        //if (other.TryGetComponent<SirenController>(out var sirenController)
        //    && other.TryGetComponent<SirenModel>(out var sirenModel)
        //    )
        //{
        //    Debug.Log($" 현재 사이렌 상태 : {sirenModel.monsterState.ToString()}");
        //    if (sirenModel.monsterState == Monster.MonsterState.WanderingAround)
        //        sirenModel.ChangeState(Monster.MonsterState.Alert);
        //}
    }

    public void StopMoveSound() => audioSourceList[(int)AudioSourceType.Move].Stop();
    public void PlayMoveSound(PlayerState state)
    {
        if (state.Equals(PlayerState.Walk))
            audioSourceList[(int)AudioSourceType.Move].clip = SoundManager.Instance.GetWalkClip();
        else
            audioSourceList[(int)AudioSourceType.Move].clip = SoundManager.Instance.GetRunClip();

        if (!audioSourceList[(int)AudioSourceType.Move].isPlaying)
            audioSourceList[(int)AudioSourceType.Move].Play();
    }

    public void PlayItemPickUpSound() => audioSourceList[(int)AudioSourceType.Item].
                                            PlayOneShot(SoundManager.Instance.GetItemPickUpClip());

    public void PlayItemPickUpFailedSound()
    {
        audioSourceList[(int)AudioSourceType.Item].clip = SoundManager.Instance.GetBuyItemFaildClip();
        audioSourceList[(int)AudioSourceType.Item].Play();
    }
    
    // public void PlaySanitySound(float volume)
    // {
    //     // 현재 클립이 다른 경우 (수치에 따른 클립 사운드 재생)
    //     // if (!currentSanityClip.Equals(clip))
    //     // {
    //     //     currentSanityClip = clip;
    //     //     audioSource.PlayOneShot(currentSanityClip);
    //     //     sanityClipLength = currentSanityClip.length;
    //     //     Debug.Log("클립 전환됨!");
    //     //     return;
    //     // }

    //     if (sanityClipLength > 0f)
    //     {
    //         sanityClipLength -= Time.deltaTime;
    //         return;
    //     }
        
    //     Debug.Log("사운드 끝나서 새로 재생!");

    //     // 해당 사운드 끝나면 바로 재생
    //     audioSource.PlayOneShot(currentSanityClip);
    //     sanityClipLength = currentSanityClip.length;
        
    // }
}
