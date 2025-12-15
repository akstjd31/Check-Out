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

    // ������ �ݱ�
    public void PickUpItem(ItemTableData item)
    {
        if (inventory == null) return;

        int inventoryIndex = -1;
        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("�κ��丮 �� á�� �� ���ϴ� �ڵ�");
            return;
        }

        inventory.GetItem(item, inventoryIndex);
    }

    // ������ ������
    public void DropItem(int index)
    {
        if (inventory == null) return;

        if (player == null) return;

        ItemTableData item = inventory.MoveItem(index);

        if (item == null) return;
    }
    
    // ���� �ø��ų� ������ �κ��丮 ���� ���� ��
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
            Debug.Log("�κ��丮�� �������� �����ϴ�");
            return;
        }
          
        // �κ��丮�� �������� �����ҽ� �տ� ǥ��
        // HandItem(currentItem);

        return;
    }

    // �տ� ǥ���� ������
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
