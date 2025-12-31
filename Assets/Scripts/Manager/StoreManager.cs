using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
    private Store store;
    // private GameObject player;
    private Inventory inventory;
    private AudioSource audioSource;

    private Dictionary<int, ShopTableData> dataID;
    public bool IsInitialized { get; private set; }
    public bool IsOpen { get; set; }

    protected override void Awake()
    {
        base.Awake();
        dataID = new Dictionary<int, ShopTableData>();
        store = FindAnyObjectByType<Store>();
        inventory = FindAnyObjectByType<Inventory>();

        audioSource = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (IsInitialized) return;

        var shopTable = TableManager.Instance.GetTable<int, ShopTableData>();

        foreach (var shopId in TableManager.Instance.GetAllIds(shopTable))
        {
            ShopTableData shopTableData = shopTable[shopId];
            dataID[shopTableData.id] = shopTableData;
            store.SetShopList(shopTableData);
        }

        IsInitialized = true;
    }

    public int GetItemListSize() => dataID.Count;

    private void PlayBuyFailedSound()
    {
        audioSource.clip = SoundManager.Instance.GetBuyItemFailedClip();

        if (audioSource.clip != null)
            audioSource.Play();
    }

    private void PlayBuySound()
    {
        audioSource.clip = SoundManager.Instance.GetBuyItemClip();
        if (audioSource.clip != null)
            audioSource.Play();
    }

    private void PlaySellSound()
    {
        audioSource.clip = SoundManager.Instance.GetSellItemClip();

        if (audioSource.clip != null)
            audioSource.Play();
    }

    // 아이템 구매
    public void BuyItem(ShopTableData shopItem)
    {
        if (shopItem == null) return;

        if (store == null) return;

        if (inventory == null) return;

        if (dataID.TryGetValue(shopItem.id, out var data) == false) return;

        int price = store.GetBuyPrice(data);

        bool canBuy = CheckMoney(price);

        if (canBuy == false)
        {
            PlayBuyFailedSound();
            return;
        }

        int inventoryIndex = -1;

        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            PlayBuyFailedSound();
            return;
        }

        var item = ItemManager.Instance.Createinstance(data.itemId);

        InventoryManager.Instance.PickUpItem(item);

        GameManager.Instance.ChangeMoney(-price);
        
        PlayBuySound();
    }

    // 아이템 판매
    public void SellItem(int index)
    {

        if (inventory == null) return;

        if (store == null) return;

        if (inventory.slots == null) return;

        if (index < 0 || index >= inventory.slots.Length) return;

        if (inventory.slots[index] == null) return;

        var item = InventoryManager.Instance.MoveItem(index);
        //debug.Log(item.itemdata.itemName);

        if (item == null) return;

        int price = store.GetSellPrice(item);
        //debug.Log($"가격 {price}");

        GameManager.Instance.ChangeMoney(price);

        PlaySellSound();
    }

    // 돈 충분한지 체크
    public bool CheckMoney(int price)
    {
        //if (player == null) return false;

        //if (player.money > price)
        //return true;

        if (GameManager.Instance.Money >= price)
            return true;

        return false;
    }
}
