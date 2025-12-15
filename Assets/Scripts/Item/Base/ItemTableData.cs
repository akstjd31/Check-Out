using UnityEngine;

public class ItemTableData : TableBase
{
    public int id { get; set; }
    public ItemType type { get; set; }
    public string name { get; set; }
    public string desc { get; set; }
    public bool sellable { get; set; }
    public int sellPrice { get; set; }
    public int buyPrice { get; set; }
}
