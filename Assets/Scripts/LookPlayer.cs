using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // 카메라 방향 그대로 바라보게
        transform.rotation = Quaternion.LookRotation(
            transform.position - cam.position
        );
    }
}
