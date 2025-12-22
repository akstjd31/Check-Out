using UnityEngine;
using UnityEngine.Rendering;

public class CameraTwoD : MonoBehaviour
{
    private Transform camTransform;

    //private void Awake()
    //{
    //    camTransform = this.GetComponent<Transform>();
    //}

    public delegate void PreCullEvent(Transform camera);
    public static PreCullEvent onPreCull;

    //private void OnPreCull()
    //{
    //    if (onPreCull != null)
    //    {
    //        onPreCull?.Invoke(camTransform);
    //    }
    //}

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
    }

    private void BeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        if (cam != GetComponent<Camera>()) return;

        onPreCull?.Invoke(cam.transform);
    }
}

