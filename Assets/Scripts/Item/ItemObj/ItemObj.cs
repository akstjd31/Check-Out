using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public enum ObjState {On,Off}

public class ItemObj : MonoBehaviour
{
    [SerializeField] public int Duration {  get; private set; }

    public int Consumption { get; private set; }

    public ItemInstance ItemInstance { get; set; }

    public ObjState state { get; private set; } = ObjState.Off;

    public int ItemId;

    bool used = false;

    public event Action OnItem;
    public event Action OffItem;

    public void SetItemInfo(ItemInstance info)
    {
        ItemInstance = info;

        Duration = info.duration;
        Consumption = info.consumption;
    }

    public int Returnduration() => Duration;

    private void Update()
    {
        if (state == ObjState.On && used == false)
        {
            Debug.Log("코루틴 시작함");
            Debug.Log("남은 배터리 :" + Duration);
            StartCoroutine(StartConsumption());
        }
        else if (state == ObjState.Off && used == true)
        {
            Debug.Log("코루틴 끝남");
            StopCoroutine(StartConsumption());
            used = false;
        }
    }

    public IEnumerator StartConsumption()
    {
        used = true;
        while (Duration > 0)
        {
            Duration -= Consumption;
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("배터리 없음");
        ChangeState(ObjState.Off);
        yield break;
    }

    public void ChangeState(ObjState inputState)
    {
        switch (inputState)
        {
            case ObjState.On:
                state = ObjState.On;
                OnItem?.Invoke();
                break;
            case ObjState.Off:
                state = ObjState.Off;
                OffItem?.Invoke();
                break;
        }
    }
}
