using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    [SerializeField] ItemTableData itemTableData;

    public int ID => itemTableData.id;

    public ItemType Type => itemTableData.type;

    public string Name => itemTableData.name;

    public string Desc => itemTableData.desc;

    public bool Sellable => itemTableData.sellable;

    public int SellPrice => itemTableData.sellPrice;

    public int BuyPrice => itemTableData.buyPrice;

    public int Sell()
    {

        return SellPrice;
    }
    public int Buy()
    {

        return BuyPrice;
    }


}
