using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerAreaDetector))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerSanityVisualController))]
[RequireComponent(typeof(PlayerCamera))]

public class PlayerSanity : MonoBehaviour
{
    private StatController stat;
    private PlayerAreaDetector areaDetector;
    private PlayerStateMachine stateMachine;
    private PlayerSanityVisualController santyVisual;
    private PlayerCamera playerCamera;
    private PlayerInvincibility invincibility;
    private float sanityTimer = 1f;

    private bool darkness = false;
    private bool isDead = false;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        areaDetector = this.GetComponent<PlayerAreaDetector>();
        stateMachine = this.GetComponent<PlayerStateMachine>();
        santyVisual = this.GetComponent<PlayerSanityVisualController>();
        playerCamera = this.GetComponent<PlayerCamera>();
        invincibility = this.GetComponent<PlayerInvincibility>();
    }

    private void Update()
    {
        if (!FadeController.Instance.IsFadeEnded)
            return;

        Die();

        sanityTimer -= Time.deltaTime;
        if (sanityTimer > 0f) return;

        sanityTimer = 1f;

        UpdateSituationByArea();
        UpdateSanityValue();
    }

    // 정신력 수치 갱신
    private void UpdateSanityValue()
    {
        stat.ConsumeSanity(stat.CurrentSanityDps);
        santyVisual.UpdateSanity(stat.CurrentSanityPercent / 100);
    }

    // 영역별 상태(Situation) 변경
    private void UpdateSituationByArea()
    {
        if (darkness)
            return;

        if (areaDetector.IsSafe)
        {
            stateMachine.ChangeSituation(PlayerSituation.Safe);
            return;
        }

        if (areaDetector.IsMonster)
        {
            stateMachine.ChangeSituation(PlayerSituation.Chase);
            return;
        }

        if (areaDetector.IsLight)
        {
            stateMachine.ChangeSituation(PlayerSituation.Normal);
        }
        else
        {
            stateMachine.ChangeSituation(PlayerSituation.Dark);
        }
    }

    public void SetDarkness(bool state)
    {
        darkness = state;
    }

    public void Die()
    {
        if (isDead) return;

        if (!stat.IsRemainSanity())
        {
            isDead = true;

            if (invincibility.isHit)
                stateMachine.ChangeDie(playerDeath.Hit);
            else
                stateMachine.ChangeDie(playerDeath.Normal);
        }
    }

}
