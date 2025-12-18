using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;     // 카메라의 부모 (눈, 머리 등)
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraDistance = 0.1f;
    private float maxPitch = 90f;

    private float pitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        this.transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
