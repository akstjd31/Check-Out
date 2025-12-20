using UnityEngine;

public class LookPlayer : MonoBehaviour
{

    private GameObject maimCamera;
    private Transform target;
    Vector3 pos;

    void Start()
    {
        maimCamera = GameObject.Find("Main Camera");
        Debug.Log(maimCamera.name);
        target = maimCamera.transform;
    }
    
    // 플레이어 바라보게 하기
    void Update()
    {
        if (target != null)
        {
            pos = new Vector3 (target.position.x, transform.position.y, target.position.z);
            transform.LookAt(pos);
        }
    }
}
