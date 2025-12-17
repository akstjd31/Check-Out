using UnityEngine;
public class ItemGroupTableData : TableBase
{
    [SerializeField] ItemTableData itemTable;
    public int id { get; set; }
    public int itemGroup { get; set; }
    public int itemId { get; set; }
    public int probability { get; set; }
}
