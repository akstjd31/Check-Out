using UnityEngine;

/// <summary>
/// PlayerStat 프로퍼티를 가지게 되는 중개자(?) 역할
/// </summary>
[RequireComponent(typeof(PlayerStatInitializer))]
public class PlayerStatHolder : MonoBehaviour
{
    public PlayerStat Stat { get; private set; }
    [SerializeField] private SaveLoadManager saveLoadManager;
    private void Awake()
    {
        Stat = new PlayerStat();
    }

    public void Init(PlayerStat stat)
    {
        Stat = stat;
        this.GetComponent<StatController>().Init();
        Debug.Log("플레이어 스탯 설정 완료!");
    }
}
