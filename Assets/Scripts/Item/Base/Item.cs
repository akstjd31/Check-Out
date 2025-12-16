using System;
using UnityEngine;

public class Item : Interactable, IItem
{
    [SerializeField] ItemTableData itemTableData;

    [SerializeField] private int sellPrice;
    [SerializeField] private int buyPrice;

    public int ID => itemTableData.id;

    public ItemType Type => (ItemType)Enum.Parse(typeof(ItemType),itemTableData.itemType);

    public string Name => itemTableData.itemName;

    public string Desc => itemTableData.itemDescription;

    public bool Sellable => itemTableData.isCanSell;

    public int SellPrice => sellPrice;

    public int BuyPrice => buyPrice;
    
    public override void OnFocusEnter()
    {
        // 상호작용할 키나 하이라이트 기능
        text.text = "E key press";
    }

    public override void OnFocusExit()
    {
        // 빠져나갔을때
        text.text = "";
    }

    public override void Interact()
    {
        bool success = InventoryManager.Instance.PickUpItem(this);

        if (success)
        {

        }
    }
}
