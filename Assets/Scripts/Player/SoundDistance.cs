using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerStateMachine))]

public class SoundDistance : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundClip;
    private AudioSource soundSource;
    [UnityEngine.Range(0f, 1f)]
    [SerializeField] private float volume;
    [SerializeField] private GameObject soundCollider;
    [SerializeField] private float distance;
    private PlayerStateMachine state;
    private SphereCollider distanceCollider;

    // 프로퍼티
    public AudioSource SoundSource { get { return soundSource; } }
    public float Volume { get { return volume; } }

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        state = this.GetComponent<PlayerStateMachine>();
        distanceCollider = soundCollider.GetComponent<SphereCollider>();

        distanceCollider.radius = 0.5f * distance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<SirenModel>(out var sirenModel) && (state.CurrentState == PlayerState.Run || state.CurrentState == PlayerState.Walk))
        {
            if (sirenModel.monsterState == Monster.MonsterState.WanderingAround)
                sirenModel.ChangeState(Monster.MonsterState.Alert);
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

    public void PlayClip(int index, bool isLoop)
    {
        soundSource.clip = soundClip[index];
        soundSource.loop = isLoop;
        soundSource.Play();
    }

    public void SetVolume(int volume)
    {
        soundSource.volume = volume;
    }
    public void Stop()
    {
        soundSource?.Stop();
    }
}
