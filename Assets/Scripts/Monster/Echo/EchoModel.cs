using System;
using UnityEngine;

public class EchoModel : Monster
{
    [Header("IndividualProperties")]
    [SerializeField] private float darkSituationHoldingTime = 10f;
    [SerializeField] private float respawnTime = 30f;

    // 눈 마주침 상호작용 거리 
    [SerializeField] private float rangeOfRecognition = 5.0f;

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

    // 프로퍼티
    public float RangeOfRecognition { get { return rangeOfRecognition; } }
    public float DarkSituationHoldingTime { get { return darkSituationHoldingTime; } }
    public float RespawnTime { get { return respawnTime; } }
    public float ViewRadius { get { return viewRadius; } }
    public float ViewAngle { get { return viewAngle; } }
    public LayerMask PlayerMask { get { return playerMask; } }
    public LayerMask ObstacleMask { get { return obstacleMask; } }
    public float Delay { get { return delay; } }

    // 이벤트 생성
    public event Action OnObserve;
    public event Action OnEyeContact;
    public event Action OnContactPlayer;

    public override void ChangeState(MonsterState inputState)
    {

        switch (inputState)
        {
            case MonsterState.Observe:
                monsterState = MonsterState.Observe;
                OnObserve?.Invoke();
                break;
            case MonsterState.EyeContact:
                monsterState = MonsterState.EyeContact;
                OnEyeContact?.Invoke();
                break;
            case MonsterState.contactPlayer:
                monsterState = MonsterState.contactPlayer;
                OnContactPlayer?.Invoke();
                break;
        }
    }

}
