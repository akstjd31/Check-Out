using UnityEngine;

[RequireComponent(typeof(PlayerStatHolder))]
public class StatController : MonoBehaviour
{
    private PlayerStatHolder playerStatHolder;
    public int CurrentSanity { get; private set; }          // 현재 정신력
    public int CurrentRecoverStamina { get; private set; }  // 현재 스태미나 회복력
    public int CurrentStamina { get; private set; }         // 현재 스태미나
    public float CurrentMoveSpeed { get; private set; }     // 현재 이동 속도
    public int CurrentSanityDps { get; private set; }       // 정신력 감소량
    public int CurrentRunStaminaCost { get; private set; }  // 달리기 코스트
    public float CurrentInvincibilityTime { get; private set; }  // 무적시간

    private void Awake()
    {
        playerStatHolder = this.GetComponent<PlayerStatHolder>();
    }

    public void Init()
    {
        CurrentStamina = playerStatHolder.Stat.MaxStamina;
        CurrentSanity = playerStatHolder.Stat.MaxSanity;
        CurrentRunStaminaCost = playerStatHolder.Stat.RunStaminaCost;
        CurrentInvincibilityTime = playerStatHolder.Stat.InvincibilityTime;
    }

    // 각 상태에 따른 기본 수치 적용
    public void UpdateUsedValue(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Run:
                CurrentRecoverStamina = 0;
                CurrentMoveSpeed = playerStatHolder.Stat.RunSpeed;
                break;
            case PlayerState.Idle:
                CurrentRecoverStamina = playerStatHolder.Stat.StaminaRecoverIdle;
                CurrentMoveSpeed = 0;
                break;
            case PlayerState.Walk:
                CurrentRecoverStamina = playerStatHolder.Stat.StaminaRecoverWalk;
                CurrentMoveSpeed = playerStatHolder.Stat.MoveSpeed;
                break;
        }
    }

    public void UpdateSituationUsedValue(Playersituation situation)
    {
        switch (situation)
        {
            case Playersituation.Safe:
                CurrentSanityDps = 0;
                break;
            case Playersituation.Normal:
                CurrentSanityDps = playerStatHolder.Stat.SanityDpsNormal;
                break;
            case Playersituation.Dark:
                CurrentSanityDps = playerStatHolder.Stat.SanityDpsDark;
                break;
            case Playersituation.Chase:
                CurrentSanityDps = playerStatHolder.Stat.SanityDpsChased;
                break;
        }
    }

    // 탈진 지속 시간
    public float GetDefaultExhaustTime() => playerStatHolder.Stat.StaminaExhaustTime;

    // 스태미나 감소
    public void ConsumeStamina(int amount) => CurrentStamina = Mathf.Max(0, CurrentStamina - amount);
    
    // 스태미나 회복
    public void RecoverStamina(int amount) => CurrentStamina = Mathf.Min(playerStatHolder.Stat.MaxStamina, CurrentStamina + amount);
    
    // 스태미나가 남아있는지?
    public bool IsRemainStamina() => CurrentStamina > 0;
    
    //정신력 감소
    public void ConsumeSanity(int amount) => CurrentSanity = Mathf.Max(0, CurrentSanity - amount);

    // 정신력이 남아있는지?
    public bool IsRemainSanity() => CurrentSanity > 0;

}
