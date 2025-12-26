using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerAreaDetector))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerSanityVisualController))]
[RequireComponent(typeof(PlayerCameraController))]

public class PlayerSanity : MonoBehaviour
{
    private StatController stat;
    private PlayerAreaDetector areaDetector;   
    private PlayerStateMachine stateMachine;
    private PlayerSanityVisualController santyVisual;
    private float sanityTimer = 1f;

    private bool darkness = false;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        areaDetector = this.GetComponent<PlayerAreaDetector>();
        stateMachine = this.GetComponent<PlayerStateMachine>();
        santyVisual = this.GetComponent<PlayerSanityVisualController>();
    }

    private void Update()
    {
        // if (!FadeController.Instance.IsFadeEnded)
        //     return;

        sanityTimer -= Time.deltaTime;
        if (sanityTimer > 0f) return;

        sanityTimer = 1f;

        UpdateSituationByArea();
        UpdateSanityValue();
    }

    // 정신력 수치 갱신
    private void UpdateSanityValue()
    {
        stat.ChangeSanity(false,stat.CurrentSanityDps);
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
        else
        {
            stateMachine.ChangeSituation(stateMachine.PreviousSituation);
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

    public bool IsSanityBelow(int value) => stat.CurrentSanityPercent <= value;
}
