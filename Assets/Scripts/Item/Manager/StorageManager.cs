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

    // �κ��丮���� ������ ��������
    public void InventoryToStorage(ItemTableData item)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int storageIndex = -1;
        bool empty = storage.CheckEmptyStorage(out storageIndex);

        if (empty == false)
        {
            Debug.Log("â���� �� ã���� ���ϴ� �ڵ�");
            return;
        }

        storage.ItemStorage(item, storageIndex);
    }

    // �κ��丮�� ������ ������
    public void StorageToInventory(int index)
    {
        if (storage == null) return;

        if (inventory == null) return;

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("â������ �κ��丮�� ������ �� ã�� �� �ڵ�");
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
