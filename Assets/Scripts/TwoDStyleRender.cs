using UnityEngine;

public enum RenderState {test}
public enum ShowedState
{
    front, bottom, left, right
}

public abstract class TwoDStyleRender : TwoDStyleObj
{
    public AnimationState animationState;

    public delegate void StateChangedHandler(RenderState state);
    public event StateChangedHandler OnStateChanged;

    [SerializeField] protected Transform sideRender;

    [SerializeField] protected Transform orentationChecker;

    public Sprite frontTexture;
    public Sprite bottomTexture;
    public Sprite leftTexture;
    public Sprite rightTexture;

    protected Transform activeRender;
    protected SpriteRenderer activeRenderer;

    private void OnEnable()
    {
        Debug.Log($"{name}: OnEnable subscribe");
        CameraTwoD.onPreCull += FixTheCamera;
    }

    private void OnDisable()
    {
        CameraTwoD.onPreCull -= FixTheCamera;
    }

    public abstract void FixTheCamera(Transform camera);

    public Transform SideRender
    {
        get { return sideRender; }
        set { sideRender = value; }
    }

    protected virtual void activePlate(Transform plate)
    {
        activeRender = plate;
        activeRender.gameObject.SetActive(true);
        activeRenderer = activeRender.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected virtual void SetActualShowedState(ShowedState state)
    {
        //Debug.Log("ActiveRenderer: " + activeRenderer + " frontTexture: " + frontTexture);
        switch (state)
        {
            case ShowedState.front:
                activeRenderer.sprite = frontTexture;
                break;
            case ShowedState.bottom:
                activeRenderer.sprite = bottomTexture;
                break;
            case ShowedState.left:
                activeRenderer.sprite = leftTexture;
                break;
            case ShowedState.right:
                activeRenderer.sprite = rightTexture;
                break;
            //case ShowedState.frontLeft:
            //    break;
            //case ShowedState.frontRight:
            //    break;
            //case ShowedState.bottomLeft:
            //    break;
            //case ShowedState.bottomRight:
            //    break;
            //case ShowedState.top:
            //    break;
            //case ShowedState.down:
            //    break;
        }
    }

    protected abstract void faceToShow(float yAngle);

}
