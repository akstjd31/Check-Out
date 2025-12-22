using System;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    // [SerializeField] private Transform playerHandTransform;

    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI invenUI;

    //private Vector3 playerPos;

    private string fileName = "InventorySaveData.json";

    protected override void Awake()
    {
        base.Awake();
        
        inventory = this.GetComponent<Inventory>();

        int size = inventory.GetDefaultInventorySize();
        inventory.SetInventory(size);
    }

    public void InventoryDebug()
    {
        Debug.Log($"인벤토리 크기: {inventory.slots.Length}");
    }

    // 인벤토리 저장 기능
    public bool SaveInventory()
    {
        SlotSaveData saveData = new SlotSaveData();

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i] == null) continue;

            saveData.slots.Add(new SlotData
            {
                index = i,
                itemId = inventory.slots[i].id
            });
        }

        SaveLoadManager.Instance.Save(fileName, saveData);
        Debug.Log("인벤토리 데이터 저장 완료!");
        return true;
    }

    // 인벤토리 불러오기
    public bool LoadInventory()
    {
        SlotSaveData saveData =
            SaveLoadManager.Instance.Load<SlotSaveData>(fileName);

        if (saveData == null) return false;

        foreach (var slot in saveData.slots)
        {
            ItemTableData item = ItemManager.Instance.GetItemData(slot.itemId);
            inventory.GetItem(item, slot.index);
            Debug.Log($"현재 불러온 아이템: {item.itemName}");
        }

        Debug.Log("인벤토리 데이터 로드 완료!");
        return true;
    }

    // 아이템 줍기
    public bool PickUpItem(Item item)
    {
        if (inventory == null)
        {
            Debug.Log("인벤토리가 업성");
            return false;
        }

        int inventoryIndex = -1;
        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 찼습니다");
            return false;
        }

        Debug.Log($"{inventoryIndex} 번째 칸의 인벤토리에 넣는 중");
        inventory.GetItem(item.data, inventoryIndex);
        Debug.Log($"{item.Name}을 성공적으로 넣었습니다");

        return true;
    }

    // 테스트용
    public void TestPickUp(Item item)
    {
        PickUpItem(item);
    }

    // 아이템 버리기
    public void DropItem(int index)
    {
        if (inventory == null) return;
        Debug.Log("성공적 1");
        if (GameManager.Instance.GetPlayer() == null) return;
        Debug.Log("성공적 2");

        ItemTableData item = inventory.MoveItem(index);
        Debug.Log("성공적 6");

        if (item == null) return;

        ItemManager.Instance.SpawnItem(item.id, GameManager.Instance.GetPlayer().transform.up);
    }
    
    // 인벤토리 슬롯 선택
    public void SelectInventory(int index)
    {
        if (inventory == null)
            return;

        if (index < 0 || index >= inventory.slots.Length)
            return;

        ItemTableData currentItem = inventory.slots[index];

        if (currentItem == null)
            return;
          
        // 부가기능
        // HandItem(currentItem);

        return;
    }

    // 손에 들고있는 아이템
    // public void HandItem(ItemTable item)
    // {
    //    if (item == null)
    //         return;

    //    if (player == null)
    //         return;

    //     if (playerHandTransform == null)
    //         return;

    //     item.gameObject.SetActive(true);
    //     item.transform.position = playerHandTransform.position;
    // }

    // public void UpdateInventory(int index)
    // {
    //
    // }
}
