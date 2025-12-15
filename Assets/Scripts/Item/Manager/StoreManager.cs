using UnityEngine;

public class StoreManager : Singletone<StorageManager>
{
    private Store store;
    // private GameObject player;
    private Inventory inventory;

    public void BuyItem(ItemTableData item)
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
    }

    public void SellItem(int index)
    {

        if (inventory == null) return;

        if (store == null) return;

        if (inventory.slots == null) return;

        if (index < 0 || index >= inventory.slots.Length) return;

        if (inventory.slots[index] == null) return;

        int price = store.GetSellPrice(inventory.slots[index]);

        //player.money += price;
    }

    public bool CheckMoney(int price)
    {
        //if (player == null) return false;

        //if (player.money > price)
            //return true;

        return false;
    }
}
