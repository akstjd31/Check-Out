using System;
using System.Collections;
using Unity.VisualScripting;
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
    private float invincibleTimer;
    private float invincibleTime;

    [Header("Value")]
    [SerializeField] private bool isInvincible = false;  // 무적 상태인지?
    public bool onHit = false;

    private EchoSpawnSystem echoSpawnSystem;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        playerCamera = this.GetComponent<PlayerCameraController>();
        visual = this.GetComponent<PlayerSanityVisualController>();
        soundController = this.GetComponent<PlayerSoundController>();
        echoSpawnSystem = this.GetComponent<EchoSpawnSystem>();
        stateMachine = this.GetComponent<PlayerStateMachine>();

        invincibleTime = stat.DefaultInvincibilityTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamagedArea"))
        {
            if (isInvincible)
                return;
            Monster model = other.GetComponentInParent<Monster>();
            if (model == null)
                return;

            if (model != null)
            {
                hitMonster = model;
            }

            hitMonster = model;

            if (stat.IsRemainSanity())
            {
                if (model is MannequinModel)
                    SoundManager.Instance.PlayMannequinAttackSound();
                else if (model is SirenModel || model is WalkerModel)
                    SoundManager.Instance.PlayWalkerAndSirenAttackSound();

                if (model is EchoModel)
                {
                    SoundManager.Instance.PlayEchoLaughSound();
                    echoSpawnSystem.DisableEcho();
                }
            }
        }
    }

    private void UpdateMonsterDamage()
    {

        if (isInvincible)
            return;

        if (hitMonster == null)
            return;

        if (stat.IsRemainSanity())
        {
            onHit = true;
            playerCamera.Hit();
            Debug.LogWarning("데미지 입음!");

            stat.ChangeSanity(onHit, -hitMonster.Power);
            Debug.Log(onHit);
            OnHitInvincible();

            visual.UpdateShake(onHit);
            onHit = false;
        }
        
    }

    private void OnHitInvincible()
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
        UpdateMonsterDamage();
        UpdateInvincibility();
    }
}
