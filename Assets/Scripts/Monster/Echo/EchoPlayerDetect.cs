using UnityEngine;

public class EchoPlayerDetect : MonoBehaviour
{
    private EchoSpawnSystem echoSpawnSystem;
    private EchoModel echo;

    private void Awake()
    {
        echoSpawnSystem = FindFirstObjectByType<EchoSpawnSystem>();
        echo = GetComponent<EchoModel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("에코가 플레이어와 충돌했습니다");
            if (echoSpawnSystem == null)
            {
                Debug.LogError("EchoController이 비어있습니다.");
                return;
            }
            echoSpawnSystem.DisableEcho();
        }
    }
}
