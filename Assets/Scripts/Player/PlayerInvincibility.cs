using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerSanity))]
[RequireComponent(typeof(StatController))]
public class PlayerInvincibility : MonoBehaviour
{
    private StatController stat;
    [SerializeField] private bool isInvincible = false;  // 무적 상태인지?
    
    private void Awake()
    {
        stat = this.GetComponent<StatController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;

        if (other.CompareTag("DamagedArea"))
        {
            var monster = other.GetComponentInParent<Monster>();
            
            if (monster != null)
            {
                Debug.LogWarning("데미지 입음!");
                stat.ConsumeSanity(monster.Power);
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
