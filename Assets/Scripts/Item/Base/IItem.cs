using UnityEngine;

public enum ItemType
{
    General,
    Consumable,
    Gadget
}

public interface IItem
{
    int ID { get; }
    ItemType Type { get; }
    string Name { get; }
    string Desc { get; }
    bool Sellable { get; }
    int SellPrice { get; }
    int BuyPrice { get; }
}
