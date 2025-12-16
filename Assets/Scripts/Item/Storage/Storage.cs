using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int storageSize = 16;
    [SerializeField] public Item[] storageList;

    private void Start()
    {
        SetStorage(storageSize);
    }

    public void SetStorage(int size)
    {
        if (size < 1)
        {
            Debug.Log("크기가 최소 1 이상이여야 함");
            return;
        }

        storageList = new Item[size];
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
    public void ItemStorage(Item item, int index)
    {
        if (item == null)
            return;

        if (storageList == null)
            return;

        if (index < 0 || index >= storageList.Length) return;

        if (storageList[index] != null) return;

        storageList[index] = item;
    }

    // 아이템 이동(인벤토리에 넣기)
    public Item MoveItem(int index)
    {
        if (storageList == null)
            return null;

        if (index < 0 || index >= storageList.Length)
            return null;

        if (storageList[index] == null)
            return null;

        Item item = storageList[index];

        RemoveItem(index);

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
