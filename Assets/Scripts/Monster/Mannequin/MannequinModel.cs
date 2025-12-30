using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinModel : Monster
{
    // 워커 상태 정의 : 배회, 접근, 정지, 어그로 해제, 발견
    //public enum WalkerState { WanderingAround, Approach, Stop, MissingPlayer,FindPlayer }
    
    private AudioSource audioSource;

    [Header("IndividualProperties")]
    [Header("Speed")]
    [SerializeField]private float approachSpeed = 9.0f;
    // 시야 갱신 시점 조정
    [SerializeField] private float delay;
    [Header("Patrol")]
    [Header("Station")]
    // 경유할 장소 설정
    public List<Transform> moveTransformList;
    [Header("delay")]
    [SerializeField] int minimumStopDelay = 2;
    [SerializeField] int maxStopDelay = 3;

    [HideInInspector]
    //프로퍼티
    public float ApproachSpeed { get { return approachSpeed; } }
    public float Delay { get { return delay; } }
    public int MinimumStopDelay { get { return minimumStopDelay; } }
    public int MaxStopDelay { get { return maxStopDelay; } }


    // 이벤트 생성
    public event Action OnWanderingAround;
    public event Action OnApproach;
    public event Action OnStop;
    public event Action OnMissingPlayer;
    public event Action OnFindPlayer;
    public event Action OnAlerted;

    private void Awake() => audioSource = this.GetComponent<AudioSource>();

    public override void ChangeState(MonsterState inputState)
    {
        // 입력 받은 상태로 현재 상태를 바꾸고 해당 이벤트를 인보크함 
        switch (inputState)
        {
            case MonsterState.WanderingAround:
                monsterState = MonsterState.WanderingAround;

                if (!audioSource.isPlaying)
                    audioSource.Play();
                    
                OnWanderingAround?.Invoke();
                break;
            case MonsterState.Approach:
                monsterState = MonsterState.Approach;

                if (!audioSource.isPlaying)
                    audioSource.Play();

                OnApproach?.Invoke();
                // OnChaseAfter?.Invoke();
                break;
            case MonsterState.Stop:
                isObservedFromPlayer = true;
                monsterState = MonsterState.Stop;
                audioSource.Stop();
                OnStop?.Invoke();
                // OnChaseAfter?.Invoke();
                break;
            case MonsterState.MissingPlayer:
                monsterState = MonsterState.MissingPlayer;
                OnMissingPlayer?.Invoke();
                // OnMissingPlayerAfter?.Invoke();
                break;
            case MonsterState.FindPlayer:
                if (OnFindPlayer == null)
                    Debug.LogWarning("OnFindPlayer에 구독자가 없습니다.");
                monsterState = MonsterState.FindPlayer;
                OnFindPlayer?.Invoke();
                break;
            case MonsterState.Alerted:
                monsterState = MonsterState.Alerted;
                OnAlerted?.Invoke();
                break;

        }
    }
}

