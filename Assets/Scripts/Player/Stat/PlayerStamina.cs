using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    private StatController stat;
    private PlayerStatHolder holder;

    public bool IsExhausted { get; private set; }   // 탈진 상태
    private float staminaTimer;
    private float exhaustTimer;
    private float defaultExhaustTime;               // 기본 탈진 상태 탈출 타이머
    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        defaultExhaustTime = holder.Stat.StaminaExhaustTime;
        exhaustTimer = defaultExhaustTime;
    }

    public void UpdateStamina(bool isRunning)
    {
        // 탈진 상태라면
        if (IsExhausted)
        {
            exhaustTimer -= Time.deltaTime;
            if (exhaustTimer <= 0f)
                IsExhausted = false;
            return;
        }

        staminaTimer -= Time.deltaTime;
        if (staminaTimer > 0f) return;

        staminaTimer = 1f;

        // 뛸 수 있는 경우
        if (isRunning && stat.IsRemainStamina())
        {
            stat.ConsumeStamina(stat.CurrentRunStaminaCost);
            if (stat.CurrentStamina <= 0)
                EnterExhaust();
                
        }
        else
        {
            stat.RecoverStamina(stat.CurrentRecoverStamina);
        }
    }

    private void EnterExhaust()
    {
        Debug.Log("탈진!");
        IsExhausted = true;
        exhaustTimer = defaultExhaustTime;
    }
}
