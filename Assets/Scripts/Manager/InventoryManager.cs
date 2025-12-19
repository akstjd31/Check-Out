using System;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerHandTransform;

    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI invenUI;
    public event Action<int> test;

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
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

        test?.Invoke(inventoryIndex);
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
        if (player == null) return;
        Debug.Log("성공적 2");

        ItemTableData item = inventory.MoveItem(index);
        Debug.Log("성공적 6");

        if (item == null) return;

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
