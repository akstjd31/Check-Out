using System;
using System.Collections.Generic;

[Serializable]
public class SlotSaveData : SaveBase
{
    public List<SlotData> slots = new();
}

[Serializable]
public class SlotData
{
    public int index;
    public int itemId;
    public int duration;
}
