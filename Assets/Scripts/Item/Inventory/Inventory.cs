using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] slots;

    private int inventorySize = 4;


    private void Start()
    {
        SetInventory(inventorySize);
    }

    public void SetInventory(int size)
    {
        if (size < 1)
        {
            Debug.Log("크기가 최소 1 이상이여야 함");
            return;
        }
            
        slots = new Item[size];
    }

    // 인벤토리가 비어있는지 체크
    public bool CheckEmpty(out int index)
    {
        index = -1;

        if (slots == null)
        {
            Debug.Log("인벤토리 자체가 없습니다");
            return false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                continue;

            // 인벤토리가 비어있을 시

            index = i;
            Debug.Log($"{index}번째 칸이 비어있음");
            return true;
        }

        return false;
    }

    // 아이템 가져오기
    public void GetItem(Item item, int index)
    {
        if (slots == null)
        {
            Debug.Log("인벤토리 자체가 없습니다");
            return;
        }

        if (item == null)
        {
            Debug.Log("아이템이 없습니다");
            return;
        }

        if (index < 0 || index >= slots.Length)
            return;

        if (slots[index] != null)
        {
            Debug.Log("이미 아이템이 들어있습니다");
            return;
        }
             
        slots[index] = item;
        return;
    }

    // 아이템 이동(버리거나 창고)
    public Item MoveItem(int index)
    {
        if (slots == null)
            return null;

        if (index < 0 || index >= slots.Length)
            return null;

        if (slots[index] == null)
            return null;

        Item item = slots[index];

        RemoveItem(index);

        return item;
    }

    // 배열에서 아이템 제거
    private void RemoveItem(int index)
    {
        if (slots == null)
            return;

        if (index < 0 || index >= slots.Length)
            return;

        if (slots[index] == null)
            return;

        slots[index] = null;
    }
}
