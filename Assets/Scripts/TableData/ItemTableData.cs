public class ItemTableData : TableBase
{
    public int id { get; set; }
    public string itemType { get; set; }
    public string itemName { get; set; }
    public string itemDescription { get; set; }
    public bool isCanSell { get; set; }
    public int sellPrice { get; set; }
    public string imgPath { get; set; }
    public int itemEffect { get; set; }
}
