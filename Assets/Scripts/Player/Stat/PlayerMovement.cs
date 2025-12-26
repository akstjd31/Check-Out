using UnityEngine;

// 플레이어 이동 담당
public class PlayerMovement : MonoBehaviour
{
    private StatController stat;
    private Rigidbody rigid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stat = this.GetComponent<StatController>();
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Move(Vector2 input)
    {
        Vector3 dir = new Vector3(input.x, 0, input.y);
        
        this.transform.Translate(dir * stat.CurrentMoveSpeed * Time.fixedDeltaTime);

        // 리지드바디 값으로 이동하기 (물리 이동 연산이라 충돌 버그 적음)
        // Vector3 moveDir = this.transform.forward * input.x + this.transform.right * input.y;
        // rigid.linearVelocity = moveDir.normalized * stat.CurrentMoveSpeed * Time.fixedDeltaTime;
    }

}
