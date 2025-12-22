// 이벤트 테이블 임시 데이터
public class EventTableData : TableBase
{
    public int id;
    public int groupId;
    public string startType;            // 이벤트 시작 발동 조건 타입
    public string startValue;           // 발동 조건의 세부 값
    public string conditionTypeFirst;   // 첫 번째 검사 조건
    public string conditionValueFirst;  // 첫 번째 조건의 비교 값
    public string conditionTypeSecond;  // 두 번째 검사 조건
    public string conditionValueSecond; // 두 번째 조건의 비교 값
    public string eventType;            // 조건 충족 시 실행할 액션의 종류
    public string eventValue;           // 실행할 액션의 파라미터
    public string targetObject;         // 액션이 수행될 대상 오브젝트의 식별자 이름 or 경로
    public string description;          // 설명
}
