using UnityEngine;

public enum PlayerStatId
{
    MaxSanity = 1, MaxStamina, MoveSpeed, RunSpeed, SitSpeed, InvincibilityTime
}

public class PlayerStat
{
    public int MaxSanity { get; private set; }                // 최대 정신력
    public int MaxStamina { get; private set; }               // 최대 스태미너
    public float MoveSpeed { get; private set; }              // 기본 이동 속도
    public float RunSpeed { get; private set; }               // 달리기 시 이동 속도
    public float SitSpeed { get; private set; }               // 앉을 경우 이동 속도
    public float InvincibilityTime { get; private set; }      // 몬스터에게 피격 시 정신력이 깎이지 않는 무적 시간

    // 각 ID별 매핑
    public void Apply(PlayerStatId id, string value)
    {
        switch (id)
        {
            case PlayerStatId.MaxSanity:
                MaxSanity = int.Parse(value);
                break;

            case PlayerStatId.MaxStamina:
                MaxStamina = int.Parse(value);
                break;

            case PlayerStatId.MoveSpeed:
                MoveSpeed = float.Parse(value);
                break;

            case PlayerStatId.RunSpeed:
                RunSpeed = float.Parse(value);
                break;

            case PlayerStatId.SitSpeed:
                SitSpeed = float.Parse(value);
                break;

            case PlayerStatId.InvincibilityTime:
                InvincibilityTime = float.Parse(value);
                break;
        }
    }
}
