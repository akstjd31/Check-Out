using UnityEngine;

public class MonsterDetector : MonoBehaviour
{
    private Rigidbody rb;
    public float stopTime = 0.01f;
    private FourView fourView;
    private SirenModel sirenModel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fourView = GetComponent<FourView>();
        sirenModel = GetComponent<SirenModel>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (sirenModel.monsterState == Monster.MonsterState.Alert)
            return;

        if (rb.linearVelocity.magnitude > stopTime)
        {
            fourView.animState = AnimState.Move;
        }
        else
        {
            fourView.animState = AnimState.Idle;
        }
    }
}
