using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerModel : Monster
{
    // 워커 상태 정의 : 배회, 추격, 어그로 해제, 발견
    public enum WalkerState { WanderingAround, Chase, MissingPlayer,FindPlayer }
    
    [Header("IndividualProperties")]
    [Header("Speed")]
    [SerializeField]private float chaseFast = 4.5f;
    [SerializeField] private float chaseSlow = 2.0f;
    [Header("Delay")]
    [SerializeField] private float stopToMissingDelay = 2.0f;
    [Header("MonsterFieldOfView")]
    // 시야 범위 조정
    [SerializeField] private float viewRadius;
    // 각도 360으로 제한
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    // 필요한 레이어
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
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
    public WalkerState walkerState;
    //프로퍼티
    public float ChaseFast { get { return chaseFast; } }
    public float ChaseSlow { get { return chaseSlow; } }
    public float StopToMissingDelay {  get { return stopToMissingDelay; } }
    public float ViewRadius { get { return viewRadius; } }
    public float ViewAngle { get { return viewAngle; } }
    public LayerMask PlayerMask { get { return playerMask; } }
    public LayerMask ObstacleMask { get { return obstacleMask; } }
    public float Delay { get { return delay; } }
    public int MinimumStopDelay { get { return minimumStopDelay; } }
    public int MaxStopDelay { get { return maxStopDelay; } }


    // 이벤트 생성
    public event Action OnWanderingAround;
    public event Action OnChase;
    public event Action<IEnumerator> OnChaseAfter;
    public event Action OnMissingPlayer;
    public event Action<IEnumerator> OnMissingPlayerAfter;
    public event Action OnFindPlayer;
    public event Action<WalkerState> OnFindPlayerAfter;


    public void ChangeState(WalkerState inputState)
    {
        // 입력 받은 상태로 현재 상태를 바꾸고 해당 이벤트를 인보크함 
        switch (inputState)
        {
            case WalkerState.WanderingAround:
                walkerState = WalkerState.WanderingAround;
                OnWanderingAround?.Invoke();
                break;
            case WalkerState.Chase:
                walkerState = WalkerState.Chase;
                OnChase?.Invoke();
                OnChaseAfter?.Invoke();
                break;
            case WalkerState.MissingPlayer:
                walkerState = WalkerState.MissingPlayer;
                OnMissingPlayer?.Invoke();
                OnMissingPlayerAfter?.Invoke();
                break;
            case WalkerState.FindPlayer:
                walkerState = WalkerState.FindPlayer;
                OnFindPlayer?.Invoke();
                OnFindPlayerAfter?.Invoke(walkerState);
                break;
        }
    }
}

