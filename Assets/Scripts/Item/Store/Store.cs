using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] Item[] itemList;
    [SerializeField] int storeSize = 5;

    
    public void SetItemList(int size)
    {
        itemList = new Item[size];
    }
   
    // 아이템 구매 가격
    public int GetBuyPrice(Item item)
    {
        if (item == null) return 0;
        return item.BuyPrice;
    }

    // 아이템 판매 가격
    public int GetSellPrice(Item item)
    {
        if (item == null) return 0;

        return item.SellPrice;
    }
}
