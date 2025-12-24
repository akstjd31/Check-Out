using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerSanity))]
[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerCameraController))]
public class PlayerInvincibility : MonoBehaviour
{
    private StatController stat;
    private PlayerCameraController playerCamera;
    private PlayerSanityVisualController visual;
    [SerializeField] private bool isInvincible = false;  // 무적 상태인지?
    private int power = 5000;
    public bool onHit = false;

    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        playerCamera = this.GetComponent<PlayerCameraController>();
        visual = this.GetComponent<PlayerSanityVisualController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;

        if (other.CompareTag("DamagedArea"))
        {
            var monster = other.GetComponentInParent<Monster>();

            if (monster != null)
            {
                onHit = true;
                playerCamera.Hit();
                Debug.LogWarning("데미지 입음!");
                stat.ConsumeSanity(onHit, monster.Power);
                StartCoroutine(InvincibleCoroutine());
                visual.UpdateShake(onHit);
            }
        }

    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(stat.DefaultInvincibilityTime);
        isInvincible = false;
    }
}
