using System;
using UnityEngine;

public class StorageManager : Singleton<StorageManager>
{
    [SerializeField] private Storage storage;
    [SerializeField] private StorageUI storageUI;
    [SerializeField] private Inventory inventory;
    public bool IsOpen { get; set; }
    private string fileName = "StorageSaveData.json";

    protected override void Awake()
    {
        base.Awake();
        
        storage = FindAnyObjectByType<Storage>();
        inventory = FindAnyObjectByType<Inventory>();
    }

    // 창고 저장 기능
    public void SaveStorage()
    {
        SlotSaveData saveData = new SlotSaveData();

        for (int i = 0; i < storage.storageList.Length; i++)
        {
            if (storage.storageList[i] == null) continue;

            saveData.slots.Add(new SlotData
            {
                index = i,
                itemId = storage.storageList[i].id
            });
        }

        SaveLoadManager.Instance.Save(fileName, saveData);
        Debug.Log("창고 데이터 저장 완료!");
    }

    // 창고 불러오기
    public void LoadStorage()
    {
        SlotSaveData saveData = 
            SaveLoadManager.Instance.Load<SlotSaveData>(fileName);
        
        if (saveData == null) return;

        foreach (var slot in saveData.slots)
        {
            ItemTableData item = ItemManager.Instance.GetItemData(slot.itemId);
            storage.ItemStorage(item, slot.index);
        }

        Debug.Log("창고 데이터 로드 완료!");
    }

    // 창고 가져오기
    public Storage GetStorage() => storage;

    // 인벤토리 가져오기
    public Inventory GetInventory() => inventory;

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

        ItemTableData item = inventory.MoveItem(index);

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

        ItemTableData item = storage.MoveItem(index);

        if (item == null) return;

        inventory.GetItem(item,inventoryIndex);
    }

    public void SelectItem()
    {

    }
}
