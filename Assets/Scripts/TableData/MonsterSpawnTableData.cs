public class MonsterSpawnTableData : TableBase
{
    public int id { get; set; }
    public int locationX { get; set; }
    public int locationY { get; set; }
    public int locationZ { get; set; }
    public bool isRandom { get; set; }
    public string monsterId { get; set; }
}
