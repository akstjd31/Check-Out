using UnityEngine;
using System;
using System.Collections.Generic;

public class Item : Interactable, IItem
{
    public int ID { get; private set; }
    public ItemType ItemType { get; private set; }

    public string Name { get; private set; }

    public string Desc { get; private set; }

    public bool Sellable { get; private set; }

    public int SellPrice { get; private set; }

    public string ImgPath { get; private set; }

    private SpriteRenderer spriteRenderer;

    public ItemInstance item;

    private void Start()
    {
      
    }

    public void Init(ItemInstance itemInstance)
    {
        item = itemInstance;
        ID = item.itemdata.id;
        ItemType = (ItemType)Enum.Parse(typeof(ItemType), item.itemdata.itemType);
        Name = item.itemdata.itemName;
        gameObject.name = Name;
        Desc = item.itemdata.itemDescription;
        Sellable = item.itemdata.isCanSell;
        SellPrice = item.itemdata.sellPrice;
        ImgPath = item.itemdata.imgPath;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) return;

        Sprite sprite = Resources.Load<Sprite>(ImgPath);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        Debug.Log($"ID : {ID} 와 이름 : {Name} 이 성공적으로 들어갔습니다");
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
            ItemManager.Instance.ReturnItem(this);
        }
    }
}
