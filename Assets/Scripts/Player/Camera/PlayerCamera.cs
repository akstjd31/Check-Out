using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;


public class PlayerCamera : MonoBehaviour
{

    private float shakeDuration = 0.2f; // 흔들림 지속 시간
    private float shakeAmount = 0.3f; // 흔들림 강도
    public CinemachineCamera deathCam;


    public void SwitchToDeathCam()
    {
        deathCam.Priority = deathCam.Priority = 999;
    }

    public void Hit()
    {
        StartCoroutine(ShakeRoutine());
    }

     IEnumerator ShakeRoutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * shakeAmount;
            transform.localPosition += offset;   // ⭐ 누적이 아니라 "순간 델타"

            timer += Time.deltaTime;
            yield return null;

        }
    }
}
