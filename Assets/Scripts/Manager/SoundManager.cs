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

    [Header("Item")]
    [SerializeField] private AudioClip itemPickUpSound;
    [SerializeField] private AudioClip itemEatingSound;
    [SerializeField] private AudioClip buyItemFailedSound;

    [Header("Elevator")]
    [SerializeField] private AudioClip elevatorButtonClickSound;
    [SerializeField] private AudioClip elevatorActionSound;

    [Header("UI")]
    [SerializeField] private AudioClip uiButtonClickSound;

    [Header("Store")]
    [SerializeField] private AudioClip buyItemSound;
    [SerializeField] private AudioClip sellItemSound;

    [Header("Storage")]
    [SerializeField] private AudioClip storageOpenSound;
    [SerializeField] private AudioClip storageCloseSound;
    
    [Header("Background")]
    [SerializeField] private AudioClip hubSceneSound;
    [SerializeField] private AudioClip sessionSceneSound;

    [Header("Value")]
    private float currentVolume;
    [SerializeField] private float volumeSpeed;

    protected override void Awake()
    {
        base.Awake();

        audioSource = this.GetComponent<AudioSource>();
        currentVolume = audioSource.volume;
    }

    public void PlayBackgroundSound()
    {
        audioSource.clip = GameManager.Instance.CurrentState.Equals(GameState.Hub) ? hubSceneSound : sessionSceneSound;
        audioSource.Play();
    }

    public void PlayElevatorActionSound()
    {
        audioSource.clip = elevatorActionSound;
        audioSource.Play();
    }

    // 볼륨 서서히 줄이기
    public void DecreaseVolume()
    {
        if (audioSource.volume > 0f)
            audioSource.volume -= volumeSpeed * Time.deltaTime;
    }

    // 볼륨 서서히 높이기
    public void IncreaseVolume()
    {
        if (audioSource.volume < currentVolume)
            audioSource.volume += volumeSpeed * Time.deltaTime;
    }

    public void StopSound()
    {
        audioSource.volume = currentVolume;
        audioSource.Stop();
    }

    public void PlayElevatorButtonClickSound() => audioSource.PlayOneShot(elevatorButtonClickSound);
    public void PlayUIButtonClickSound() => audioSource.PlayOneShot(uiButtonClickSound);
    public void GetStorageOpenSound() => audioSource.PlayOneShot(storageOpenSound);
    public void GetStorageCloseSound() => audioSource.PlayOneShot(storageCloseSound);
    public AudioClip GetSellItemClip() => sellItemSound;
    public AudioClip GetBuyItemClip() => buyItemSound;
    public AudioClip GetBuyItemFailedClip() => buyItemFailedSound;
    public AudioClip GetItemEatingClip() => itemEatingSound;
    public AudioClip GetItemPickUpClip() => itemPickUpSound;
    public AudioClip GetWalkClip() => walkSound;
    public AudioClip GetRunClip() => runSound;
    public AudioClip GetSanityClip(float value)
    {   
        if (value >= 60f)
            return sanitySounds[0];
        else if (30f <= value && value <= 59f)
            return sanitySounds[1];
        else if (1f <= value && value <= 29f)
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
