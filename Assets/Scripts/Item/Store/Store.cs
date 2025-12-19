using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public List<ShopTableData> shopList;

    private void Awake()
    {
        shopList = new List<ShopTableData>();
    }

    public void SetShopList(ShopTableData data)
    {
        shopList.Add(data);
    }

    // 아이템 구매 가격
    public int GetBuyPrice(ShopTableData itemid)
    {
        if (shopList.Contains(itemid) == false) return 0;

        return itemid.buyPrice;
    }

    // 아이템 판매 가격
    public int GetSellPrice(ItemTableData item)
    {
        if (item == null) return 0;

        return item.sellPrice;
    }
}
