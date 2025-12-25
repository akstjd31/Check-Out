using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(PlayerStateMachine))]

public class SoundDistance : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundClip;
    private AudioSource soundSource;
    [UnityEngine.Range(0f, 1f)]
    [SerializeField] private float volume;
    [SerializeField] private float soundDelay = 0.5f;
    [SerializeField] private GameObject soundCollider;
    [SerializeField] private float distance;
    private WaitForSeconds delay;
    private PlayerStateMachine state;
    private SphereCollider distanceCollider;

    // 프로퍼티
    public AudioSource SoundSource { get { return soundSource; } }
    public float Volume { get { return volume; } }
    public float SoundDelay { get { return soundDelay; } }

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        state = this.GetComponent<PlayerStateMachine>();
        distanceCollider = soundCollider.GetComponent<SphereCollider>();

        distanceCollider.radius = 0.5f * distance;
        delay = new WaitForSeconds(soundDelay);
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

    public void PlayClip(int index, bool isLoop)
    {
        if (soundSource == null || !soundClip.Any())
            return;
            
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

    public bool IsPlaying() {  return soundSource.isPlaying; }
}
