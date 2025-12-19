using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    // 몬스터가 공통적으로 가지는 속성
    [Header("Common")]
    // 배회할 때 가지는 속도
    [SerializeField] private float patrolSpeed;
    [SerializeField] private int power;
    // 몬스터의 내부적인 속성
    [HideInInspector]
    public bool isObseredFromPlayer;

    //프로퍼티

    public float PatrolSpeed { get { return patrolSpeed; } }
    public int Power { get { return power; } }
}
