using System;
using Unity.VisualScripting;
using UnityEngine;

public enum IDType
{
    None,
    Item,           // 1001 ~ 1999
    Shop,           // 2001 ~ 2999
    ItemSpawn,      // 3001 ~ 3999
    ItemGroup,      // 4001 ~ 4999
    MonsterSpawn,   // 5001 ~ 5999
    Effect,         // 6001 ~ 6999
    EffectGroup,    // 7001 ~ 7999
    Event,          // 8001 ~ 8999
    Switch,         // 9001 ~ 9999
    Talk,           // 100001 ~ 199999
    String          // 200001 ~ 299999
}

// ID값에 따른 타입 판별
public struct EventParam
{
    public IDType type;
    public int id;

    public static EventParam Parse(string value)
    {
        IDType typeId = IDType.None;
        int num = int.Parse(value);

        if (1001 <= num && num <= 1999)
            typeId = IDType.Item;
        
        if (2001 <= num && num <= 2999)
            typeId = IDType.Shop;

        if (3001 <= num && num <= 3999)
            typeId = IDType.ItemSpawn;
        
        if (4001 <= num && num <= 4999)
            typeId = IDType.ItemGroup;
        
        if (5001 <= num && num <= 5999)
            typeId = IDType.MonsterSpawn;

        if (6001 <= num && num <= 6999)
            typeId = IDType.Effect;
        
        if (7001 <= num && num <= 7999)
            typeId = IDType.EffectGroup;

        if (8001 <= num && num <= 8999)
            typeId = IDType.Event;

        if (9001 <= num && num <= 9999)
            typeId = IDType.Switch;

        if (100001 <= num && num <= 199999)
            typeId = IDType.Talk;
        
        if (200001 <= num && num <= 299999)
            typeId = IDType.String;

        return new EventParam { type = typeId,  id = num};
    }

}
