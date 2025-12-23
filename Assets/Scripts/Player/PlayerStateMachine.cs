using UnityEngine;

// 플레이어 상태
public enum PlayerState { Idle, Walk, Run, Die }

// 특정 영역과 관련된 상태
public enum PlayerSituation { Safe, Normal, Dark, Chase }

[RequireComponent(typeof(StatController))]
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    public PlayerSituation CurrentSituation { get; private set; }

    private StatController stat;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
    }

    private void OnEnable()
    {
        stat.OnDeath += ChangeDieState;
    }

    private void OnDisable()
    {
        stat.OnDeath -= ChangeDieState;
    }

    public void ChangeDieState()
    {
        ChangeState(PlayerState.Die);
    }

    // 상태 변경
    public void ChangeState(PlayerState state)
    {
        // 이미 같은 상태면 할게 없음
        if (CurrentState == state) return;
        CurrentState = state;
        stat.UpdatePlayerState(state);
    }

    public void ChangeSituation(PlayerSituation situation)
    {
        if (CurrentSituation == situation) return;
        CurrentSituation = situation;
        stat.UpdateSituationUsedValue(situation);
    }
}
