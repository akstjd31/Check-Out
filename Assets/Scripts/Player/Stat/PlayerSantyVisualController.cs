using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSantyVisualController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private StatController statController;
    private PlayerCtrl playerCtrl;
    private Vignette vignette;

    private void Awake()
    {
        statController = this.GetComponent<StatController>();
        playerCtrl = this.GetComponent<PlayerCtrl>();
        volume.profile.TryGet(out vignette);
        SetWarning1(false);
    }

    public void UpdateSanity(float sanityPercent)
    {
        if (sanityPercent <= 0.66f && sanityPercent > 0.33f)
        {
            SetWarning1(true);
        }
        else
        {
            SetWarning1(false);
            if(sanityPercent <= 0.33f && sanityPercent > 0f)
            {
                SetWarning2(true);
            }
            else
            {
                SetWarning2(false);
            }
        }
    }

    private void SetWarning1(bool active)
    {
        vignette.intensity.value = active ? 0.3f : 0f;
        vignette.smoothness.value = active ? 0.3f : 0f;
    }
    private void SetWarning2(bool active)
    {
        if (vignette == null) return;

        vignette.intensity.value = active ? 0.4f : 0f;   // 시야 어두워짐
        vignette.smoothness.value = active ? 0.4f : 0f;   // 시야각 축소 느낌
    }
}
