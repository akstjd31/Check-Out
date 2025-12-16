using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private GameObject player;
    [SerializeField ]private Transform playerHandTransform;

    private Inventory inventory;
    
    private void Awake()
    {
    }

    public void GetInventory()
    {
        if (player == null)
            return;

        inventory = player.GetComponentInChildren<Inventory>();
    }

    // 아이템 줍기
    public void PickUpItem(ItemTableData item)
    {
        if (inventory == null) return;

        int inventoryIndex = -1;
        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 찼습니다");
            return;
        }

        inventory.GetItem(item, inventoryIndex);
    }

    // 아이템 버리기
    public void DropItem(int index)
    {
        if (inventory == null) return;

        if (player == null) return;

        ItemTableData item = inventory.MoveItem(index);

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

        Debug.Log(currentItem);

        if (currentItem == null)
        {
            Debug.Log("아이템이 없습니다");
            return;
        }
          
        // 부가기능
        // HandItem(currentItem);

        return;
    }

    // 손에 들고있는 아이템
   // public void HandItem(Item item)
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
