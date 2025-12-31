using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    private PlayerCameraController playerCamController;
    private Rigidbody rigid;
    [SerializeField] private Transform cameraPivot;     // 카메라의 부모 (눈, 머리 등)
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraDistance = 0.1f;
    private float maxPitch = 90f;
    private float pitch;
    private bool canCameraRotate;

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerCamController = this.GetComponent<PlayerCameraController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        FadeManager.Instance.OnFadeStarted += HandleFadeStarted;
        FadeManager.Instance.OnFadeEnded += HandleFadeEnded;
    }

    private void OnDisable()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.OnFadeStarted -= HandleFadeStarted;
            FadeManager.Instance.OnFadeEnded -= HandleFadeEnded;
        }
    }

    private void HandleFadeStarted()
    {
        canCameraRotate = false;
    }

    private void HandleFadeEnded()
    {
        canCameraRotate = true;
    }


    private void LateUpdate()
    {
        if (canCameraRotate && !playerCamController.IsPlayDeathCam && !GameManager.Instance.IsOpenedUI())
            UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rigid.angularVelocity = Vector3.up * mouseX;
        this.transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
