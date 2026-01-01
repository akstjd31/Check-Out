using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    // [SerializeField] private List<AudioClip> clipList;
    [Header("Title")]
    [SerializeField] private AudioClip mainSound;

    [Header("Player")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip[] sanitySounds;  // 정상 ~ 사망순으로 배치할 것

    [Header("Item")]
    [SerializeField] private AudioClip itemPickUpSound;
    [SerializeField] private AudioClip itemEatingSound;
    [SerializeField] private AudioClip buyItemFailedSound;
    [SerializeField] private AudioClip flashLightOnSound;
    [SerializeField] private AudioClip batteryChangedSound;

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

    [Header("Monster")]
    [SerializeField] private AudioClip mannequinAttackSound;
    [SerializeField] private AudioClip walkerAndSirenAttackSound;
    [SerializeField] private AudioClip walkerPatrolSound;
    [SerializeField] private AudioClip walkerChaseSound;
    [SerializeField] private AudioClip sirenLoudSound;
    [SerializeField] private AudioClip achoLaughSound;

    protected override void Awake()
    {
        base.Awake();

        audioSource = this.GetComponent<AudioSource>();
        currentVolume = audioSource.volume;
    }

    public void StopSound() => audioSource.Stop();

    public void PlayBackgroundSound()
    {
        if (hubSceneSound != null && sessionSceneSound != null)
        {
            audioSource.clip = GameManager.Instance.CurrentState.Equals(GameState.Hub) ? hubSceneSound : sessionSceneSound;
            audioSource.Play();
        }
    }

    public void PlayElevatorActionSound()
    {
        if (elevatorActionSound != null)
        {
            audioSource.clip = elevatorActionSound;
            audioSource.Play();
        }
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

    public void PlayMainSound()
    {
        audioSource.clip = mainSound;
        audioSource.loop = true;
        audioSource.volume = currentVolume;

        audioSource.Play();
    }

    // 엘리베이터 버튼 눌렀을 때 
    public void PlayElevatorButtonClickSound()
    {
        if (elevatorButtonClickSound != null)
            audioSource.PlayOneShot(elevatorButtonClickSound);
    }

    // UI 버튼 눌렀을 때
    public void PlayUIButtonClickSound()
    {
        if (uiButtonClickSound != null)
            audioSource.PlayOneShot(uiButtonClickSound);
    }

    // 창고 열었을 때
    public void PlayStorageOpenSound()
    {
        if (storageOpenSound != null)
            audioSource.PlayOneShot(storageOpenSound);
    }

    // 창고 닫았을 때
    public void PlayStorageCloseSound()
    {
        if (storageCloseSound != null)
            audioSource.PlayOneShot(storageCloseSound);
    }

    // 마네킹 공격 사운드
    public void PlayMannequinAttackSound()
    {
        if (mannequinAttackSound != null)
            audioSource.PlayOneShot(mannequinAttackSound);
    }

    // 워커 & 사이렌 공격 사운드
    public void PlayWalkerAndSirenAttackSound()
    {
        if (walkerAndSirenAttackSound != null)
            audioSource.PlayOneShot(walkerAndSirenAttackSound);
    }

    public void PlayPlayerDeathSound()
    {
        if (sanitySounds[3] != null)
            audioSource.PlayOneShot(sanitySounds[3]);
    }
    
    public void PlayEchoLaughSound()
    {
        if (achoLaughSound != null)
            audioSource.PlayOneShot(achoLaughSound);
    }
    
    public AudioClip GetSirenLoudClip() => sirenLoudSound;
    public AudioClip GetWalkerPatrolClip() => walkerPatrolSound;
    public AudioClip GetWalkerChaseClip() => walkerChaseSound;
    public AudioClip GetBatteryChangedClip() => batteryChangedSound;
    public AudioClip GetFlashLightOnClip() => flashLightOnSound;
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
            return null;
    }
}
