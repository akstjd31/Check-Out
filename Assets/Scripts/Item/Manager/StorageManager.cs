using UnityEngine;

public class StorageManager : Singletone<StorageManager>
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

    // 인벤토리에서 아이템 가져오기
    public void InventoryToStorage(ItemTableData item)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int storageIndex = -1;
        bool empty = storage.CheckEmptyStorage(out storageIndex);

        if (empty == false)
        {
            Debug.Log("창고가 꽉 찾을시 원하는 코드");
            return;
        }

        storage.ItemStorage(item, storageIndex);
    }

    // 인벤토리에 아이템 보내기
    public void StorageToInventory(int index)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("창고에서 인벤토리로 보낼때 꽉 찾을 시 코드");
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
