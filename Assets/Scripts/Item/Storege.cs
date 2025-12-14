using Unity.VisualScripting;
using UnityEngine;

public class Storege : MonoBehaviour
{
    [SerializeField] private int storageSize = 16;
    [SerializeField] Item[] storageList;

    private InventoryManager inventory;
    private Item currentItem;

    private void Start()
    {
        storageList = new Item[storageSize];
        inventory = InventoryManager.Instance;
    }

    // 필요하면 bool로 체크
    public void ItemStorage(int currentIndex)
    {
        if (inventory == null)
            return;

        if (currentIndex < 0 || currentIndex >= inventory.InventorySize)
            return;

        Item item = inventory.items[currentIndex];

        if (item == null)
            return;

        if (storageList == null)
            return;

        bool emptyStorage = false;
        int index = 0;

        // First - fit 방식으로 처음 발견된 곳에 아이템을 넣는다
        for (int i = 0; i < storageList.Length; i++)
        {
            if (storageList[i] != null)
                continue;

            emptyStorage = true;
            index = i;
            break;
        }

        if (emptyStorage)
        {
            storageList[index] = item;
            inventory.items[currentIndex] = null;
        }
            

        else
        {
            Debug.Log("창고가 꽉 찼습니다");
        }    
    }

    // 창고내에 인덱스 위치에 있는 아이템을 인벤토리에 넣는 기능
    public void ExpertItem(int index)
    {
        if (inventory  == null)
            return;

        if (index < 0 || index >= storageList.Length)
            return;

        if (storageList[index] == null)
            return;

        bool emptyInventory = false;

        for (int i = 0; i < inventory.InventorySize; i++)
        {
            if (inventory.items[i] != null)
                continue;

            emptyInventory = true;
            break;
        }

        if (emptyInventory)
        {
            inventory.GetItem(storageList[index]);
            storageList[index] = null;
        }

        else
            Debug.Log("인벤토리가 꽉 찼습니다");
    }
}
