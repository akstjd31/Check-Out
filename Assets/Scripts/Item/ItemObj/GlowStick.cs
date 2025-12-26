using UnityEngine;

public class GlowStick : ItemObj
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        animator.Play("GlowStickOn");
    }

    private void LightOff()
    {
        animator.Play("GlowStickOFF");
    }

}
