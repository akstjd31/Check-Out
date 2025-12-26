using UnityEngine;

public class FlashLight : ItemObj
{
    [SerializeField ]Light lightComponent;

    private void OnEnable()
    {
        OnItem += LightOn;
        OffItem += LightOff;
    }

    private void OnDisable()
    {
        ChangeState(ObjState.Off);
        OnItem -= LightOn;
        OffItem -= LightOff;
    }

    private void LightOn()
    {
        lightComponent.enabled = true;
    }

    private void LightOff()
    {
        
        lightComponent.enabled = false;
    }
}
