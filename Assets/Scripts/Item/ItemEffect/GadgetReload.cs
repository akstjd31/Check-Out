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
        
        int index = 0;
        value = 0;
        if (inventory == null)
            inventory = InventoryManager.Instance.GetInvetory();

        Debug.Log(inventory);

        if (inventory.slots == null)
            return false;

        foreach(var slot in inventory.slots)
        {
            if (slot == null)
            {
                index++;
                continue;
            }

            if (slot.itemdata.id == Value1)
            {
                Debug.Log("찾음");
                value = Value2;
                Debug.Log(value);
                inventory.MoveItem(index);
                return true;
            }
            index++;
        }

        return false;
    }
}
