using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;



public class PlayerHitDeath : MonoBehaviour
{
    [SerializeField] private VideoPlayer walker;
    [SerializeField] private Texture walkerTextur;
    [SerializeField] private VideoPlayer siren;
    [SerializeField] private Texture sirenTextur;
    [SerializeField] private VideoPlayer mannequin;
    [SerializeField] private Texture mannequinTextur;
    [SerializeField] private VideoPlayer echo;
    [SerializeField] private Texture echoTextur;

    public RawImage rawImage;

    private bool isplaying = false;
    private void Awake()

    {
        InitVideo(walker);
        InitVideo(siren);
        InitVideo(mannequin);
        InitVideo(echo);

    }

    private void InitVideo(VideoPlayer vp)
    {
        vp.playOnAwake = false;
        vp.Stop();
        vp.gameObject.SetActive(false);
    }

    public void PlayDeathVideo(Monster monster)
    {
        if (monster == null)
            return;

        VideoPlayer target = null;

        if (monster is WalkerModel)
        {
            isplaying = true;
            target = walker;
            rawImage.texture = walkerTextur;
        }
        else if (monster is SirenModel)
        {
            isplaying = true;
            target = siren;
            rawImage.texture = sirenTextur;
        }
        else if (monster is MannequinModel)
        {
            isplaying = true;
            target = mannequin;
            rawImage.texture= mannequinTextur;
        }
        else if (monster is EchoModel)
        {
            isplaying = true;
            target = echo;
            rawImage.texture = echoTextur;
        }

        if (target == null)
            return;

        target.gameObject.SetActive(true);
        target.Play();
        if (!isplaying)
        {
           rawImage.enabled = false;
        }

    }
}
