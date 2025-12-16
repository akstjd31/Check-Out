using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Spawn3DPrefabTile", menuName = "Tiles/Spawn 3D Prefab Tile")]
public class Spawn3DPrefabTile : TileBase
{
    [Header("타일 스프라이트")]
    public Sprite previewSprite;

    [Header("타일 종류")]
    public MapNodeType nodeType;

    /// <summary>
    /// 타일의 데이터를 얻어오기 위한 GetTileData 메서드입니다
    /// </summary>
    /// <param name="position">받아올 타일의 좌표</param>
    /// <param name="tilemap">타일맵 인터페이스를 통해 생성된 타일맵</param>
    /// <param name="tileData">받아올 타일의 데이터</param>
    public override void GetTileData(
        Vector3Int position,
        ITilemap tilemap,
        ref TileData tileData)
    {
        tileData.sprite = previewSprite;
        tileData.colliderType = Tile.ColliderType.None;
        tileData.transform = Matrix4x4.identity;
    }
}

//해당 타일이 가질 속성.
public enum MapNodeType
{
    Empty,
    Path,
    Room,
    Spawn,
    Exit
}
