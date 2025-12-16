using UnityEngine;

public class Item : MonoBehaviour, IItem
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

    public int Sell()
    {

        return SellPrice;
    }
    public int Buy()
    {

        return BuyPrice;
    }


}
