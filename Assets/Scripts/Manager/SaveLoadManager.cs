using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    
    private string GetPath(string fileName) => Path.Combine(Application.persistentDataPath, fileName);
    
    // 데이터 저장 기능
    public void Save<T> (string fileName, T data) where T : SaveBase
    {
        string path = GetPath(fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    // 데이터 로드 기능
    public T Load<T> (string fileName) where T : SaveBase, new()
    {
        string path = GetPath(fileName);

        if (!File.Exists(path))
        {
            Debug.Log("해당 파일명이 존재하지 않습니다!");
            return null;
        }
        
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
