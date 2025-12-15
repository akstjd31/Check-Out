using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int MaxSanity { get; set; }      // 최대 정신력
    public int maxStamina { get; set; }     // 최대 스태미너
    public float moveSpeed { get; set; }    // 기본 이동 속도
    public float runSpeed;                  // 달리기 시 이동 속도
    public float sitSpeed;                  // 앉을 경우 이동 속도
    public float invincibilityTime;         // 몬스터에게 피격 시 정신력이 깎이지 않는 무적 시간
}
