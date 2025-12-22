using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private Item itemPrefab;

    private ObjPool<Item> itemPool;
    private Dictionary<int, ItemTableData> dataID;


    protected override void Awake()
    {
        base.Awake();
        itemPool = new ObjPool<Item>();
        dataID = new Dictionary<int, ItemTableData>();

        Init();
        if (itemPrefab == null) return;

        itemPool.CreatePool(itemPrefab, 10, transform);
    }

    public void Init()
    {
        var itemTable = TableManager.Instance.GetTable<int, ItemTableData>();

        if (itemTable == null)
        {
            Debug.Log("아이템 테이블이 null 입니다");
            return;
        }

        foreach (var targetId in TableManager.Instance.GetAllIds(itemTable))
        {
            ItemTableData ItemTableData = itemTable[targetId];

            if (ItemTableData != null)
            {
                dataID[ItemTableData.id] = ItemTableData;
            }
        }
    }

    public void Test(int itemid)
    {
        SpawnItem(itemid + 1000, transform.position);
    }

    public Item SpawnItem(int itemID, Vector3 pos)
    {
        if (dataID.TryGetValue(itemID, out var data) == false) return null;

        Item item = itemPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Init(data);
        return item;
    }

    public void ReturnItem(Item item)
    {
        Debug.Log(item);
        itemPool.ReturnObject(itemPrefab, item);
    }

    // 스포너에 의해 생성된 아이템들 씬 전환 전에 회수하는 작업 
    public void ReturnAllItem()
    {
        var items = GetComponentsInChildren<Item>(true);

        foreach (var item in items)
        {
            if (item != null)
                itemPool.ReturnObject(itemPrefab, item);
        }
    }


    public ItemTableData GetItemData(int itemid)
    {
        if (dataID.TryGetValue(itemid, out var item) == false) return null;

        return item;
    }
}
