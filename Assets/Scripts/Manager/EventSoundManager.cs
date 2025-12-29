using UnityEngine;

public class EventSoundManager : Singleton<EventSoundManager>
{
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();

        audioSource = this.GetComponent<AudioSource>();
    }

        // 경로에 존재하는 파일 클립 반환
    public void AudioSourceSettingByPath(string filePath, bool isLoop)
    {
        var clip = Resources.Load<AudioClip>(filePath);

        if (filePath == null)
        {
            Debug.LogError("해당 경로에 파일이 존재하지 않습니다!");
            return;
        }
        
        if (audioSource == null)
        {
            Debug.LogError("사운드 매니저에 오디오 소스가 없습니다!");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = isLoop;
    }

    public void PlaySoundWithDelay(float delay)
    {
        Debug.Log($"현재 딜레이 값: {delay}");
        audioSource.PlayDelayed(delay);
    }

    public void StopSound() => audioSource.Stop();
}
