using UnityEngine;


public enum NodeType
{
    Empty, Spawn, Path, Room, Exit
}
public class MapNode
{
    [Header("타일맵에서의 좌표")]
    public Vector3Int cell;

    [Header("월드 좌표")]
    public Vector3 world;

    [Header("생성할 오브젝트 종류")]
    public NodeType type;

    [Header("구분 전용 - 고유번호")]
    public int index;
}
