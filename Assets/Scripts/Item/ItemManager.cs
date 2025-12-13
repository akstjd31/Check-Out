using System.Net.NetworkInformation;
using UnityEngine;

public class ItemManager : Singletone<ItemManager>
{
    [SerializeField] private Item[] items;
    
    ObjPool<Item> itemPool;

    private void Start()
    {
        SetPool();
    }

    private void SetPool()
    {
        if (items == null)
            return;
        foreach (var item in items)
        {
            itemPool.CreatePool(item, 10, transform);
        }
    }
}
