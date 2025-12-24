using System;
using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

[RequireComponent(typeof(PlayerStatHolder))]
public class StatController : MonoBehaviour
{
    [Header("Component")]
    private PlayerView playerView;
    private PlayerStatHolder holder;
    private PlayerInvincibility invincibility;
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
    public bool isDie = false;
    
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
        playerView = GameObject.Find(playerStatCanvasName).GetComponent<PlayerView>();
        invincibility = this.GetComponentInChildren<PlayerInvincibility>();
        stateMachine = this.GetComponentInChildren<PlayerStateMachine>();
    }

    public void Init()
    {
        CurrentStamina = holder.Stat.MaxStamina;
        CurrentSanity = holder.Stat.MaxSanity;
        CurrentRunStaminaCost = holder.Stat.RunStaminaCost;
        DefaultInvincibilityTime = holder.Stat.InvincibilityTime;

        playerView.UpdateStaminaText(CurrentStamina);
        playerView.UpdateSanityText((int)CurrentSanityPercent);
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
                CurrentSanityDps = holder.Stat.SanityDpsDark;
                 //CurrentSanityDps = 1000;  // 테스트용 
                break;
            case PlayerSituation.Chase:
                CurrentSanityDps = holder.Stat.SanityDpsChased;
                break;
        }

        playerView.UpdatePlayerSituationText(situation.ToString());
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
    public void ConsumeSanity(bool onhit,int amount)
    {

        CurrentSanity = Mathf.Max(0, CurrentSanity - amount);
        playerView.UpdateSanityText((int)CurrentSanityPercent);
        if (!IsRemainSanity())
            playerDie(onhit);

    }

    // 정신력이 남아있는지?
    public bool IsRemainSanity() => CurrentSanity > 0;
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
}