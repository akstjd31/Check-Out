using UnityEngine;

public class ElevatorCreation : MonoBehaviour
{
    [Header("소환할 엘레베이터 프리팹")]
    public GameObject elevatorPrefab;

    [Header("소환 실패 시 틀어막기 위한 프리팹")]
    public GameObject elevatorWall;

    [Header("랜덤 생성용 위치 여부")]
    public bool isRandom = true;

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
}
