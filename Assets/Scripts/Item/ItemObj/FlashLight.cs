using UnityEngine;

public class FlashLight : ItemObj
{
    private Light lightComponent;
    private GameObject lightArea;


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
        lightArea = player.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
        lightComponent.enabled = true;

        player.GetComponent<PlayerSoundController>().PlayFlashLightOnSound();
        lightArea.SetActive(true);
    }

    private void LightOff()
    {
        lightArea.SetActive(false);
        lightComponent.enabled = false;
    }
}
