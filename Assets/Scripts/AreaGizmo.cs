using UnityEngine;

public class AreaGizmo : MonoBehaviour
{
    private SphereCollider sphere;

    private void OnDrawGizmos()
    {
        if (sphere == null)
            sphere = this.GetComponent<SphereCollider>();

        if (sphere == null)
            return;

        if (this.CompareTag("SafeArea"))
            Gizmos.color = Color.blue;
        else if (this.CompareTag("Monster"))
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.yellow;

        Vector3 center = this.transform.TransformPoint(sphere.center);

        float radius = sphere.radius * Mathf.Max(
            transform.lossyScale.x,
            transform.lossyScale.y,
            transform.lossyScale.z
        );

        Gizmos.DrawWireSphere(center, radius);
    }
}
