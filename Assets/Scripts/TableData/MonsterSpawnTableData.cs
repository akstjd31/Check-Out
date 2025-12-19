public class MonsterSpawnTableData : TableBase
{
    public int id { get; set; }
    public float locationX { get; set; }
    public float locationY { get; set; }
    public float locationZ { get; set; }
    public bool isRandom { get; set; }
    public string monsterId { get; set; }
}
