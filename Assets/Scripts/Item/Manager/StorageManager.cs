using UnityEngine;

public class StorageManager : Singleton<StorageManager>
{
    [SerializeField] private GameObject player;
    private Storage storage;
    private Inventory inventory;

    public void GetStorage()
    {
        if (player == null)
            return;

        storage = player.GetComponentInChildren<Storage>();
    }
    public void GetInventory()
    {
        if (player == null)
            return;

        inventory = player.GetComponentInChildren<Inventory>();
    }

    // 인벤토리에서 창고로
    public void InventoryToStorage(ItemTableData item)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int storageIndex = -1;
        bool empty = storage.CheckEmptyStorage(out storageIndex);

        if (empty == false)
        {
            Debug.Log("창고가 꽉 차 있습니다");
            return;
        }

        storage.ItemStorage(item, storageIndex);
    }

    // 창고에서 인벤토리로
    public void StorageToInventory(int index)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 차있습니다");
            return;
        }

        ItemTableData item = storage.MoveItem(index);

        if (item == null) return;

        inventory.GetItem(item,inventoryIndex);
    }

    public void SelectItem()
    {

    }
}
