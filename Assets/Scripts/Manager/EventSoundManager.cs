using UnityEngine;

public class EventSoundManager : Singleton<EventSoundManager>
{
    // 경로에 존재하는 파일 클립 반환
    public AudioClip GetAudioClipByPath(string filePath, bool isLoop)
    {
        var clip = Resources.Load<AudioClip>(filePath);

        if (filePath == null)
        {
            Debug.LogError("해당 경로에 파일이 존재하지 않습니다!");
            return null;
        }

        return clip;
    }
}
