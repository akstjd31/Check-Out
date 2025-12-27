using UnityEngine;

public class MonsterDetector : MonoBehaviour
{
    private Rigidbody rb;
    public float stopTime = 0.01f;
    private FourView fourView;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fourView = GetComponent<FourView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
