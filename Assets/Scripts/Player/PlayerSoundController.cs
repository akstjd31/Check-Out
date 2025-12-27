using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(PlayerStateMachine))]

public class PlayerSoundController : MonoBehaviour
{
    [Header("Component")]
    private AudioSource audioSource;
    [SerializeField] private GameObject soundSensorObj;
    private PlayerStateMachine state;
    private SphereCollider distanceCollider;

    [Header("Value")]
    [SerializeField] private float soundDistance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        state = this.GetComponent<PlayerStateMachine>();
        distanceCollider = soundSensorObj.GetComponent<SphereCollider>();

        distanceCollider.radius = 0.5f * soundDistance;
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

    public void StopSound() => audioSource.Stop();
    public void PlaySound(PlayerState state)
    {
        if (state.Equals(PlayerState.Walk))
            audioSource.clip = SoundManager.Instance.GetWalkClip();
        else
            audioSource.clip = SoundManager.Instance.GetRunClip();

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
