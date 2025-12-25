using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSanityVisualController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;
    [Header("첫 번째로 해당 수치만큼 낮아졌을 때 처리")]
    [SerializeField, Range(0, 1)] private float firstWarningValue;

    [Header("두 번째로 해당 수치만큼 낮아졌을 때 처리")]
    [SerializeField, Range(0, 1)] private float secondWarningValue;
    private float currentIntensity;
    private float currentSmoothness;

    private void Awake()
    {
        volume = FindAnyObjectByType<Volume>();

        if (volume != null)
            volume.profile.TryGet(out vignette);
    }

    private void Start()
    {
        if (firstWarningValue < secondWarningValue)
        {
            Debug.Log("수치를 잘못 입력하셨습니다. 현재 두 번째 값이 더 큼");
            return;
        }
    }

    public void UpdateSanity(float sanityPercent)
    {
        if (firstWarningValue < secondWarningValue)
            return;

        if (sanityPercent <= firstWarningValue && sanityPercent > secondWarningValue)
        {
            SetWarningFirst(true);
        }
        else
        {
            SetWarningFirst(false);
            if(sanityPercent <= secondWarningValue && sanityPercent > 0f)
            {
                SetWarningSecond(true);
            }
            else
            {
                SetWarningSecond(false);
            }
        }
    }

    public void UpdateShake(bool onHit)
    {
        if (!onHit)
            return;
        Shake(onHit);
        Invoke(nameof(ShakeOff), 0.3f);
    }

    public void ShakeOff()
    {
        Shake(false);
    }

    public void Shake(bool hit)
    {
        vignette.intensity.value = hit ? 0.4f : currentIntensity;
        vignette.smoothness.value = hit ? 0.4f : currentSmoothness;
    }

    private void SetWarningFirst(bool active)
    {
        vignette.intensity.value = active ? 0.3f : 0f;
        vignette.smoothness.value = active ? 0.3f : 0f;

        currentIntensity = vignette.intensity.value;
        currentSmoothness = vignette.smoothness.value;
    }
    private void SetWarningSecond(bool active)
    {
        if (vignette == null) return;

        vignette.intensity.value = active ? 0.4f : 0f;   // 시야 어두워짐
        vignette.smoothness.value = active ? 0.4f : 0f;   // 시야각 축소 느낌

        currentIntensity = vignette.intensity.value;
        currentSmoothness = vignette.smoothness.value;
    }
}
