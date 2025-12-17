using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // 현재는 아이템 전용이지만 나중에 확장성을 고려해서 바꿀 필요 있음.
    private string SavePath => Path.Combine(Application.persistentDataPath, "ItemSaveData.json");   

    // 데이터 저장 기능
    public void Save<T> (T data) where T : SaveBase
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    // 데이터 로드 기능
    public T Load<T> () where T : SaveBase, new()
    {
        if (!File.Exists(SavePath))
            return new T();
        
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<T>(json);
    }
}
