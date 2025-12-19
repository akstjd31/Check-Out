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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monsterCount++;
            return;
        }

        if (other.CompareTag("LightArea"))
        {
            lightCount++;
            return;
        }
        
        if (other.CompareTag("SafeArea"))
        {
            safeCount++;
            return;
        }
    }

    // 태그를 통해서 범위를 벗어나면 발동이됨(빛, 안전지대, 몬스터)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monsterCount--;
        }

        if (other.CompareTag("LightArea"))
        {
            lightCount--;
        }

        if (other.CompareTag("SafeArea"))
        {
            safeCount--;
        }
    }
}
