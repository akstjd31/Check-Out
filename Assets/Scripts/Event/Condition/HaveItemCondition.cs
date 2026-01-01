
// 컨디션 = "haveitem" : 특정 아이템을 보유 중 체크, True이면 이벤트를 실행한다.
public class HaveItemCondition : IEventCondition
{
    public bool Check(string eventValue)
    {
        int itemId = int.Parse(eventValue);
        return InventoryManager.Instance.HaveItem(itemId);
    }
}
