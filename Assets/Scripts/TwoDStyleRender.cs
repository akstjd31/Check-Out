using UnityEngine;

public enum RenderState {test}

public enum AnimState {Idle, Move}
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

    public AnimState animState = AnimState.Idle;

    public Animator animator;

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

    // 플레이트 설정
    protected virtual void activePlate(Transform plate)
    {
        activeRender = plate;
        activeRender.gameObject.SetActive(true);
        activeRenderer = activeRender.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // 방향에 따른 스프라이트 넣기
    protected virtual void SetActualShowedState(ShowedState state)
    {
        //Debug.Log("ActiveRenderer: " + activeRenderer + " frontTexture: " + frontTexture);
        if (animState == AnimState.Idle)
        {
            switch (state)
            {
                case ShowedState.front:
                    activeRenderer.sprite = frontTexture;
                    activeRenderer.flipX = false;
                    break;
                case ShowedState.bottom:
                    activeRenderer.sprite = bottomTexture;
                    activeRenderer.flipX = false;
                    break;
                case ShowedState.left:
                    activeRenderer.sprite = leftTexture;
                    activeRenderer.flipX = true;
                    break;
                case ShowedState.right:
                    activeRenderer.sprite = rightTexture;
                    activeRenderer.flipX = false;
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

        if (animState == AnimState.Move)
        {
            switch (state)
            {
                case ShowedState.front:
                    animator.Play("FrontMove");
                    activeRenderer.flipX = false;
                    break;
                case ShowedState.bottom:
                    animator.Play("BottomMove");
                    activeRenderer.flipX = false;
                    break;
                case ShowedState.left:
                    animator.Play("SideMove");
                    activeRenderer.flipX = true;
                    break;
                case ShowedState.right:
                    animator.Play("SideMove");
                    activeRenderer.flipX = false;
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
    }
    protected abstract void faceToShow(float yAngle);

}
