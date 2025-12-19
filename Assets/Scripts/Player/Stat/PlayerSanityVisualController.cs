using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSanityVisualController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    private void Awake() => volume.profile.TryGet(out vignette);

    public void UpdateSanity(float sanityPercent)
    {
        if (sanityPercent <= 0.66f && sanityPercent > 0.33f)
        {
            SetWarningFirst(true);
        }
        else
        {
            SetWarningFirst(false);
            if(sanityPercent <= 0.33f && sanityPercent > 0f)
            {
                SetWarningSecond(true);
            }
            else
            {
                SetWarningSecond(false);
            }
        }
    }

    private void SetWarningFirst(bool active)
    {
        vignette.intensity.value = active ? 0.3f : 0f;
        vignette.smoothness.value = active ? 0.3f : 0f;
    }
    private void SetWarningSecond(bool active)
    {
        if (vignette == null) return;

        vignette.intensity.value = active ? 0.4f : 0f;   // 시야 어두워짐
        vignette.smoothness.value = active ? 0.4f : 0f;   // 시야각 축소 느낌
    }
}
