using System.Collections.Generic;
using UnityEngine;

// 상태머신 클래스
public class StateMachine<T>
{
    private readonly Dictionary<T, IState> states = new();  // 상태 관련 데이터 <키, 상태>
    private IState currentState;                            // 현 상태

    // 상태 추가하기
    public void AddState(T key, IState state)
    {
        if (!states.ContainsKey(key))
            states.Add(key, state);
        else
            Debug.Log("이미 존재하는 키입니다!");
    }

    // 상태 전이
    public void ChangeState(T key)
    {
        currentState?.Exit();
        currentState = states[key];
        currentState.Enter();
    }

    // 상태 갱신
    public void Update() => currentState?.Update();
}
