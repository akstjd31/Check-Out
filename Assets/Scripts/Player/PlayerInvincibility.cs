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
    public bool onHit = false;

    private void Awake()
    {
        stat = GetComponent<StatController>();
        playerCamera = GetComponent<PlayerCameraController>();
        visual = GetComponent<PlayerSanityVisualController>();
        soundController = GetComponent<PlayerSoundController>();
        echoSpawnSystem = GetComponent<EchoSpawnSystem>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start() => invincibleTime = stat.DefaultInvincibilityTime;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DamagedArea"))
            return;

        if (isInvincible)
            return;

        Monster monster = other.GetComponentInParent<Monster>();
        if (monster == null)
            return;

        ApplyDamage(monster);
    }

    private void ApplyDamage(Monster monster)
    {
        if (!stat.IsRemainSanity())
            return;

        onHit = true;
        hitMonster = monster;

        // 🔊 사운드 처리
        if (monster is MannequinModel)
            SoundManager.Instance.PlayMannequinAttackSound();
        else if (monster is SirenModel || monster is WalkerModel)
            SoundManager.Instance.PlayWalkerAndSirenAttackSound();
        else if (monster is EchoModel)
        {
            SoundManager.Instance.PlayEchoLaughSound();
            echoSpawnSystem.DisableEcho();
        }

        // 📷 카메라 & 비주얼
        playerCamera.Hit();
        visual.UpdateShake(onHit);

        // 💥 데미지 적용 (1회)
        stat.ChangeSanity(onHit, -monster.Power);
        Debug.LogWarning("플레이어 데미지 1회 적용");

        // 🛡 무적 시작
        StartInvincibility();

        onHit = false;
        hitMonster = null; // 중복 데미지 방지
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

    private void Update()
    {
        UpdateInvincibility();
    }
}
