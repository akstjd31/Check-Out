using UnityEngine;

public class StaminaIncrease : ItemEffect
{
    public StaminaIncrease(string name,int value1, int value2, string controlKey)
    {
        EffectName = name;
        Value1 = value1;
        Value2 = value2;
        ControlKey = controlKey;

    }

    public override bool Use()
    {
        GameObject player = GameManager.Instance.Player;
        StatController playerStat = player.transform.GetComponent<StatController>();
        PlayerSoundController soundController = player.transform.GetComponent<PlayerSoundController>();

        soundController.PlayEatingSound();
        playerStat.AddStamina(Value1);

        //InventoryManager.Instance.RemoveInventoryItem();

        return true;
    }
}
