using UnityEngine;

public class MonsterSoundDetect : MonoBehaviour, ISound
{
    // 플레이어가 소리 감지 영역 안에 있는 지 판단하는 변수
    private bool isPlayerAround = false;
    public void DetectSound(Transform inputTransform)
    {
        // isPlayerAround가 참인 경우에만 아래 코드 수행
        if (isPlayerAround)
        {
            Debug.Log($"{name}사운드 발생");
        }
    }
    // 소리 감지 영역 내에 플레이어가 진입하면 isPlayerAround를 참으로 바꿈
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 주변에 있음");
            isPlayerAround = true;
        }
    }
    // 소리 감지 영역 내에 플레이어가 사라지면 isPlayerAround를 거짓으로 바꿈
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 주변에서 사라짐");
            isPlayerAround = false;
        }
    }
}
