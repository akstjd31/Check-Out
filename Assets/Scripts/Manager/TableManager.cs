using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
[SerializeField] private const string DefaultIdColumnName = "id";
    
    [Header("CSV 파일 경로(Resources 폴더 내의 경로)")]
    [SerializeField] private string csvPath = "Data";

    private static readonly Dictionary<Type, string> TableMap = new Dictionary<Type, string>()
    {
        
    };
    
    // 로드된 테이블 인스턴스를 저장해둘 컨테이너
    private static readonly Dictionary<Type, object> LoadedTables = new Dictionary<Type, object>();

    // CSV 파일명과 테이블 클래스 매칭을 위한 이름 저장 딕셔너리
    private static Dictionary<string, Type> TableNameToTypeMap;

    protected override void Awake()
    {
        base.Awake();

        // 런타임에 모든 TableBase 상속 클래스를 찾아 테이블명 -> 타입으로 맵을 구축
        BuildTableNameToTypeMap();

        // Resources 경로에서 CSV 파일을 읽어서 모든 테이블을 로드한다.
        LoadAllTables();
    }

    private static void BuildTableNameToTypeMap()
    {
        TableNameToTypeMap = new Dictionary<string, Type>();

        // 현재의 AppDomain에 로드된 모든 어셈블리 가져오기
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // TableBase 타입을 캐싱해두고 타입 비교에 사용하기 위함
        Type tableBaseType = typeof(TableBase);

        // 가져온 어셈블리마다 타입 검사
        foreach (var assembly in assemblies)
        {
            // TableBase를 상속받는 모든 non-abstract 클래스 타입을 가져오기
            var rowTypes = assembly.GetTypes()
                            .Where(t => t.IsClass && t.IsAbstract == false && t.IsSubclassOf(tableBaseType));

            foreach (var type in rowTypes)
            {
                string tableName = type.Name.Replace("Data", "");
                TableNameToTypeMap[tableName] = type;   
            }
        }

        Debug.Log($"{TableNameToTypeMap.Count}개의 TRow 매핑 완료");
    }

    private void LoadAllTables()
    {
        TextAsset[] csvFiles = Resources.LoadAll<TextAsset>($"{csvPath}");

        if (csvFiles.Length == 0)
        {
            Debug.LogError("지정된 경로에서 CSV 파일을 찾지 못함");
            return;
        }

        foreach (var csvFile in csvFiles)
        {
            string csvFileName = csvFile.name;

            if (TableNameToTypeMap.TryGetValue(csvFileName, out Type rowType) == false)
            {
                Debug.LogWarning($"CSV 파일 {csvFileName}에 대응하는 클래스가 존재하지 않습니다.");
                continue;
            }

            
            string idColumnName = DefaultIdColumnName;

            if (TableMap.TryGetValue(rowType, out string mappedIdName))
            {
                idColumnName = mappedIdName;
            }

            Type keyType = typeof(int);

            try
            {
                Type tableParserType = typeof(TableParser);

                MethodInfo parseMethod = tableParserType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);

                if (parseMethod == null)
                {
                    Debug.LogError("Parse 메서드를 찾을 수 없음");
                    continue;
                }

                MethodInfo genericParseMethod = parseMethod.MakeGenericMethod(keyType, rowType);

                object loadedTable = genericParseMethod.Invoke(null, new object[] { csvFile, idColumnName});

                if (loadedTable != null)
                {
                    LoadedTables[rowType] = loadedTable;

                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{rowType.Name} 로드 중 {e} 발생");
            }
        }
    }

    public Table<TKey, TRow> GetTable<TKey, TRow>() where TRow : TableBase
    {
        if (LoadedTables.TryGetValue(typeof(TRow), out object tableObject))
        {
            return tableObject as Table<TKey, TRow>;
        }

        return null;
    }

    public List<TKey> GetAllIds<TKey, TRow>(Table<TKey, TRow> table) where TRow : TableBase
    {
        if (table == null)
            return new List<TKey>();
        
        return table.Data.Keys.ToList();
    }
}
