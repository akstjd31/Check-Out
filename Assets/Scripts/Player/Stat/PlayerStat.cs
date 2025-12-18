using UnityEngine;

public enum PlayerStatId
{
    MaxSanity = 1,      // 최대 정신력
    MaxStamina,
    MoveSpeed, 
    RunSpeed, 
    SitSpeed,           // 앉아서 이동 속도
    InvincibilityTime,  // 피격 시 무적 시간
    MouseSensitivity,   // 마우스 민감도
    SanityDpsNormal,    // 기본 정신력 감소
    SanityDpsDark,      // 어두운 곳에서 정신력 감소
    SanityDpsChased,    // 추격 중 정신력 감소
    RunStaminaCost,     // 달리기 중 스태미나 감소
    StaminaRecoverIdle, // 대기 중 스태미나 회복
    StaminaRecoverWalk, // 걷기 중 스태미나 회복
    StaminaExhaustTime  // 탈진 시간
}

public class PlayerStat
{
    public int MaxSanity { get; private set; }                // 최대 정신력
    public int MaxStamina { get; private set; }               // 최대 스태미너
    public float MoveSpeed { get; private set; }              // 기본 이동 속도
    public float RunSpeed { get; private set; }               // 달리기 시 이동 속도
    public float SitSpeed { get; private set; }               // 앉을 경우 이동 속도
    public float InvincibilityTime { get; private set; }    
    public float MouseSensitivity { get; private set; }     
    public int SanityDpsNormal { get; private set; }      
    public int SanityDpsDark { get; private set; }      
    public int SanityDpsChased { get; private set; }
    public int RunStaminaCost { get; private set; }
    public int StaminaRecoverIdle { get; private set; }
    public int StaminaRecoverWalk { get; private set; }
    public float StaminaExhaustTime { get; private set; }
    

    // 각 ID별 매핑
    public void Apply(PlayerStatId id, string value)
    {
        switch (id)
        {
            case PlayerStatId.MaxSanity: MaxSanity = int.Parse(value); break;

            case PlayerStatId.MaxStamina: MaxStamina = int.Parse(value); break;

            case PlayerStatId.MoveSpeed: MoveSpeed = float.Parse(value); break;

            case PlayerStatId.RunSpeed: RunSpeed = float.Parse(value); break;

            case PlayerStatId.SitSpeed: SitSpeed = float.Parse(value); break;
                
            case PlayerStatId.InvincibilityTime: InvincibilityTime = float.Parse(value); break;

            case PlayerStatId.MouseSensitivity: MouseSensitivity = float.Parse(value); break;

            case PlayerStatId.SanityDpsNormal: SanityDpsNormal = int.Parse(value); break;

            case PlayerStatId.SanityDpsDark: SanityDpsDark = int.Parse(value); break;

            case PlayerStatId.SanityDpsChased: SanityDpsChased = int.Parse(value); break;

            case PlayerStatId.RunStaminaCost: RunStaminaCost = int.Parse(value); break;

            case PlayerStatId.StaminaRecoverIdle: StaminaRecoverIdle = int.Parse(value); break;

            case PlayerStatId.StaminaRecoverWalk: StaminaRecoverWalk = int.Parse(value); break;

            case PlayerStatId.StaminaExhaustTime: StaminaExhaustTime = float.Parse(value); break;

        }
    }
}
