using System;
using UnityEngine;

public class WalkerModel : Monster
{
    // 워커 상태 정의 : 배회, 추격, 어그로 해제, 발견
    public enum WalkerState { WanderingAround, Chase, MissingPlayer,FindPlayer }
    
    [Header("IndividualProperties")]
    [Header("Speed")]
    [SerializeField]private float chaseFast = 4.5f;
    [SerializeField] private float chaseSlow = 2.0f;
    [HideInInspector]
    public WalkerState walkerState;

    public event Action OnWanderingAround;
    public event Action OnChase;
    public event Action OnMissingPlayer;
    public event Action OnFindPlayer;

    private void Awake() { Init(); }
    private void Init()
    {
        // 변수 초기화
        isObseredToPlayer = false;
        walkerState = WalkerState.WanderingAround;
    }

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
                break;
            case WalkerState.MissingPlayer:
                walkerState = WalkerState.MissingPlayer;
                OnMissingPlayer?.Invoke();
                break;
            case WalkerState.FindPlayer:
                walkerState = WalkerState.FindPlayer;
                OnFindPlayer?.Invoke();
                break;
        }
    }
}

