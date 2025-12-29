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
        lightComponent = player.transform.GetComponentInChildren<Light>();
        lightComponent.enabled = true;

        player.GetComponent<PlayerSoundController>().PlayFlashLightOnSound();
    }

    private void LightOff()
    {
        
        lightComponent.enabled = false;
    }
}
