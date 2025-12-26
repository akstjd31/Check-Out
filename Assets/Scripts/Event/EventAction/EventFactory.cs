using UnityEngine;
public static class EventFactory
{
    public static IEventAction Create(string eventType)
    {
        switch (eventType)
        {
            case "setSwitchOn":
                return new SetSwitchOnAction();
            case "soundOnce":
                Debug.Log("소리 재생 이벤트 발동");
                return new SoundOnceAction();
            case "showText":
                return new ShowTextAction();
        }

        return null;
    }
}