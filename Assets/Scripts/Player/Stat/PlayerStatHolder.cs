using UnityEngine;

/// <summary>
/// PlayerStat 프로퍼티를 가지게 되는 중개자(?) 역할
/// </summary>
[RequireComponent(typeof(PlayerStatInitializer))]
public class PlayerStatHolder : MonoBehaviour
{
    public PlayerStat Stat { get; private set; }
    public PlayerView PlayerView { get; private set; }
    private void Awake()
    {
        PlayerView = FindAnyObjectByType<PlayerView>();

        GameManager.Instance.PlayerInit(this.gameObject, this, PlayerView);
    }

    public void Init(PlayerStat stat)
    {
        Stat = stat;
        this.GetComponent<StatController>().Init();
        Debug.Log("플레이어 스탯 설정 완료!");
    }
}
