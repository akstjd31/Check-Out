using UnityEngine;

public class PlayerStatHolder : MonoBehaviour
{
    public PlayerStat Stat { get; private set; }
    private void Awake()
    {
        Stat = new PlayerStat();
    }

    public void Init(PlayerStat stat)
    {
        Stat = stat;
        Debug.Log("플레이어 스탯 설정 완료!");
    }
}
