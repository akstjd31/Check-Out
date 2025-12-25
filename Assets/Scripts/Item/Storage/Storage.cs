using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int storageSize = 16;
    [SerializeField] public ItemInstance[] storageList;
    public Action<int> OnSlotUpdated;

    public int GetDefaultStorageSize() => storageSize;

    public void SetStorage(int size)
    {
        if (size < 1)
        {
            Debug.Log("크기가 최소 1 이상이여야 함");
            return;
        }

        storageList = new ItemInstance[size];
    }

    // 창고가 비어있는지 체크
    public bool CheckEmptyStorage(out int index)
    {
        index = -1;

        if (storageList == null)
            return false;

        // First - fit 방식으로 처음 발견된 곳에 아이템을 넣는다
        for (int i = 0; i < storageList.Length; i++)
        {
            if (storageList[i] != null)
                continue;

            index = i;
            return true;
        }

        return false;
    }

    // 창고에 아이템 넣기
    public void ItemStorage(ItemInstance item, int index)
    {
        if (item == null)
            return;
        Debug.Log("일단 아이템은 null이 아님");

        if (storageList == null)
            return;
        Debug.Log("스토리지 리스트도 null이 아님");

        if (index < 0 || index >= storageList.Length) return;
        Debug.Log("인덱스도 잘 들어갔음");

        if (storageList[index] != null) return;

        Debug.Log($"아이템: {item.itemdata.itemName}, 인덱스: {index}");

        storageList[index] = item;
        OnSlotUpdated?.Invoke(index);
    }

    // 아이템 이동(인벤토리에 넣기)
    public ItemInstance MoveItem(int index)
    {
        if (storageList == null)
        {
            Debug.Log("1");
            return null;
        }

        if (index < 0 || index >= storageList.Length)
        {
            Debug.Log("2");
            return null;
        }

        if (storageList[index] == null)
        {
            Debug.Log("3");
            return null;
        }


        ItemInstance item = storageList[index];

        RemoveItem(index);
        OnSlotUpdated?.Invoke(index);
        return item;
    }

    // 아이템이 이동했을 시
    private void RemoveItem(int index)
    {
        if (storageList == null)
            return;

        if (index < 0 || index >= storageList.Length)
            return;

        if (storageList[index] == null)
            return;

        storageList[index] = null;
    }
}
