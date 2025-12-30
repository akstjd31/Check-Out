using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerSanity))]
[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerCameraController))]
public class PlayerInvincibility : MonoBehaviour
{

    public Monster hitMonster { get; private set; }
    [Header("Component")]
    private StatController stat;
    private PlayerCameraController playerCamera;
    private PlayerSanityVisualController visual;
    private PlayerSoundController soundController;
    private PlayerStateMachine stateMachine;
    private bool isInDamageArea;
    private Monster currentMonster;
    private float invincibleTimer;

    [Header("Value")]
    [SerializeField] private bool isInvincible = false;  // 무적 상태인지?
    public bool onHit = false;
    public bool hit = false;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        playerCamera = this.GetComponent<PlayerCameraController>();
        visual = this.GetComponent<PlayerSanityVisualController>();
        soundController = this.GetComponent<PlayerSoundController>();
        stateMachine = this.GetComponent<PlayerStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.GetComponentInParent<Monster>();
        Monster model = other.GetComponentInParent<Monster>();
        if (isInvincible)
            return;
        if (other.CompareTag("DamagedArea"))
        {

            if (monster == null)
                return;

            if (model != null)
            {
                hitMonster = model;
            }
            currentMonster = monster;
            isInDamageArea = true;
            hit = true;

            if (monster is MannequinModel)
                SoundManager.Instance.PlayMannequinAttackSound();
            else if (monster is SirenModel || monster is WalkerModel)
                SoundManager.Instance.PlayWalkerAndSirenAttackSound();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("DamagedArea"))
            return;

        Monster monster = other.GetComponentInParent<Monster>();
        if (monster == currentMonster)
        {
            isInDamageArea = false;
            currentMonster = null;
            hit = false;
        }
    }

    private void UpdateMonsterDamage()
    {
        if (!isInDamageArea)
            return;

        if (isInvincible)
            return;

        if (hit)
        {
            onHit = true;
            playerCamera.Hit();
            Debug.LogWarning("데미지 입음!");

            stat.ChangeSanity(onHit, -currentMonster.Power);
            Debug.Log(onHit);
            OnHitInvincible();

            visual.UpdateShake(onHit);
        }
        onHit = false;
    }

    private void OnHitInvincible()
    {
        isInvincible = true;
        invincibleTimer = stat.DefaultInvincibilityTime;
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

    public void ClearHitMonster()
    {
        hitMonster = null;
    }
}
