using System;
using UnityEngine;

[RequireComponent(typeof(PlayerSanity))]
[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerCameraController))]
public class PlayerInvincibility : MonoBehaviour
{
    public Monster hitMonster { get; set; }

    [Header("Component")]
    private StatController stat;
    private PlayerCameraController playerCamera;
    private PlayerSanityVisualController visual;
    private PlayerSoundController soundController;
    private PlayerStateMachine stateMachine;
    private EchoSpawnSystem echoSpawnSystem;

    [Header("Invincibility")]
    private float invincibleTimer;
    private float invincibleTime;

    [SerializeField] private bool isInvincible = false;
    private bool onHit = false;

    private void Awake()
    {
        stat = GetComponent<StatController>();
        playerCamera = GetComponent<PlayerCameraController>();
        visual = GetComponent<PlayerSanityVisualController>();
        soundController = GetComponent<PlayerSoundController>();
        echoSpawnSystem = GetComponent<EchoSpawnSystem>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        invincibleTime = stat.DefaultInvincibilityTime;
    }

    private void Update()
    {
        UpdateInvincibility();
    }

    /// <summary>
    /// 공격 범위 안에 있는 동안 호출됨
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("DamagedArea"))
            return;

        // 무적 중이면 데미지 X
        if (isInvincible)
            return;

        Monster monster = other.GetComponentInParent<Monster>();
        if (monster == null)
            return;

        if (monster is MannequinModel && monster.monsterState == Monster.MonsterState.Stop) return;

        ApplyDamage(monster);
    }

    private void ApplyDamage(Monster monster)
    {
        if (!stat.IsRemainSanity())
            return;

        onHit = true;
        hitMonster = monster;

        if (monster is MannequinModel)
            SoundManager.Instance.PlayMannequinAttackSound();
        else if (monster is SirenModel || monster is WalkerModel)
            SoundManager.Instance.PlayWalkerAndSirenAttackSound();
        else if (monster is EchoModel)
        {
            SoundManager.Instance.PlayEchoLaughSound();
            echoSpawnSystem.DisableEcho();
        }

        playerCamera.Hit();
        visual.UpdateShake(onHit);

        stat.ChangeSanity(true, -monster.Power);
        //debug.LogWarning("플레이어 데미지 적용");

        StartInvincibility();

        onHit = false;
        hitMonster = null;
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibleTimer = invincibleTime;
    }

    private void UpdateInvincibility()
    {
        if (!isInvincible)
            return;

        invincibleTimer -= Time.deltaTime;

        if (invincibleTimer <= 0f)
        {
            isInvincible = false;
        }
    }
}
