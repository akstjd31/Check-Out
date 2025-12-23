using UnityEngine;
using System.Collections;


[RequireComponent(typeof(StatController))]
public class PlayerCameraController : MonoBehaviour
{
    private float shakeDuration = 0.1f;
    private float shakeAmount = 0.2f;

    public float fallAngle = -70f;
    public float fallDuration = 1.2f;

    public Rigidbody playerRb;
    public MonoBehaviour[] disableScripts;
    private StatController stat;
    private void Awake()
    {
        stat = this.GetComponent<StatController>();
    }

    private void OnEnable()
    {
        stat.OnDeath += SwitchToDeathCam;
    }

    private void OnDisable()
    {
        stat.OnDeath -= SwitchToDeathCam;
    }

    public void SwitchToDeathCam()
    {
        StartCoroutine(FallBack());
    }

    public void Hit()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float timer = 0f;
        Vector3 originalPos = transform.localPosition;

        while (timer < shakeDuration)
        {
            transform.localPosition = originalPos +
                Random.insideUnitSphere * shakeAmount;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    IEnumerator FallBack()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(fallAngle, 0, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fallDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        LockPlayer();
    }

    //쓰러짐이 끝난 뒤 플레이어 차단
    void LockPlayer()
    {
        // // 입력, 이동 스크립트 차단
        // foreach (var s in disableScripts)
        //     if (s != null) s.enabled = false;

        // Rigidbody가 있을 때만 봉인
        if (playerRb != null)
        {
            playerRb.detectCollisions = false;
            playerRb.useGravity = false;
            playerRb.isKinematic = true;
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }

        GameManager.Instance.isGameOver = true;
    }
}
