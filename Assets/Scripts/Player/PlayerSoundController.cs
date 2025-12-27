using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerSoundController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private List<AudioSource> audioSourceList; // 발소리, 정신력, 스태미나
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
        stat.OnDeath += EndLoop;
    }

    private void OnDisable()
    {
        stat.OnDeath -= EndLoop;
    }

    private void Update()
    {
        if (stat != null)
            ClipToCompare();
    }

    // 정신력 감소량에 따른 클립 변화 체크
    public void ClipToCompare()
    {
        AudioClip clip = SoundManager.Instance.GetSanityClip(stat.CurrentSanityPercent);
        if (!audioSourceList[1].clip.Equals(clip))
        {
            audioSourceList[1].clip = clip;
            audioSourceList[1].Play();
        }
    }

    // 죽었을 떄 루핑 꺼주기
    public void EndLoop()
    {
        audioSourceList[1].volume = 0.3f;
        audioSourceList[1].loop = false;
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

    public void StopMoveSound() => audioSourceList[0].Stop();
    public void PlayMoveSound(PlayerState state)
    {
        if (state.Equals(PlayerState.Walk))
            audioSourceList[0].clip = SoundManager.Instance.GetWalkClip();
        else
            audioSourceList[0].clip = SoundManager.Instance.GetRunClip();

        if (!audioSourceList[0].isPlaying)
            audioSourceList[0].Play();
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
