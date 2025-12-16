using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> moveTransform;

    private NavMeshAgent navMeshAgent;
    private PlayerInput monsterInput;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        monsterInput.actions["Move"].started -= OnMove;
    }

    private void Init()
    {
        // 내비매쉬 에이전트 겟 컴포넌트 후 정지 상태로 변경
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;

        // 인풋 시스템 설정
        monsterInput = GetComponent<PlayerInput>();
        monsterInput.actions["Move"].started += OnMove;
    }
    // 입력받은 위치로 이동하는 매서드
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} {moveTransform[0].name}로 이동 시작 ");
        navMeshAgent.SetDestination(moveTransform[0].position);
        navMeshAgent.isStopped = false;
    }
}
