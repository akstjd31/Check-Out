using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class GadgetReload : ItemEffect
{
    private Inventory inventory;

    public GadgetReload(string name,int value1, int value2, string controlKey)
    {
        EffectName = name;
        Value1 = value1;
        Value2 = value2;
        ControlKey = controlKey;
    }

    public override bool Use(out int value)
    {
        value = 0;
        inventory = InventoryManager.Instance.GetInvetory();

        foreach(var slot in inventory.slots)
        {
            if (slot.itemdata.id == Value1)
            {
                value = Value2;
                return true;
            }
        }

        return false;
    }
}
