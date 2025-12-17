using UnityEngine;
using System;

public class Item : Interactable, IItem
{
    public int ID { get; private set; }
    public ItemType Type { get; private set; }

    public string Name { get; private set; }

    public string Desc { get; private set; }

    public bool Sellable { get; private set; }

    public int SellPrice { get; private set; }

    public string ImgPath { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(ItemTableData itemTable)
    {
        ID = itemTable.id;
        Type = (ItemType)Enum.Parse(typeof(ItemType), itemTable.itemType);
        Name = itemTable.itemName;
        Desc = itemTable.itemDescription;
        Sellable = itemTable.isCanSell;
        SellPrice = itemTable.sellPrice;
        ImgPath = itemTable.imgPath;

        Sprite sprite = Resources.Load<Sprite>(ImgPath);
        //spriteRenderer.sprite = sprite;
    }

    public override void OnFocusEnter()
    {
        // 상호작용할 키나 하이라이트 기능
        promptText = "E key press";
    }

    public override void OnFocusExit()
    {
        // 빠져나갔을때
        promptText = "";
    }

    public override void Interact()
    {
        bool success = InventoryManager.Instance.PickUpItem(this);

        if (success)
        {

        }
    }
}
