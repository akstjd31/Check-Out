using UnityEngine;

public class SanityIncrease : ItemEffect
{
    public SanityIncrease(string name, int value1, int value2, string controlKey)
    {
        EffectName = name;
        Value1 = value1;
        Value2 = value2;
        ControlKey = controlKey;

    }

    public override bool Use()
    {
        StatController playerStat = GameManager.Instance.GetPlayer().transform.GetComponent<StatController>();

        playerStat.ChangeSanity(false,-Value1);

        InventoryManager.Instance.RemoveInventoryItem();

        return true;
    }
}
