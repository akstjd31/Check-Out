/// <summary>
/// 실행될 이벤트 관련 인터페이스
/// </summary>
public interface IEventAction
{
    void Execute(string eventValue, string targetObject);
}