using UnityEngine;

public class Item : Interactable, IItem
{
    [SerializeField] ItemTableData itemTableData;

    [SerializeField] private int sellPrice;
    [SerializeField] private int buyPrice;

    public int ID => itemTableData.id;

    public ItemType Type => itemTableData.type;

    public string Name => itemTableData.name;

    public string Desc => itemTableData.desc;

    public bool Sellable => itemTableData.sellable;

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
