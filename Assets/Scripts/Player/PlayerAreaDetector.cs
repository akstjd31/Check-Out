using UnityEngine;

public class PlayerAreaDetector : MonoBehaviour
{
    private int lightCount = 0;
    private int safeCount = 0;
    private int mosterCount = 0;
    public bool isLight => lightCount > 0;

    public bool isSafe => safeCount > 0;
    public bool isMonster => mosterCount > 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LightArea"))
        {
            lightCount++;
        }
        if (other.CompareTag("SafeArea"))
        {
            safeCount++;
        }

        if (other.CompareTag("Monster"))
        {
            mosterCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightArea"))
        {
            lightCount--;
        }
        if (other.CompareTag("SafeArea"))
        {
            safeCount--;
        }
        if (other.CompareTag("Monster"))
        {
            mosterCount--;
        }
    }
}
