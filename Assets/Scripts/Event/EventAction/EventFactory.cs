public static class EventFactory
{
    public static IEventAction Create(string eventType)
    {
        switch (eventType)
        {
            case "setSwitchOn":
                return new SetSwitchOnAction();
            case "soundOnce":
                return new SoundOnceAction();
            case "showText":
                return new ShowTextAction();
            case "soundLoop":
                return new SoundLoopAction();
            case "delay":
                return new DelayAction();
            case "cooldown":
                return new CoolDownAction();
            case "soundStop":
                return new SoundStopAction();
            case "playAnim":
                return new PlayAnimAction();
            case "setActiveObject":
                return new SetActiveObjectAction();
        }

        return null;
    }
}