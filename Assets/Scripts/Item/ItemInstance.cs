
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemState { On, Off }

[Serializable]
public class ItemInstance
{
    public ItemTableData itemdata { get; private set; }
    public List<ItemEffect> effects { get; private set; }
    public int duration {  get; set; }
    public int consumption { get; set; }
    public ItemState state { get; private set; } = ItemState.Off;

    public event Action OnItem;
    public event Action OffItem;

    public ItemInstance(ItemTableData itemdata, List<ItemEffect> effects)
    {
        this.itemdata = itemdata;
        this.effects = effects;

        foreach (var effect in effects)
        {
            if (effect is GadgetDuration)
            {
                duration = effect.Value1;
                consumption = effect.Value2;
            }

            else if (effect is ConsumableDuration)
            {
                duration = effect.Value1;
                consumption = effect.Value2;
            }
        }
    }

    public bool Use(string key)
    {
        bool sucess = false;
        foreach (var effect in effects)
        {
            if (key == effect.ControlKey)
            {
                switch (effect.EffectName)
                {
                    case "ConsumableDuration":
                        sucess = ChangeState(ItemState.On);
                        break;
                    case "GadgetReload":
                        sucess = effect.Use(out int value);
                        if (sucess)
                            duration += value;
                        break;
                    case "Light":
                        sucess = state == ItemState.On ? ChangeState(ItemState.Off) : effect.Use();
                        break;
                    default:
                        sucess = effect.Use();
                        break;
                }
            }
        }

        return sucess;
    }


    public IEnumerator Consumption()
    {
        while (duration <= 0)
        {
            duration -= consumption;
            yield return new WaitForSeconds(1f);
        }
        ChangeState(ItemState.Off);
        yield break;
    }

    // 아이템의 온 오프
    public bool ChangeState(ItemState inputState)
    {
        switch (inputState)
        {
            case ItemState.On:
                state = ItemState.On;
                OnItem?.Invoke();
                return true;
            case ItemState.Off:
                state = ItemState.Off;
                OffItem?.Invoke();
                return true;
        }
        return false;
    }
}


