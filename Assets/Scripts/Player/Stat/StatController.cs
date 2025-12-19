using UnityEngine;

[RequireComponent(typeof(PlayerStatHolder))]
public class StatController : MonoBehaviour
{
    private PlayerView playerView;
    private PlayerStatHolder holder;
    public int CurrentSanity { get; private set; }          // 현재 정신력
    public int CurrentRecoverStamina { get; private set; }  // 현재 스태미나 회복력
    public int CurrentStamina { get; private set; }         // 현재 스태미나
    public float CurrentMoveSpeed { get; private set; }     // 현재 이동 속도
    public int CurrentSanityDps { get; private set; }       // 정신력 감소량
    public int CurrentRunStaminaCost { get; private set; }  // 달리기 코스트
    public float CurrentInvincibilityTime { get; private set; }  // 무적시간

    private void Awake()
    {
        holder = this.GetComponent<PlayerStatHolder>();
        playerView = GameObject.Find("Canvas").GetComponent<PlayerView>();
    }

    public void Init()
    {
        CurrentStamina = holder.Stat.MaxStamina;
        CurrentSanity = holder.Stat.MaxSanity;
        CurrentRunStaminaCost = holder.Stat.RunStaminaCost;
        CurrentInvincibilityTime = holder.Stat.InvincibilityTime;

        playerView.UpdateStaminaText(CurrentStamina);
        playerView.UpdateSanityText(CurrentSanity);
    }

    // 각 상태에 따른 기본 수치 적용
    public void UpdateUsedValue(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Run:
                CurrentRecoverStamina = 0;
                CurrentMoveSpeed = holder.Stat.RunSpeed;
                break;
            case PlayerState.Idle:
                CurrentRecoverStamina = holder.Stat.StaminaRecoverIdle;
                CurrentMoveSpeed = 0;
                break;
            case PlayerState.Walk:
                CurrentRecoverStamina = holder.Stat.StaminaRecoverWalk;
                CurrentMoveSpeed = holder.Stat.MoveSpeed;
                break;
        }
    }

    //각 상태에 따라 정신력에 데미지를 입음
    public void UpdateSituationUsedValue(PlayerSituation situation)
    {
        switch (situation)
        {
            case PlayerSituation.Safe:
                CurrentSanityDps = 0;
                break;
            case PlayerSituation.Normal:
                CurrentSanityDps = holder.Stat.SanityDpsNormal;
                break;
            case PlayerSituation.Dark:
                CurrentSanityDps = holder.Stat.SanityDpsDark;
                break;
            case PlayerSituation.Chase:
                CurrentSanityDps = holder.Stat.SanityDpsChased;
                break;
        }

        playerView.UpdatePlayerSituationText(situation.ToString());
    }

    // 탈진 지속 시간
    public float GetDefaultExhaustTime() => holder.Stat.StaminaExhaustTime;

    // 스태미나 감소
    public void ConsumeStamina()
    {
        CurrentStamina = Mathf.Max(0, CurrentStamina - CurrentRunStaminaCost);
        playerView.UpdateStaminaText(CurrentStamina);
    } 
    
    // 스태미나 회복
    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Min(holder.Stat.MaxStamina, CurrentStamina + CurrentRecoverStamina);
        playerView.UpdateStaminaText(CurrentStamina);
    }
    
    // 스태미나가 남아있는지?
    public bool IsRemainStamina() => CurrentStamina >= holder.Stat.RunStaminaCost;
    
    //정신력 감소
    public void ConsumeSanity()
    {
        CurrentSanity = Mathf.Max(0, CurrentSanity - CurrentSanityDps);
        playerView.UpdateSanityText(CurrentSanity);
    }

    // 정신력이 남아있는지?
    public bool IsRemainSanity() => CurrentSanity > 0;

}
