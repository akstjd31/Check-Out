using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBakeTool : EditorWindow
{
    [Header("생성할 프리팹")]
    [SerializeField] GameObject exitPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject normalWallPrefab;
    [SerializeField] GameObject roomWallPrefab;
    [SerializeField] GameObject doorPrefab;

    //베이크 진행할 타일맵
    private Tilemap tilemap;

    //베이크 결과물을 담을 부모 오브젝트
    private Transform mapRoot;

    //각 타일을 노드 관계로 연결하기 위한 Dictionary
    private Dictionary<Vector3Int, MapNode> nodes;

    //문 생성 시에 해당 부분에 지정할 탈출구 포인트.
    private List<ExitPoint> exitPoints;

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

        normalWallPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Normal Wall Prefab", normalWallPrefab, typeof(GameObject), false);

        roomWallPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Room Wall Prefab", roomWallPrefab, typeof(GameObject), false);

        doorPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Door Prefab", doorPrefab, typeof(GameObject), false);

        exitPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Exit Prefab", exitPrefab, typeof(GameObject), false);

        if (GUILayout.Button("SetUp"))
        {
            SetUp();
        }

        //GUI를 통해 Bake Map을 눌렀을 경우 Bake 메서드 실행.
        if (GUILayout.Button("Bake Map"))
        {
            Bake();
        }
    }

    void SetUp()
    {
        tilemap = FindAnyObjectByType<Tilemap>();
        mapRoot = FindAnyObjectByType<MapRoot>().transform;
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

        //배치된 타일에 따른 노드를 수집
        CollectNodes();
        
        //노드에 맞는 바닥 생성
        SpawnFloorNodes();

        //각 타일의 모서리에 오브젝트를 생성하기 위한 메서드
        ResolveEdges();

        MeshCombineSystem.Combine(mapRoot);
        //NavMash 공간까지 구현하기 위한 코드
        var surface = mapRoot.GetComponent<NavMeshSurface>();
        if (surface != null)
            surface.BuildNavMesh();

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

            //모든 방향을 체크
            foreach(var dir in dirs)
            {
                //특정 타일의 위치를 받고, 옆에 있는 타일의 위치를 받아온 타일 위치에 방향을 더한 값으로 저장.
                Vector3Int next = node.cell + dir;

                //바라보고 있는 방향에 이웃 노드가 존재하지 않을 경우 (비어있는 경우) 벽 생성
                if(!nodes.TryGetValue(next, out var neighbor))
                {
                    SpawnWallBetween(node, dir);
                    continue;
                }

                //바라보고 있는 방향에 이웃 노드가 있을 때, 현재 노드의 타입과 이웃 노드의 타입이 일치하면
                if (node.type == neighbor.type)
                {
                    //우선 현재 노드가 방인지 체크하고(참인 경우 이웃 노드도 타입이 방으로 같음.)
                    if(node.type == NodeType.Room)
                    {
                        //서로의 고유번호를 비교. 같다면 같은 타일을 사용하는 것이므로 다음 절차로.
                        if (node.index == neighbor.index)
                            continue;
                        else
                        {
                            //서로의 고유번호가 같지 않은 경우에, 각 Index가 4 차이나는 경우 혹은 이웃 index가 특수 노드인 경우 다음 절차로.
                            //이웃 노드가 5 이상이라는 것은 문 생성용 노드라는 것이므로 해당 노드가 문을 생성할 수 있도록 아무것도 생성하지 않음.
                            if (node.index == neighbor.index + 4 || node.index + 4 == neighbor.index || neighbor.index >= 5)
                                continue;
                            //서로의 고유번호가 다르면서 서로의 고유번호가 4 이하인 경우는 아예 다른 방인 것이므로 벽을 생성.
                            else if (node.index <= 4 && neighbor.index <= 4)
                            {
                                SpawnWallBetween(node, dir);
                                continue;
                            }
                            //서로의 고유번호가 다르면서 서로의 고유번호 중 하나라도 5를 넘어가는 경우 문을 생성하는 노드이므로 문 생성.
                            else if (node.index >= 5)
                            {
                                SpawnDoorBetween(node, dir);
                                continue;
                            }
                        }
                    }
                    //현재 노드가 방이 아니라면 길 혹은 탈출구인데, 이 경우 벽이나 문을 생성해줄 이유가 없으니 다음 절차로.
                    else
                        continue;
                }

                //길 노드와 방 노드가 서로 접촉하고 있는 경우
                else if (IsPathRoomPair(node, neighbor))
                {
                    //서로의 고유번호가 같다면
                    if (node.index == neighbor.index)
                    {
                        //문을 생성하는 것은 언제나 길 노드에서 이루어져야 함. 방에서 문을 밖으로 열 가능성 제거. (변경 가능)
                        if(neighbor.type == NodeType.Room)
                        SpawnDoorBetween(node, dir);
                        continue;
                    }
                    //고유번호가 다른 경우 벽을 생성.
                    else
                    {
                        SpawnWallBetween(node, dir);
                        continue;
                    }
                }
                //출구 노드의 경우 벽이나 문을 생성할 이유가 없다.
                else if (node.type == NodeType.Exit || neighbor.type == NodeType.Exit) 
                {
                    if (node.index != neighbor.index)
                        SpawnWallBetween(node, dir);
                    continue;
                }
                //여기까지 온 경우 (빈 타일이거나) Empty 종류의 타일이므로 벽을 생성.
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
        //해당 칸의 절반 크기만큼의 길이를 구하고 약간만 숫자를 줄여 다른 타일에서 생성한 오브젝트의 영향을 줄인다.
        Vector3 cellHalf = tilemap.transform.TransformVector(tilemap.cellSize * 0.45f);
        //XYZ축 기준의 방향을 XZY축 기준으로 변경하여
        Vector3 offset = new Vector3(dir.x * cellHalf.x, 0f, dir.y * cellHalf.y);
        //노드의 좌표에서 특정 방향으로 칸의 절반만큼 이동한 상태에서 벽을 생성한다.
        Vector3 pos = node.world + offset;

        //현재 노드가 벽이며, 그 고유번호가 4 이하인 경우에는 방 내부이므로 방 전용 벽을 생성하고,
        //그 이외의 경우에는 일반 벽을 생성한다.
        GameObject wall =
            node.type == NodeType.Room?
            (GameObject)PrefabUtility.InstantiatePrefab(roomWallPrefab):
            (GameObject)PrefabUtility.InstantiatePrefab(normalWallPrefab);

        CreateColliderOfWall(wall);

        //벽의 위치, 회전, 크기와 부모를 정해준다.
        wall.transform.position = pos;
        wall.transform.rotation = RotationFromDir(dir);
        wall.transform.localScale = tilemap.cellSize * tilemap.transform.parent.transform.localScale.x;
        wall.transform.SetParent(mapRoot, true);
    }
    /// <summary>
    /// 문을 형성하기 위한 메서드입니다. SpawnWallBetween 메서드와 기능이 비슷하나 이동을 위한 추가 코드가 존재합니다.
    /// </summary>
    /// <param name="node">문 형성의 기준점이 될 노드</param>
    /// <param name="dir">방향</param>
    void SpawnDoorBetween(MapNode node, Vector3Int dir)
    {
        Vector3 cellHalf = tilemap.transform.TransformVector(tilemap.cellSize * 0.5f);
        Vector3 offset = new Vector3(dir.x * cellHalf.x, 0f, dir.y * cellHalf.y);
        Vector3 pos = node.world + offset;

        GameObject door =
            (GameObject)PrefabUtility.InstantiatePrefab(doorPrefab);


        door.transform.position = pos;
        door.transform.rotation = RotationFromDir(dir);
        door.transform.localScale = tilemap.cellSize * tilemap.transform.parent.transform.localScale.x;
        door.transform.SetParent(mapRoot, true);

        //추가된 코드. 방 타입에 따라서 NavMeshLink를 생성합니다.
        if (node.type == NodeType.Path || node.type == NodeType.Room)
        {
            //방에 출구라는 표시를 남기기 위한 컴포넌트 추가
            // GameObject doorChild =  door.transform.GetChild(0).gameObject;
            door.AddComponent<ExitPoint>();
            ExitPoint exitPoint = door.GetComponent<ExitPoint>();

            //NavMesh 생성용 게임 오브젝트 생성.
            GameObject movePath = new GameObject();
            //구분하기 쉽도록 이름 변경
            movePath.name = "NavMeshLink";
            movePath.transform.parent = mapRoot;
            movePath.transform.position = door.transform.position;
            movePath.transform.rotation = door.transform.rotation;

            //생성된 오브젝트에 NavMeshLink 추가.
            NavMeshLink navLink = movePath.AddComponent<NavMeshLink>();
            exitPoint.SetNavMeshLink(navLink);

            //시작지점은 방, 도착지점은 길. 생성되는 방향 따라 지정.
            navLink.startPoint = Vector3.back * 1.5f;
            navLink.endPoint = Vector3.forward * 1.5f;
            //일방통행 여부는 길 타입일 경우는 일방통행, 방 타입일 경우 쌍방향 통행. => 몬스터가 방에 들어올 수 있는 걸 생각해서 쌍방향
            navLink.bidirectional = true;
            //지역에 맞게 지역 종류 변경
            navLink.area = node.type == NodeType.Path? NavMesh.GetAreaFromName("Walkable") : NavMesh.GetAreaFromName("Room");
        }
    }

    /// <summary>
    /// 벽 생성 시에 BoxCollider을 추가해주는 메서드입니다.
    /// </summary>
    /// <param name="wall">벽 프리팹</param>
    void CreateColliderOfWall(GameObject wall)
    {
        //벽 오브젝트로부터 MeshRenderer을 받아옵니다.
        //위치를 직접 지정하지 않으려면 빈 오브젝트에 넣어 위치를 바꿔주어야 하므로 자식으로부터 찾아옵니다.
        var renderer = wall.GetComponentInChildren<MeshRenderer>();

        //발견되지 않으면 반환합니다.
        if (renderer == null)
            return;

        //박스의 경계를 지정해 줍니다.
        Bounds bound = renderer.bounds;

        //해당 벽이 Collider을 가지고 있는 상태인지 확인하고, 가지고 있다면 받아옵니다.
        BoxCollider collider = wall.GetComponent<BoxCollider>();
        //존재하지 않아 받아오지 못했다면, BoxCollider을 새로 형성해 줍니다.
        if(collider == null)
        collider = wall.AddComponent<BoxCollider>();

        //해당 Collider의 중심 지역은 벽 프리팹의 중앙이 되어야 합니다.
        collider.center = wall.transform.InverseTransformPoint(bound.center);
        //해당 Collider의 크기를 벽의 크기와 동일하게 변경합니다.
        collider.size = bound.size;
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
                    SpawnExit(node);
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
    /// 출구(엘레베이터) 생성을 제어하기 위한 코드로, 생성 시 방향을 제어합니다.
    /// </summary>
    /// <param name="node"></param>
    void SpawnExit(MapNode node)
    {
        //기본 회전은 identity(회전 적용 X)
        Quaternion rotation = Quaternion.identity;

        //모든 방향을 기준으로
        foreach (var dir in dirs)
        {
            //확인할 좌표는 현재 좌표에 방향값을 더한 곳.
            Vector3Int next = node.cell + dir;

            //해당 좌표에 이웃이 없었을 경우, 즉 유효한 타일이 아니었을 경우 넘김
            if (!nodes.TryGetValue(next, out var neighbor))
                continue;

            // 같은 고유번호인지 확인. 탈출구 한정이므로 -1만 체크하는 길 타일의 예외 처리를 두지 않아도 된다.
            if (node.index != neighbor.index && node.index != neighbor.index + 4)
                continue;

            //같은 고유번호를 가진 타일이 확인된 경우 해당 타일을 바라보는 방향으로 변경.
            rotation = RotationFromDir(dir);
        }

        GameObject go = Instantiate(exitPrefab, node.world, rotation);
        go.transform.SetParent(mapRoot, true);

        //크기 조절
        go.transform.localScale = tilemap.cellSize * (tilemap.transform.parent.transform.localScale.x / 10);
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
        go.transform.localScale = tilemap.cellSize * (tilemap.transform.parent.transform.localScale.x / 10);
    }

    /// <summary>
    /// 타일맵이 XZY축을 사용함에 따라, XYZ 기준의 방향을 XZY 기준으로 강제 변경합니다.
    /// </summary>
    /// <param name="dir">XYZ 축 사용 기준 방향</param>
    /// <returns></returns>
    Vector3 DirToWorld(Vector3Int dir)
    {
        //타일맵의 X,Y축을 월드 좌표의 X,Z축으로 변경
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