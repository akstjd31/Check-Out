using UnityEngine;

public class ItemFloatY : MonoBehaviour
{
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatSpeed;

    private Vector3 startPos;

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        this.transform.position = this.transform.parent.position + Vector3.up * yOffset;
    }
}
