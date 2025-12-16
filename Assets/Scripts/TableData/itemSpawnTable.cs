using UnityEngine;
public class itemSpawnTable : TableBase
{
    [SerializeField] ItemGroupTableData itemGroupTable;
    public int id { get; set; }
    public float locationX { get; set; }
    public float locationY { get; set; }
    public float locationZ { get; set; }
    public int itemGroup => itemGroupTable.id;
}
