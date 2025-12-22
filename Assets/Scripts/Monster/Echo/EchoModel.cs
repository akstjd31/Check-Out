using System;
using UnityEngine;

public class EchoModel : Monster
{
    [Header("IndividualProperties")]
<<<<<<< Updated upstream
    [SerializeField] private float rangeOfRecognition;
    [SerializeField] private float darkSituationHoldingTime = 10f;
    [SerializeField] private float respawnTime = 30f;
=======
    // 눈 마주침 상호작용 거리 
    [SerializeField] private float rangeOfRecognition = 5.0f;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
    public float RangeOfRecognition { get { return rangeOfRecognition; } }
    public float DarkSituationHoldingTime { get { return darkSituationHoldingTime; } }
    public float RespawnTime { get { return respawnTime; } }
=======
>>>>>>> Stashed changes
    public float ViewRadius { get { return viewRadius; } }
    public float ViewAngle { get { return viewAngle; } }
    public LayerMask PlayerMask { get { return playerMask; } }
    public LayerMask ObstacleMask { get { return obstacleMask; } }
    public float Delay { get { return delay; } }
<<<<<<< Updated upstream

    // 액션
=======
    public float RangeOfRecognition { get { return rangeOfRecognition; } }

    // 이벤트 생성
>>>>>>> Stashed changes
    public event Action OnObserve;
    public event Action OnEyeContact;
    public event Action OnContactPlayer;

    public override void ChangeState(MonsterState inputState)
    {
<<<<<<< Updated upstream
        switch(inputState)
=======
        switch (inputState)
>>>>>>> Stashed changes
        {
            case MonsterState.Observe:
                monsterState = MonsterState.Observe;
                OnObserve?.Invoke();
                break;
            case MonsterState.EyeContact:
                monsterState = MonsterState.EyeContact;
                OnEyeContact?.Invoke();
                break;
<<<<<<< Updated upstream
            case MonsterState.ContactPlayer:
                monsterState = MonsterState.ContactPlayer;
=======
            case MonsterState.contactPlayer:
                monsterState = MonsterState.contactPlayer;
>>>>>>> Stashed changes
                OnContactPlayer?.Invoke();
                break;
        }
    }

}
