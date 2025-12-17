using UnityEngine;

[RequireComponent(typeof(PlayerStatHolder))]
public class StatController : MonoBehaviour
{
    private PlayerStatHolder playerStatHolder;
    private int staminaRecoverIdle = 10;                    // 멈춰있을때 초당 회복
    private int staminaRecoverWalk = 5;                     // 걸을때 초당 회복
    private int defaultExhaustTime = 3;                     // 탈진 지속 시간
    public int CurrentSanity { get; private set; }          // 현재 정신력
    public int CurrentRecoverStamina { get; private set; }  // 현재 스태미나 회복력
    public int CurrentStamina { get; private set; }         // 현재 스태미나
    public float CurrentMoveSpeed { get; private set; }     // 현재 이동 속도

    private void Awake()
    {
        playerStatHolder = this.GetComponent<PlayerStatHolder>();
    }

    public void Init()
    {
        CurrentStamina = playerStatHolder.Stat.MaxStamina;
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
                CurrentRecoverStamina = staminaRecoverIdle;
                CurrentMoveSpeed = 0;
                break;
            case PlayerState.Walk:
                CurrentRecoverStamina = staminaRecoverWalk;
                CurrentMoveSpeed = playerStatHolder.Stat.MoveSpeed;
                break;
        }
    }

    // 탈진 지속 시간
    public float GetDefaultExhaustTime() => defaultExhaustTime;

    // 스태미나 감소
    public void ConsumeStamina(int amount) => CurrentStamina = Mathf.Max(0, CurrentStamina - amount);
    
    // 스태미나 회복
    public void RecoverStamina(int amount) => CurrentStamina = Mathf.Min(playerStatHolder.Stat.MaxStamina, CurrentStamina + amount);
    
    // 스태미나가 남아있는지?
    public bool IsRemainStamina() => CurrentStamina > 0;
}
