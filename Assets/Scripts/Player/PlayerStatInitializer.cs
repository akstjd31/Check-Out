using UnityEngine;

public class PlayerStatInitializer : MonoBehaviour
{
    private PlayerStatHolder holder;

    private void Awake()
    {
        holder = this.GetComponent<PlayerStatHolder>();
    }
    private void Start()
    {
        var playerStatTable = TableManager.Instance.GetTable<int, PlayerConfigTableData>();
        
        if (playerStatTable == null)
        {
            Debug.Log("플레이어 스탯 테이블이 null 입니다");
            return;
        }
        
        PlayerStat stat = new PlayerStat();

        foreach (int targetId in TableManager.Instance.GetAllIds(playerStatTable))
        {
            PlayerConfigTableData playerConfigTableData = playerStatTable[targetId];

            if (playerConfigTableData != null)
            {
                int id = playerConfigTableData.id;
                string value = playerConfigTableData.configValue;
                stat.Apply((PlayerStatId)id, value);
            }
        }

        holder.Init(stat);
    }
}
