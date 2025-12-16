using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerHandTransform;

    [SerializeField] private Inventory inventory;
    

    private void Start()
    {
        //GetInventory();
    }

    public void GetInventory()
    {
        if (player == null)
            return;

        inventory = player.GetComponentInChildren<Inventory>();
    }

    // 아이템 줍기
    public void PickUpItem(Item item)
    {
        if (inventory == null)
        {
            Debug.Log("인벤토리가 업성");
            return;
        }

        int inventoryIndex = -1;
        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 찼습니다");
            return;
        }

        Debug.Log($"{inventoryIndex} 번째 칸의 인벤토리에 넣는 중");
        inventory.GetItem(item, inventoryIndex);
        Debug.Log($"{item.name}을 성공적으로 넣었습니다");
    }

    // 아이템 버리기
    public void DropItem(int index)
    {
        if (inventory == null) return;

        if (player == null) return;

        Item item = inventory.MoveItem(index);

        if (item == null) return;
    }
    
    // 인벤토리 슬롯 선택
    public void SelectInventory(int index)
    {
        if (inventory == null)
            return;

        if (index < 0 || index >= inventory.slots.Length)
            return;

        Item currentItem = inventory.slots[index];

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
