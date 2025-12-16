using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] ItemTableData[] itemList;
    [SerializeField] int storeSize = 5;

    
    public void SetItemList(int size)
    {
        itemList = new ItemTableData[size];
    }
   
    // 아이템 구매 가격
    public int GetBuyPrice(ItemTableData item)
    {
        if (item == null) return 0;
        return item.buyPrice;
    }

    // 아이템 판매 가격
    public int GetSellPrice(ItemTableData item)
    {
        if (item == null) return 0;

        return item.sellPrice;
    }
}
