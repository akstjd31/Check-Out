using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public enum ObjState {On,Off}

public class ItemObj : MonoBehaviour
{
    public int Consumption { get; private set; }

    public ItemInstance ItemInstance { get; set; }

    public ObjState state { get; private set; } = ObjState.Off;

    public int ItemId;

    private Coroutine coroutine;

    public event Action OnItem;
    public event Action OffItem;

    public void SetItemInfo(ItemInstance info)
    {
        ItemInstance = info;

        Consumption = info.consumption;
    }

    public void StopConsumption()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void StartConsumption()
    {
        StopConsumption();

        coroutine = StartCoroutine(RunConsumption());
    }

    public IEnumerator RunConsumption()
    {
        
        while (ItemInstance.duration > 0 && state == ObjState.On)
        {
            Debug.Log("남은 배터리 :" + ItemInstance.duration);
            ItemInstance.duration -= Consumption;
            if (ItemInstance.duration < 0)
                ItemInstance.duration = 0;
            yield return new WaitForSeconds(1f);
        }

        if (state == ObjState.On)
            ChangeState(ObjState.Off);

        coroutine = null;
    }

    public void ChangeState(ObjState inputState)
    {
        if (state == inputState) return;

        state = inputState;

        if (state == ObjState.On)
        {
            OnItem?.Invoke();
            StartConsumption();
        }
        else
        {
            OffItem?.Invoke();
            StopConsumption();
        }
    }
}
