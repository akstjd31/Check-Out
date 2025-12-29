using UnityEngine;

public class EchoPlayerDetect : MonoBehaviour
{
    private EchoController echoController;

    private void Awake()
    {
        echoController = FindFirstObjectByType<EchoController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("에코가 플레이어와 충돌했습니다");
            if (echoController == null)
            {
                Debug.LogError("EchoController이 비어있습니다.");
                return;
            }
            echoController.DisableEcho();
        }
    }
}
