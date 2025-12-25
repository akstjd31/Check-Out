using UnityEngine;

public class Light : ItemEffect
{
    public Light(string name, int value1, int value2, string controlKey)
    {
        EffectName = name;
        Value1 = value1;
        Value2 = value2;
        ControlKey = controlKey;
    }
}
