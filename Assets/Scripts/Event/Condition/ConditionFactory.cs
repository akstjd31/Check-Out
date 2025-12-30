using UnityEngine;
/// <summary>
/// 시작 조건에 따른 객체 생성 클래스
/// </summary>
public static class ConditionFactory
{
    public static IEventCondition Create(string conditionType)
    {
        switch (conditionType)
        {
            case "checkSwitchOff":
                return new CheckSwitchOffCondition();
            case "checkSwitchOn":
                return new CheckSwitchOnCondition();
        }

        return null;
    }
}
