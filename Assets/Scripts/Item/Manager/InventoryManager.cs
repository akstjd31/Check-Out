using UnityEngine;

public class InventoryManager : Singletone<InventoryManager>
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
            Debug.Log("인벤토리 꽉 찼을 시 원하는 코드");
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
    
    // 휠을 올리거나 내려서 인벤토리 선택 했을 시
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
            Debug.Log("인벤토리에 아이템이 없습니다");
            return;
        }
          
        // 인벤토리에 아이템이 존재할시 손에 표시
        // HandItem(currentItem);

        return;
    }

    // 손에 표시할 아이템
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
