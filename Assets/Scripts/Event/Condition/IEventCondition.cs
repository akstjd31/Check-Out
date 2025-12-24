
// 컨디션 조건 관련 인터페이스
public interface IEventCondition
{   
    // 매개변수는 특정 경로나 ID값이 들어갈 수 있음.
    bool Check(string eventValue);
}
