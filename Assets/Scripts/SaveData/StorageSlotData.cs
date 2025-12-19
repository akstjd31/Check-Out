using System;
using System.Collections.Generic;

[Serializable]
public class StorageSaveData : SaveBase
{
    public List<StorageSlotData> slots = new();
}

[Serializable]
public class StorageSlotData
{
    public int index;
    public int itemId;
}
