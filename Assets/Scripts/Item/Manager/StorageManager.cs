using UnityEngine;

public class StorageManager : Singleton<StorageManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private Storage storage;
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        //GetStorage();
        //GetInventory();
    }

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
    public void InventoryToStorage(int index)
    {
        if (storage == null) return;

        if (inventory == null) return;

        if (index < 0 || index >= inventory.slots.Length) return;

        if (inventory.slots[index] == null) return;

        int storageIndex = -1;
        bool empty = storage.CheckEmptyStorage(out storageIndex);

        if (empty == false)
        {
            Debug.Log("창고가 꽉 차 있습니다");
            return;
        }

        Item item = inventory.MoveItem(index);

        if (item == null) return;

        storage.ItemStorage(item, storageIndex);
    }

    // 창고에서 인벤토리로
    public void StorageToInventory(int index)
    {
        if (storage == null) return;

        if (storage.storageList == null) return;

        if (inventory == null) return;


        if (index < 0 || index >= storage.storageList.Length) return;

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 차있습니다");
            return;
        }

        Item item = storage.MoveItem(index);

        if (item == null) return;

        inventory.GetItem(item,inventoryIndex);
    }

    public void SelectItem()
    {

    }
}
