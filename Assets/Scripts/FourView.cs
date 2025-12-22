using UnityEngine;

public class FourView : TwoDStyleRender
{
    private const float heightSlide = 360.0f / 8.0f;

    protected void Start()
    {
        activePlate(sideRender);
    }

    public override void FixTheCamera(Transform camera)
    {
        Debug.Log($"{name}: FixTheCamera called");
        orentationChecker.LookAt(camera);
        faceToShow(orentationChecker.localEulerAngles.y);
        activeRender.localEulerAngles = new Vector3(0.0f, orentationChecker.localEulerAngles.y, 0.0f);
    }

    protected override void faceToShow(float yAngle)
    {
        if (yAngle >= 7 * heightSlide || yAngle < heightSlide)
        {
            SetActualShowedState(ShowedState.front);
        }
        else if (yAngle < (3 * (360.0f / 8.0f)))
        {
            SetActualShowedState(ShowedState.right);
        }
        else if (yAngle < (5 * (360.0f / 8.0f)))
        {
            SetActualShowedState(ShowedState.bottom);
        }
        else
        {
            SetActualShowedState(ShowedState.left);
        }
    }
}
