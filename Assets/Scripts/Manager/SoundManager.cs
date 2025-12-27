using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    // [SerializeField] private List<AudioClip> clipList;
    [Header("Player")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip[] sanitySounds;  // 정상 ~ 사망순으로 배치할 것

    protected override void Awake()
    {
        base.Awake();

        audioSource = this.GetComponent<AudioSource>();
    }

    public AudioClip GetWalkClip() => walkSound;
    public AudioClip GetRunClip() => runSound;
    public AudioClip GetSanityClip(float value)
    {   
        if (value >= 60)
            return sanitySounds[0];
        else if (30 <= value && value <= 59)
            return sanitySounds[1];
        else if (1 <= value && value <= 29)
            return sanitySounds[2];
        else
            return sanitySounds[3];
    }


    // 경로에 존재하는 파일 재생
    public void PlaySoundWithPath(string filePath)
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

        audioSource.PlayOneShot(clip);
    }
}
