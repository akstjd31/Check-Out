using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapBakeTool : EditorWindow
{
    [Header("생성할 프리팹")]
    [SerializeField] GameObject exitPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject doorPrefab;

    //베이크 진행할 타일맵
    private Tilemap tilemap;

    //베이크 결과물을 담을 부모 오브젝트
    private Transform mapRoot;

    //각 타일을 노드 관계로 연결하기 위한 Dictionary
    private Dictionary<Vector3Int, MapNode> nodes;

    //"Vector3Int형 방향"의 배열.
    static readonly Vector3Int[] dirs =
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };

    /// <summary>
    /// 유니티 에디터 상에서 실행하기 위한 칸을 추가합니다.
    /// </summary>
    [MenuItem("Tools/Map/Bake Map")]
    static void Open()
    {
        GetWindow<MapBakeTool>("Map Bake");
    }

    void OnGUI()
    {
        //Bake를 진행할 Tilemap
        tilemap = (Tilemap)EditorGUILayout.ObjectField(
            "Source Tilemap", tilemap, typeof(Tilemap), true);

        //Bake로 생성된 GameObject들을 담을 루트
        mapRoot = (Transform)EditorGUILayout.ObjectField(
            "Map Root", mapRoot, typeof(Transform), true); // "라벨명", 소환할 오브젝트, 허용할 타입, 씬 내 오브젝트 허용 여부

        EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);

        pathPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Path Prefab", pathPrefab, typeof(GameObject), false);

        roomPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Room Floor Prefab", roomPrefab, typeof(GameObject), false);

        wallPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Wall Prefab", wallPrefab, typeof(GameObject), false);

        doorPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Door Prefab", doorPrefab, typeof(GameObject), false);

        exitPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Exit Prefab", exitPrefab, typeof(GameObject), false);


        //GUI를 통해 Bake Map을 눌렀을 경우 Bake 메서드 실행.
        if (GUILayout.Button("Bake Map"))
        {
            Bake();
        }
    }

    /// <summary>
    /// 단 하나의 메쉬를 가지도록 통합하는 메서드입니다.
    /// </summary>
    void Bake()
    {
        //배치된 게 없으면 실행 불가
        if (tilemap == null || mapRoot == null) return;

        //부모 오브젝트의 자식 수가 0보다 큰 경우, 즉시 모든 자식 오브젝트를 파괴
        while (mapRoot.childCount > 0)
            DestroyImmediate(mapRoot.GetChild(0).gameObject);

        // 타일맵 기반 데이터 해석 및 생성
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            //타일맵에서 해당 위치에 타일이 없으면 과정 넘김
            if (!tilemap.HasTile(pos))
                continue;

            //해당 위치에 있는 커스텀 타일의 정보를 받아옴
            var tile = tilemap.GetTile<Spawn3DPrefabTile>(pos);

            //정보가 없다면 넘김
            if (tile == null)
                continue;

            //월드맵 기반으로 해당 타일의 위치 생성
            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);

        }

        CollectNodes();
        SpawnFloorNodes();
        ResolveEdges();
        MeshCombineSystem.Combine(mapRoot);

        Debug.Log("맵 Bake 성공!");
    }

    /// <summary>
    /// 방향을 입력받아 회전시켜 주는 메서드입니다.
    /// </summary>
    /// <param name="dir">Quaternion 값으로 반환받을 방향</param>
    /// <returns></returns>
    Quaternion RotationFromDir(Vector3Int dir)
    {
        if (dir == Vector3Int.up)
            return Quaternion.Euler(0, 180, 0);
        else if (dir == Vector3Int.right)
            return Quaternion.Euler(0, -90, 0);
        else if (dir == Vector3Int.left)
            return Quaternion.Euler(0, 90, 0);
        else
            return Quaternion.identity;
    }

    /// <summary>
    /// 타일맵으로부터 모든 타일을 불러와 노드를 생성하는 메서드입니다.
    /// </summary>
    void CollectNodes()
    {
        //Dictionary에 new 할당
        nodes = new Dictionary<Vector3Int, MapNode>();

        //타일맵에서 타일이 있는 가장 작은 좌표부터 가장 큰 좌표까지 영역을 받아와서
        //모든 좌표를 하나씩 꺼내 그 좌표마다 각각 코드를 적용한다.
        foreach(var pos in tilemap.cellBounds.allPositionsWithin)
        {
            //타일맵에서 좌표상에 타일이 존재하지 않는다면 즉시 다음 과정으로.
            if (!tilemap.HasTile(pos))
                continue;

            //타일이 존재하면, 타일을 얻어온다.
            var tile = tilemap.GetTile<Spawn3DPrefabTile>(pos);
            //이때 타일을 불러오는 데 실패했다면 즉시 다음 과정으로.
            if (tile == null)
                continue;

            //노드를 새롭게 생성한다.
            nodes[pos] = new MapNode
            {
                cell = pos,
                world = tilemap.GetCellCenterWorld(pos),
                type = Convert(tile.nodeType),
                index = tile.index
            };
            Debug.Log($"Collected {nodes.Count} nodes");
        }
    }

    /// <summary>
    /// 이웃 노드에 따라 벽 또는 문을 생성하는 메서드입니다.
    /// </summary>
    void ResolveEdges()
    {
        //Dictionary의 <TKey, TValue> 중 TValue를 받아와야 존재하는 타일의 노드만 받아올 것.
        foreach(var node in nodes.Values)
        {
            //빈 타일인 경우 다음 단계로
            if (node.type == NodeType.Empty)
                continue;

            foreach(var dir in dirs)
            {
                Vector3Int next = node.cell + dir;

                //바라보고 있는 방향에 이웃 노드가 존재하지 않을 경우 (비어있는 경우) 벽 생성
                if(!nodes.TryGetValue(next, out var neighbor))
                {
                    SpawnWallBetween(node, dir);
                    continue;
                }

                //바라보고 있는 방향에 이웃 노드가 있다면 방일 경우 문을, 아닐 경우 벽을 생성
                if (node.type == neighbor.type)
                {
                    if(node.type == NodeType.Room)
                    {
                        if (node.index == neighbor.index)
                            continue;
                        else
                        {
                            if (node.index == neighbor.index + 4 || node.index + 4 == neighbor.index || neighbor.index >= 5)
                                continue;
                            else if(node.index <= 4 && neighbor.index <= 4)
                            {
                                SpawnWallBetween(node, dir);
                                continue;
                            }
                            else if (node.index >= 5)
                            {
                                SpawnDoorBetween(node, dir);
                                continue;
                            }
                        }
                    }
                    else
                        continue;
                }
                else if (IsPathRoomPair(node, neighbor))
                {
                    if (node.index == neighbor.index)
                    {
                        SpawnDoorBetween(node, dir);
                        continue;
                    }
                    else
                    {
                        SpawnWallBetween(node, dir);
                        continue;
                    }
                }
                else if (node.type == NodeType.Exit || neighbor.type == NodeType.Exit) 
                    continue;
                else
                {
                    SpawnWallBetween(node, dir);
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 벽을 형성하기 위한 메서드입니다. SpawnDoorBetween 메서드 또한 문을 형성하기 위한 메서드로 기능이 거의 동일합니다.
    /// </summary>
    /// <param name="node">벽 형성의 기준점이 될 노드</param>
    /// <param name="dir">방향</param>
    void SpawnWallBetween(MapNode node, Vector3Int dir)
    {
        //해당 칸의 절반 크기만큼의 길이를 구하고
        Vector3 cellHalf = tilemap.transform.TransformVector(tilemap.cellSize * 0.5f);
        //XYZ축 기준의 방향을 XZY축 기준으로 변경하여
        Vector3 offset = new Vector3(dir.x * cellHalf.x, 0f, dir.y * cellHalf.y);
        //노드의 좌표에서 특정 방향으로 칸의 절반만큼 이동한 상태에서 벽을 생성한다.
        Vector3 pos = node.world + offset;

        GameObject wall =
            (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);

        wall.transform.position = pos;
        wall.transform.rotation = RotationFromDir(dir);
        wall.transform.SetParent(mapRoot, true);
    }
    void SpawnDoorBetween(MapNode node, Vector3Int dir)
    {
        Vector3 cellHalf = tilemap.transform.TransformVector(tilemap.cellSize * 0.5f);
        Vector3 offset = new Vector3(dir.x * cellHalf.x, 0f, dir.y * cellHalf.y);
        Vector3 pos = node.world + offset;

        GameObject wall =
            (GameObject)PrefabUtility.InstantiatePrefab(doorPrefab);

        wall.transform.position = pos;
        wall.transform.rotation = RotationFromDir(dir);
        wall.transform.SetParent(mapRoot, true);
    }

    /// <summary>
    /// 바닥의 노드에 따라 프리팹을 형성합니다.
    /// </summary>
    void SpawnFloorNodes()
    {
        foreach(var node in nodes.Values)
        {
            switch (node.type)
            {
                case NodeType.Path:
                    Spawn(pathPrefab, node.world);
                    break;
                case NodeType.Room:
                    Spawn(roomPrefab, node.world);
                    break;
                case NodeType.Exit:
                    Spawn(pathPrefab, node.world);
                    Spawn(exitPrefab, node.world);
                    break;
                case NodeType.Empty:
                    break;
                default:
                    Debug.LogWarning($"할당되지 않은 노드 타입: {node.type}");
                    break;
            }
        }
    }

    /// <summary>
    /// 지정된 위치에 프리팹을 생성합니다.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    void Spawn(GameObject prefab, Vector3 pos)
    {
        //유효하지 않은 프리팹인 경우 그대로 반환
        if (!prefab) return;

        //에디터 상에서 프리팹을 생성하는 것이므로 PrefabUtility 사용
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.SetParent(mapRoot, true);
        go.transform.position = pos;
    }

    /// <summary>
    /// 타일맵이 XZY축을 사용함에 따라, XYZ 기준의 방향을 XZY 기준으로 강제 변경합니다.
    /// </summary>
    /// <param name="dir">XYZ 축 사용 기준 방향</param>
    /// <returns></returns>
    Vector3 DirToWorld(Vector3Int dir)
    {
        // Tilemap (X,Y) → World (X,Z)
        return new Vector3(dir.x, 0f, dir.y);
    }

    /// <summary>
    /// 커스텀 타일맵에 사용한 타일 종류를 노드에 사용하는 타일 종류로 변환하는 메서드입니다.
    /// </summary>
    /// <param name="tileType">커스텀 타일맵에서의 타일 종류</param>
    /// <returns></returns>
    NodeType Convert(MapNodeType tileType)
    {
        switch(tileType)
        {
            case MapNodeType.Path:
                return NodeType.Path;
            case MapNodeType.Room:
                return NodeType.Room;
            case MapNodeType.Exit:
                return NodeType.Exit;
            case MapNodeType.Spawn:
                return NodeType.Spawn;
            default:
                return NodeType.Empty;
        }
    }

    /// <summary>
    /// 벽 노드와 길 노드가 이웃인지 확인하기 위한 메서드입니다.
    /// </summary>
    /// <param name="a">기준이 되는 노드</param>
    /// <param name="b">이웃 노드</param>
    /// <returns></returns>
    bool IsPathRoomPair(MapNode a, MapNode b)
    {
        return (a.type == NodeType.Path && b.type == NodeType.Room) ||
              (a.type == NodeType.Room && b.type == NodeType.Path);
    }
}