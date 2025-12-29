using UnityEngine;

public class TrackingEye : MonoBehaviour
{
    [SerializeField] private GameObject eye;

    private void Update()
    {
        transform.rotation = eye.transform.rotation;
    }
}
