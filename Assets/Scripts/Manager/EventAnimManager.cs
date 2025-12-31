using UnityEngine;

public class EventAnimManager : Singleton<EventAnimManager>
{
    // 경로에 존재하는 파일 애니메이션 클립 변환
    public AnimationClip GetAnimClipByPath(string filePath)
    {
        var clip = Resources.Load<AnimationClip>(filePath);

        if (filePath == null)
        {
            //debug.LogError("해당 경로에 파일이 존재하지 않습니다!");
            return null;
        }

        return clip;
    }
}
