using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerModel : Monster
{
    // 워커 상태 정의 : 배회, 추격, 어그로 해제, 발견
    //public enum WalkerState { WanderingAround, Chase, MissingPlayer,FindPlayer }
    
    [Header("IndividualProperties")]
    [Header("Speed")]
    [SerializeField]private float chaseFast = 4.5f;
    [SerializeField] private float chaseSlow = 2.0f;
    [Header("Delay")]
    [SerializeField] private float stopToMissingDelay = 2.0f;
    // 시야 갱신 시점 조정
    [SerializeField] private float delay;
    [Header("Patrol")]
    [Header("Station")]
    // 경유할 장소 설정
    public List<Transform> moveTransformList;
    [Header("delay")]
    [SerializeField] int minimumStopDelay = 2;
    [SerializeField] int maxStopDelay = 3;

    //프로퍼티
    public float ChaseFast { get { return chaseFast; } }
    public float ChaseSlow { get { return chaseSlow; } }
    public float StopToMissingDelay {  get { return stopToMissingDelay; } }
    public float Delay { get { return delay; } }
    public int MinimumStopDelay { get { return minimumStopDelay; } }
    public int MaxStopDelay { get { return maxStopDelay; } }


    // 이벤트 생성
    public event Action OnWanderingAround;
    public event Action OnChase;
    public event Action OnMissingPlayer;
    public event Action OnFindPlayer;
    public event Action OnAlerted;


    public override void ChangeState(MonsterState inputState)
    {
        // 입력 받은 상태로 현재 상태를 바꾸고 해당 이벤트를 인보크함 
        switch (inputState)
        {
            case MonsterState.WanderingAround:
                Debug.Log($"{monsterState} : WanderingAround");
                monsterState = MonsterState.WanderingAround;
                OnWanderingAround?.Invoke();
                break;
            case MonsterState.Chase:
                Debug.Log($"{monsterState} : Chase");
                monsterState = MonsterState.Chase;
                OnChase?.Invoke();
                // OnChaseAfter?.Invoke();
                break;
            case MonsterState.MissingPlayer:
                Debug.Log($"{monsterState} : MissingPlayer");
                monsterState = MonsterState.MissingPlayer;
                OnMissingPlayer?.Invoke();
                // OnMissingPlayerAfter?.Invoke();
                break;
            case MonsterState.FindPlayer:
                Debug.Log($"{monsterState} : FindPlayer");
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

