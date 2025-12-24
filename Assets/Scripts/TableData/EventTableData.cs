public class EventTableData : TableBase
{
    public int id { get; set; }
    public int groupId { get; set; }
    public string startType { get; set; }
    public string startValue { get; set; }
    public string conditionType1 { get; set; }
    public string conditionValue1 { get; set; }
    public string conditionType2 { get; set; }
    public string conditionValue2 { get; set; }
    public string eventType { get; set; }
    public string eventValue { get; set; }
    public string targetObject { get; set; }
    public string description { get; set; }
}
