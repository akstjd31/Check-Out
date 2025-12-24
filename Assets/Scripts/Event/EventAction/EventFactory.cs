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
        }

        return null;
    }
}