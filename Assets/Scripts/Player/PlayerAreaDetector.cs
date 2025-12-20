using UnityEngine;

// 영역별 정신력 감소량 계산 관련 클래스
public class PlayerAreaDetector : MonoBehaviour
{
    [SerializeField] private int lightCount = 0;
    [SerializeField] private int safeCount = 0;
    [SerializeField] private int monsterCount = 0;
    public bool IsLight => lightCount > 0;
    public bool IsSafe => safeCount > 0;
    public bool IsMonster => monsterCount > 0;


    // 태그를 통해서 범위 내 있으면 발동이됨(빛, 안전지대, 몬스터)
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MonsterArea"))
        {
            Debug.Log(other.transform.parent.name);
            var walkerModel = other.transform.parent.GetComponent<WalkerModel>();

            if (walkerModel != null && walkerModel.walkerState.Equals(Monster.MonsterState.Chase))
            {
                monsterCount = 1;
            }
            
            return;
        }

        if (other.CompareTag("LightArea"))
        {
            lightCount = 1;
            return;
        }
        
        if (other.CompareTag("SafeArea"))
        {
            safeCount = 1;
            return;
        }
    }

    // 태그를 통해서 범위를 벗어나면 발동이됨(빛, 안전지대, 몬스터)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MonsterArea"))
        {
            monsterCount = -1;
        }

        if (other.CompareTag("LightArea"))
        {
            lightCount = -1;
        }

        if (other.CompareTag("SafeArea"))
        {
            safeCount = -1;
        }
    }
}
