using System;
using System.Collections.Generic;
using UnityEngine;

public class SirenModel : Monster
{
    // 워커 상태 정의 : 배회, 추격, 어그로 해제, 발견
    
    [Header("IndividualProperties")]
    [Header("Delay")]
    [SerializeField] private float stopToMissingDelay = 3.0f;
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
    // 비명 범위
    [Header("scream")]
    [SerializeField] float distance = 20f;

    [HideInInspector]
    //프로퍼티
    public float StopToMissingDelay {  get { return stopToMissingDelay; } }
    public float ViewRadius { get { return viewRadius; } }
    public float ViewAngle { get { return viewAngle; } }
    public LayerMask PlayerMask { get { return playerMask; } }
    public LayerMask ObstacleMask { get { return obstacleMask; } }
    public float Delay { get { return delay; } }
    public int MinimumStopDelay { get { return minimumStopDelay; } }
    public int MaxStopDelay { get { return maxStopDelay; } }
    public float Distance { get { return distance; } }

    // 이벤트 생성
    public event Action OnWanderingAround;
    public event Action OnAlert;
    public event Action OnFindPlayer;

    public override void ChangeState(MonsterState inputState)
    {
        // 입력 받은 상태로 현재 상태를 바꾸고 해당 이벤트를 인보크함 
        switch (inputState)
        {
            case MonsterState.WanderingAround:
                monsterState = MonsterState.WanderingAround;
                OnWanderingAround?.Invoke();
                break;
            case MonsterState.Alert:
                monsterState = MonsterState.Alert;
                OnAlert?.Invoke();
                break;
            case MonsterState.FindPlayer:
                monsterState = MonsterState.FindPlayer;
                OnFindPlayer?.Invoke();
                break;
        }
    }
}

