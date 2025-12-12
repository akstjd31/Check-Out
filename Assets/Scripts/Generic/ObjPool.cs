using UnityEngine;
using System.Collections.Generic;

public class ObjPool<T> where T : MonoBehaviour
{
    private Dictionary<T, Queue<T>> pool = new();
    private Transform parent;

    // 각 오브젝트 풀에는 부모가 존재.
    public void CreatePool(T prefab, int poolSize, Transform parent)
    {
        this.parent = parent;

        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<T>();

        for (int i = 0; i < poolSize; i++)
        {
            T instance = Object.Instantiate(prefab, parent);
            instance.gameObject.SetActive(false);
            pool[prefab].Enqueue(instance);
        }
    }

    // 풀에서 꺼내기
    public T GetObject(T prefab)
    {
        if (!pool.TryGetValue(prefab, out var q))
        {
            q = new Queue<T>();
            pool[prefab] = q;
        }

        if (q.Count > 0)
        {
            T obj = q.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        // 부족할 경우 생성
        T newObj = Object.Instantiate(prefab, parent);
        return newObj;
    }

    // 회수
    public void ReturnObject(T prefab, T obj)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<T>();

        obj.gameObject.SetActive(false);
        pool[prefab].Enqueue(obj);
    }
}
