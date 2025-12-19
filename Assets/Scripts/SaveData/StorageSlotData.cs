using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

[Serializable]
public class StorageSaveData
{
    public List<StorageSlotData> slots = new();
}

[Serializable]
public class StorageSlotData
{
    public int index;
    public int itemId;
}
