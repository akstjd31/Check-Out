using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    private StatController stat;
    private PlayerAreaDetector areaDetector;   
    private PlayerStateMachine stateMachine;
    [SerializeField] private PlayerView view;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        areaDetector = this.GetComponent<PlayerAreaDetector>();
        stateMachine = this.GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        // 정신력(체력)이 남아있지 않은 경우
        if (!stat.IsRemainSanity())
        {
            // 게임 오버 관련
            return;
        }

        UpdateSituationByArea();
        UpdateSanityValue();
        view.UpdateSanityText(stat.CurrentSanity);
    }

    // 정신력 수치 갱신
    private void UpdateSanityValue() => stat.ConsumeSanity(stat.CurrentSanityDps);
    
    // 영역별 상태(Situation) 변경
    private void UpdateSituationByArea()
    {
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

        stateMachine.ChangeSituation(
            areaDetector.IsLight ? PlayerSituation.Normal : PlayerSituation.Dark
        );
    }
}
