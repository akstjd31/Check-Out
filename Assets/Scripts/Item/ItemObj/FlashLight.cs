using UnityEngine;

public class FlashLight : ItemObj
{
    private Light lightComponent;
    private GameObject lightArea;
    private PlayerAreaDetector areaDetector;


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

    private void Update()
    {
        if (state.Equals(ObjState.On))
        {
            areaDetector.SetLightCount(1);
        }
    }

    private void LightOn()
    {
        if (player != null)
        {
            lightComponent = player.transform.GetComponentInChildren<Light>();
            areaDetector = player.transform.GetComponent<PlayerAreaDetector>();
            player.GetComponent<PlayerSoundController>().PlayFlashLightOnSound();
        }

        lightComponent.enabled = true;
    }

    private void LightOff()
    {
        lightComponent.enabled = false;

        if (areaDetector != null)
            areaDetector.SetLightCount(0);
    }
}
