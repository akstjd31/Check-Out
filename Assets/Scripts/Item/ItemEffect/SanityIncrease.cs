using UnityEngine;

public class SanityIncrease : ItemEffect
{
    public SanityIncrease(string name, int value1, int value2, string controlKey)
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
        playerStat.ChangeSanity(false,Value1);

        //InventoryManager.Instance.RemoveInventoryItem();
       
        return true;
    }
}
