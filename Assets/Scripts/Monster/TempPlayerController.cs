using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TempPlayerController : MonoBehaviour
{
    private MonsterSoundDetect monsterSoundDetect;

    private InputAction moveAction;
    private PlayerInput playerInput;

    [SerializeField] private float moveSpeed = 1;
    private Vector3 moveInput;

    private event Action<Transform> soundAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        
    }

    private void Start()
    {
        // 몬스터 태그를 통해서 현재 씬에 있는 몬스터를 불러와서 컴포넌트 추가
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("SoundDetect");
        foreach (GameObject monster in monsters)
        {
            MonsterSoundDetect detect = monster.GetComponent<MonsterSoundDetect>();
            if (detect != null)
            {
                soundAction += detect.DetectSound;
            }
            else
            {
                Debug.LogWarning($"{monster.name}에는 MonsterSoundDetect가 없습니다!");
            }

        }
    }

    // 플레이어 인풋 구독 (걷기, 달리기)
    private void OnEnable()
    {
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;


        //soundAction += monsterSoundDetect.DetectSound;
    }


    // 플레이어 인풋 구독 해제 (걷기, 달리기)
    private void OnDisable()
    {
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;

        //soundAction -= monsterSoundDetect.DetectSound;
    }
    private void FixedUpdate()
    {
        // 이동
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        this.transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
    }

    // 이동 키를 입력받고 있을 때
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("플레이어가 움직임");
        moveInput = context.ReadValue<Vector3>();
        soundAction?.Invoke(transform);
    }

    // 이동 취소되었을 때
    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector3.zero;
    }
}
