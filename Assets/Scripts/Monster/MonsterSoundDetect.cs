using UnityEngine;

public class MonsterSoundDetect : MonoBehaviour, ISound
{
    public void DetectSound(Transform inputTransform)
    {
        Debug.Log("사운드 발생");
    }
}
