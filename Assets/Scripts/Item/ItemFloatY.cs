using UnityEngine;

public class ItemFloatY : MonoBehaviour
{
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatSpeed;

    private Vector3 startPos;

    private void Start()
    {
        startPos = this.transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        this.transform.position = startPos + Vector3.up * yOffset;
    }
}
