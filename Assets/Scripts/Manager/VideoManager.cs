using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : Singleton<VideoManager>
{
    [SerializeField] private VideoClip walker;
    [SerializeField] private VideoClip siren;
    [SerializeField] private VideoClip echo;
    [SerializeField] private VideoClip mannequin;
    [SerializeField] private RenderTexture monsterTextur;
    [SerializeField] private VideoPlayer videoPlayer;
    private RawImage rawImage;

    private bool isPlaying = false;


    public void PlayDeathVideo(Monster monster)
    {
        if (monster == null)
            return;

        if (monster is WalkerModel)
        {
            isPlaying = true;
            videoPlayer.clip = walker;
        }
        else if (monster is SirenModel)
        {
            isPlaying = true;
            videoPlayer.clip = siren;
        }
        else if (monster is MannequinModel)
        {
            isPlaying = true;
            videoPlayer.clip = mannequin;
        }
        else if (monster is EchoModel)
        {
            isPlaying = true;
            videoPlayer.clip = echo;
        }

        videoPlayer.Play();
        OnRawImage();
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Hub && rawImage == null) 
        {
            rawImage = FindAnyObjectByType<RawImageTracker>().GetComponentInChildren<RawImage>(true);
        }
    }

    public void OnRawImage()
    {
        rawImage.enabled = true;
    }
    public void OffRawImage()
    {
        rawImage.enabled = false;
    }

}
