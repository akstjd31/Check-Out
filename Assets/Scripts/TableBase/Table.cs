using System.Collections.Generic;
using UnityEngine;
public class Table<TKey, TRow> where TRow : TableBase
{
    private readonly Dictionary<TKey, TRow> data;
    private readonly string tableName;
    private readonly string idColumnName;

    public IReadOnlyDictionary<TKey, TRow> Data => data;
    public Table(string tableName, string idColumnName, Dictionary<TKey, TRow> data)
    {
        this.tableName = tableName;
        this.idColumnName = idColumnName;
        this.data = data;
    }

    public TRow this[TKey id]
    {
        get
        {
            if (data.TryGetValue(id, out TRow row))
                return row;
            
            Debug.LogError($"존재하지 않는 ID : {id}");
            return null;
        }
    }
}
