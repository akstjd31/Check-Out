using UnityEngine;

[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerStatHolder))]

// 플레이어 스태미나 관련 클래스
public class PlayerStamina : MonoBehaviour
{
    private StatController stat;
    private PlayerStatHolder holder;

    public bool IsExhausted { get; private set; }   // 탈진 상태
    private float staminaTimer;
    private float exhaustTimer;
    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        holder = this.GetComponent<PlayerStatHolder>();
    }

    // 스태미나 갱신
    public void UpdateStamina(bool isRunning, bool isMoving)
    {
        if (!FadeController.Instance.IsFadeEnded)
            return;

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

        // 달리고 있는 상태 == 왼쪽 쉬프트를 누르면서, 이동(MoveInput값이 존재할 때)할 때
        if (isRunning && isMoving)
        {
            // 현재 스태미나보다 달리기 스태미나 최소 비용보다 적은 상태
            if (!stat.IsRemainStamina())
            {
                EnterExhaust();
                return;
            }

            stat.ConsumeStamina();
        }
        else
        {
            stat.RecoverStamina();
        }
    }

    private void EnterExhaust()
    {
        exhaustTimer = holder.Stat.StaminaExhaustTime;
        Debug.Log("탈진!");
        IsExhausted = true;
    }
}
