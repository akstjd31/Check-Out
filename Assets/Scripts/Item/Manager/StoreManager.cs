using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
    [SerializeField] int money = 1000;
    [SerializeField] private Store store;
    // private GameObject player;
    [SerializeField] private Inventory inventory;

    // 아이템 구매
    public void BuyItem(Item item)
    {
        if (item == null) return;

        if (store == null) return;

        if (inventory == null) return;

        int price = store.GetBuyPrice(item);

        bool canBuy = CheckMoney(price);

        if (canBuy == false) return;

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false) return;

        inventory.GetItem(item, inventoryIndex);

        //player.Money -= price;
        money -= price;
    }

    // 아이템 판매
    public void SellItem(int index)
    {

        if (inventory == null) return;

        if (store == null) return;

        if (inventory.slots == null) return;

        if (index < 0 || index >= inventory.slots.Length) return;

        if (inventory.slots[index] == null) return;

        Item item = inventory.MoveItem(index);

        if (item == null) return;

        int price = store.GetSellPrice(item);

        //player.money += price;
        money += price;
    }

    // 돈 충분한지 체크
    public bool CheckMoney(int price)
    {
        //if (player == null) return false;

        //if (player.money > price)
            //return true;

        if (money > price)
            return true;

        return false;
    }
}
