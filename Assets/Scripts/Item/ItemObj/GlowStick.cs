using UnityEngine;

public class GlowStick : ItemObj
{
    private Animator animator;
    private AudioSource audioSource;
    private GameObject lightArea;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lightArea = transform.GetChild(0).gameObject;
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
        lightArea.SetActive(true);
    }

    private void LightOff()
    {
        animator.Play("GlowStickOFF");
        lightArea.SetActive(false);
    }

}
