using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// CSV 파일을 파싱하고 정의된 Row 클래스에 데이터를 채워넣는 클래스
/// </summary>
public class TableParser
{
    private const int HeaderRowIndex = 1;
    private const int DataStartRowIndex = 3;

    public static Table<TKey, TRow> Parse<TKey, TRow>
    (
        TextAsset csvFile,
        string idColumnName
    ) where TRow : TableBase, new()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일이 없습니다.");
            return null;
        }

        // 1. CSV 내용 읽기 및 줄/셀 분리
        List <string[]> rows;
        try
        {
            // 줄 분리, 분리된 줄 가져와서 ',' 단위 분리
            rows = csvFile.text
                    .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Split(',').Select(ceil => ceil.Trim()).ToArray())
                    .ToList();
        }
        catch (Exception e)
        {
            Debug.LogError($"CSV 파일 읽기 오류");
            return null;
        }

        // 2. 칼럼 헤더 추출
        if (rows.Count <= HeaderRowIndex)
        {
            Debug.LogError($"CSV에 컬럼 헤더 {HeaderRowIndex}가 없습니다!");
            return null;
        }

        // 헤더 행을 칼럼명 배열로 취득
        string[] columnNames = rows[HeaderRowIndex];

        // 사용자가 지정한 ID 칼럼 이름이 헤더에 있는지 확인
        int idIndex = Array.IndexOf(columnNames, idColumnName);
        if (idIndex < 0)
        {
            Debug.LogError($"CSV에 id 칼럼 {idColumnName}가 없습니다");
            return null;
        }

        // 3. TRow 클래스 속성 및 빌드 칼럼명 맵핑
        Type rowType = typeof( TRow );
        Dictionary<string, MemberInfo> memberMap = new Dictionary<string, MemberInfo>();
        foreach (var name in columnNames)
        {
            var prop = rowType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                memberMap.Add(name, prop);
                continue;
            }

            var field = rowType.GetField(name, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                memberMap.Add(name, field);
                continue;
            }
        }

        // 4. 데이터 파싱 및 TRow 인스턴스 생성

        // 최종적으로 결과가 저장될 장소
        Dictionary<TKey, TRow> allRowData = new Dictionary<TKey, TRow>();
        for (int i = DataStartRowIndex; i < rows.Count; i++)
        {
            string[] rowValues = rows[i];

            // 행의 셀 갯수가 헤더의 칼럼 갯수보다 작을 경우
            if (rowValues.Length < columnNames.Length)
            {
                if (rowValues.All(string.IsNullOrEmpty))
                    continue;
            }

            // ID 칼럼 문자열 값
            string idString = rowValues[idIndex];
            if (string.IsNullOrEmpty(idString))
                continue;

            TKey idValue = default;
            try
            {
                idValue = (TKey)Convert.ChangeType(idString, typeof(TKey));
            }
            catch (Exception e)
            {
                Debug.LogError($"id {idString} 변환 실패");
            }

            if (allRowData.ContainsKey(idValue))
            {
                Debug.LogError($"{idValue} 중복");
                continue;
            }
            // ID 처리 완료

            TRow newRow = new TRow();

            // 인스턴스에 값 채우기
            for (int j = 0; j < columnNames.Length; j++)
            {
                string columnName = columnNames[j];
                string stringValue = rowValues[j];

                // TRow에 멤버가 존재하는지 체크해서 진행
                if (memberMap.TryGetValue(columnName, out MemberInfo member))
                {
                    Type targetType = (member is PropertyInfo prop) ? prop.PropertyType : ((FieldInfo)member).FieldType;

                    try
                    {
                        // 현재는 문자열 값으로 되어있으니까 테이블에서 정의한 타입으로 변환해서 매핑시켜주기
                        object convertedValue = ConvertValue(stringValue, targetType);

                        if (member is PropertyInfo propertyInfo)
                        {
                            propertyInfo.SetValue(newRow, convertedValue);
                        }
                        else if (member is FieldInfo fieldInfo)
                        {
                            fieldInfo.SetValue(newRow, convertedValue);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"값 변환 오류: 칼럼 '{columnName}' 의 값 '{stringValue}' 을(를) {targetType} 타입으로 변환할 수 없습니다. 오류: {e.Message}");
                    }
                }
            }
            allRowData.Add(idValue, newRow);
        }

        // 정제된 데이터를 테이블로 변환
        string tableName = csvFile.name;
        return new Table<TKey, TRow>(tableName, idColumnName, allRowData);
    }

    private static object ConvertValue(string value, Type targetType)
    {
        value = value.Trim();

        // nullable이나 비어있는 값에 대한 처리
        if (string.IsNullOrEmpty(value))
        {
            if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
            {
                return Activator.CreateInstance(targetType);
            }

            return null;
        }

        // bool값에 대한 처리, 현재는 0, 1로 표시된 경우에만 처리 중.
        if (targetType == typeof(bool))
        {
            if (int.TryParse(value, out int intValue))
            {
                return intValue != 0;
            }
        }

        return Convert.ChangeType(value, targetType);
    }
}

