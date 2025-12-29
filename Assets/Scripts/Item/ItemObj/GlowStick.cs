using UnityEngine;

public class GlowStick : ItemObj
{
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnItem += LightOn;
        OffItem += LightOff;
    }

    private void OnDisable()
    {
        OnItem -= LightOn;
        OffItem -= LightOff;
    }

    private void LightOn()
    {
        audioSource.Play();
        animator.Play("GlowStickOn");
    }

    private void LightOff()
    {
        animator.Play("GlowStickOFF");
    }

}
