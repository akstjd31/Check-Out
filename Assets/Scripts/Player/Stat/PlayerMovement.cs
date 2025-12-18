using UnityEngine;

// 플레이어 이동 담당
public class PlayerMovement : MonoBehaviour
{
    private StatController stat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stat = this.GetComponent<StatController>();
    }

    // Update is called once per frame
    public void Move(Vector2 input)
    {
        Vector3 dir = new Vector3(input.x, 0, input.y);
        this.transform.Translate(dir * stat.CurrentMoveSpeed * Time.deltaTime);
    }
}
