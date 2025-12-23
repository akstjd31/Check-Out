using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Lumin;

[RequireComponent(typeof(PlayerSanity))]
[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerCamera))]
public class PlayerInvincibility : MonoBehaviour
{
    private StatController stat;
    private PlayerCamera playerCamera;
    [SerializeField] private bool isInvincible = false;  // 무적 상태인지?
    private int power = 5000;
    public bool isHit;
    
    private void Awake()
    {
        stat = this.GetComponent<StatController>();
        playerCamera = this.GetComponent<PlayerCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;

        if (other.CompareTag("DamagedArea"))
        {
            var monster = other.GetComponentInParent<Monster>();
            
            if (monster != null)
            {
                playerCamera.Hit();
                Debug.LogWarning("데미지 입음!");
                stat.ConsumeSanity(monster.Power);
                if (!stat.IsRemainSanity())
                {
                    isHit = true;
                }
                StartCoroutine(InvincibleCoroutine());
            }
        }
    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        // Physics.IgnoreLayerCollision(
        //     LayerMask.NameToLayer("Player"),
        //     LayerMask.NameToLayer("Monster"),
        //     true
        // );

        yield return new WaitForSeconds(stat.DefaultInvincibilityTime);

        // Physics.IgnoreLayerCollision(
        //     LayerMask.NameToLayer("Player"),
        //     LayerMask.NameToLayer("Monster"),
        //     false
        // );

        isInvincible = false;
    }
}
