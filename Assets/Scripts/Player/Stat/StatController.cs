using System;
using UnityEngine;

/// <summary>
/// 플레이어의 현재 스탯들을 관리하는 클래스
/// </summary>

[RequireComponent(typeof(PlayerStatHolder))]
public class StatController : MonoBehaviour
{
    [Header("Component")]
    private PlayerStatHolder holder;
    private PlayerStateMachine stateMachine;

    [Header("Property")]
    public int CurrentSanity { get; private set; }              // 현재 정신력
    public int CurrentRecoverStamina { get; private set; }      // 현재 스태미나 회복력
    public int CurrentStamina { get; private set; }             // 현재 스태미나
    public float CurrentMoveSpeed { get; private set; }         // 현재 이동 속도
    public int CurrentSanityDps { get; private set; }           // 정신력 감소량
    public int CurrentRunStaminaCost { get; private set; }      // 달리기 코스트
    public float DefaultInvincibilityTime { get; private set; }  // 무적시간
    public event Action OnDeath;
    
    //현재 정신력 비율 측정
    public float CurrentSanityPercent
    {
        get => (float)CurrentSanity / 100;
    }

    [Header("Value")]
    private float speedMultiplier = 1.5f;                        // 1.5배 속도 관련 계수
    private string playerStatCanvasName = "PlayerStatCanvas";    // 플레이어 스탯 관련 캔버스

    private void Awake()
    {
        holder = this.GetComponent<PlayerStatHolder>();
        stateMachine = this.GetComponentInChildren<PlayerStateMachine>();
    }

    public void Init()
    {
        CurrentStamina = holder.Stat.MaxStamina;
        CurrentSanity = holder.Stat.MaxSanity;
        CurrentRunStaminaCost = holder.Stat.RunStaminaCost;
        DefaultInvincibilityTime = holder.Stat.InvincibilityTime;

        if (holder != null)
        {
            holder.PlayerView.UpdateStaminaText(CurrentStamina);
            holder.PlayerView.UpdateSanityText((int)CurrentSanityPercent);
        }
    }

    // 각 상태에 따른 기본 수치 적용
    public void UpdatePlayerState(PlayerState state)
    {
        // 정신력 수치에 따른 상태 변화 적용
        bool isSanityWarning = CurrentSanityPercent <= 0.33f && CurrentSanityPercent > 0f;
        switch (state)
        {
            case PlayerState.Run:
                CurrentRecoverStamina = 0;
                CurrentMoveSpeed = isSanityWarning ? holder.Stat.RunSpeed * speedMultiplier : holder.Stat.RunSpeed;
                break;
            case PlayerState.Idle:
                CurrentRecoverStamina = isSanityWarning ? holder.Stat.StaminaRecoverIdle / 2 : holder.Stat.StaminaRecoverIdle;
                CurrentMoveSpeed = 0;
                break;
            case PlayerState.Walk:
                CurrentRecoverStamina = isSanityWarning ? holder.Stat.StaminaRecoverWalk / 2 : holder.Stat.StaminaRecoverWalk;
                CurrentMoveSpeed = isSanityWarning ? holder.Stat.MoveSpeed * speedMultiplier : holder.Stat.MoveSpeed;
                break;
            case PlayerState.Die:
                CurrentMoveSpeed = 0f;
                CurrentStamina = 0;
                CurrentSanity = 0;
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
                //CurrentSanityDps = holder.Stat.SanityDpsDark;
                CurrentSanityDps = 1000;  // 테스트용 
                break;
            case PlayerSituation.Chase:
                CurrentSanityDps = holder.Stat.SanityDpsChased;
                break;
        }

        holder.PlayerView.UpdatePlayerSituationText(situation.ToString());
    }

    public void UpdateDeath(playerDeath death)
    {
        switch(death)
        {
            case playerDeath.None:
                break;
            case playerDeath.Normal:
                Debug.Log("죽음");
                OnDeath?.Invoke();
                break;
            case playerDeath.Hit:
                Debug.Log("맞아죽음");
                OnDeath?.Invoke();
                break;
        }
    }

    // 탈진 지속 시간
    public float GetDefaultExhaustTime() => holder.Stat.StaminaExhaustTime;

    // 스태미나 감소
    public void ConsumeStamina()
    {
        CurrentStamina = Mathf.Max(0, CurrentStamina - CurrentRunStaminaCost);
        
        holder.PlayerView.UpdateStaminaText(CurrentStamina);
    } 
    
    // 스태미나 회복
    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Min(holder.Stat.MaxStamina, CurrentStamina + CurrentRecoverStamina);
        
        holder.PlayerView.UpdateStaminaText(CurrentStamina);
    }
    
    // 스태미나가 남아있는지?
    public bool IsRemainStamina() => CurrentStamina >= holder.Stat.RunStaminaCost;

    //정신력 수치 변화
    public void ChangeSanity(bool onhit, int amount)
    {
        // 음수, 양수 값에 따른 처리
        CurrentSanity = amount < 0 ? Mathf.Max(0, CurrentSanity + amount) : Mathf.Min(CurrentSanity + amount, holder.Stat.MaxSanity);
        
        holder.PlayerView.UpdateSanityText((int)CurrentSanityPercent);
            
        if (!IsRemainSanity())
            playerDie(onhit);
    }

    // 정신력이 남아있는지?
    public bool IsRemainSanity() => CurrentSanity > 0;

    // 플레이어 사망 처리 (맞아서 죽었는가? or 초당 감소량으로 죽었는가?)
    public void playerDie(bool onhit)
    {
        if (onhit)
        {
            stateMachine.ChangeDeath(playerDeath.Hit);
        }
        else
        {
            stateMachine.ChangeDeath(playerDeath.Normal);
        }
    }

    // 스태미나 추가 메서드
    public void AddStamina(int amount)
    {
        CurrentStamina = Mathf.Min(holder.Stat.MaxStamina, CurrentStamina + amount);
        holder.PlayerView.UpdateStaminaText(CurrentStamina);
    }
}