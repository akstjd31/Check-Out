using UnityEngine;

public enum ItemType
{
    General = 1,
    Consumable = 2,
    Gadget = 3
}

public interface IItem
{
    int ID { get; }
    ItemType Type { get; }
    string Name { get; }
    string Desc { get; }
    bool Sellable { get; }
    int SellPrice { get; }
    string ImgPath { get; }
}
