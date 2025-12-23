using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

enum ConditionTarget
{
    None,
    Switch,
    Sanity,
    Item
}

enum ConditionOperator
{
    None,
    On,
    Off,
    Above,
    Below,
    Have
}

public static class ConditionCheckingMachine
{
    static readonly Dictionary<string, ConditionTarget> TargetMap =
    new()
    {
        { "Switch", ConditionTarget.Switch },
        { "Sanity", ConditionTarget.Sanity },
        { "Item", ConditionTarget.Item }
    };

    static readonly Dictionary<string, ConditionOperator> OperatorMap =
    new()
    {
        { "On", ConditionOperator.On },
        { "Off", ConditionOperator.Off },
        { "Above", ConditionOperator.Above },
        { "Below", ConditionOperator.Below },
        { "Have", ConditionOperator.Have }
    };

    public static bool Check(EventTableData data)
    {
        // 두 조건을 모두 통과할 떄 이벤트 발생
        return CheckCondition(data.conditionType1, data.conditionValue1) &&
                CheckCondition(data.conditionType2, data.conditionValue2);
    }

    static bool CheckCondition(string type, string value)
    {
        // type이 none이면 value값도 none임 (자동 패스)
        if (type == "none")
            return true;

        var words = SplitWords(type);
        EventParam eventParam = EventParam.Parse(value);

        ConditionTarget target = ConditionTarget.None;
        ConditionOperator op = ConditionOperator.None;

        // 타입 비교
        // 중점이 되는 대상(target)과 연산(Operator)을 매핑
        foreach (var word in words)
        {
            if (TargetMap.TryGetValue(word, out var t))
                target = t;

            if (OperatorMap.TryGetValue(word, out var o))
                op = o;
        }

        // 조건에 부합하는지 확인
        return ExecuteCondition(target, op, eventParam);
    }

    // 실제 조건 확인
    static bool ExecuteCondition(ConditionTarget target, ConditionOperator op, EventParam evtParam)
    {
        switch (target)
        {
            // case ConditionTarget.Switch:
            //     return CheckSwitch(op, value);

            case ConditionTarget.Sanity:
                if (op.Equals(ConditionOperator.Below))
                {
                    GameObject player = GameManager.Instance.GetPlayer();
                    PlayerSanity sanity = player.GetComponent<PlayerSanity>();
                    return sanity.IsSanityBelow(evtParam.id);
                }
                else
                {
                    return true;
                }
        }

        return false;
    }

    // 컨디션조건을 대소문자로 분리
    static string[] SplitWords(string input)
    {
        return Regex.Matches(input, @"[A-Z][a-z]*|[a-z]+")
                    .Select(m => m.Value)
                    .ToArray();
    }
}
