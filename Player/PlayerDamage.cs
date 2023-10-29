using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    WarriorAttack warriorAtk;
    PlayerStateManager stateManager;

    private void Awake()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        if (FindObjectOfType<WarriorAttack>() != null)
            warriorAtk = FindObjectOfType<WarriorAttack>();
    }

    public float Damage;

    public float DMG()
    {
        if (warriorAtk != null)
        {
            if (warriorAtk.NormalAtk())
            {
                return stateManager.GetATK();
            }
            else
                return stateManager.GetATK() * Damage;
        }
        else
            return (float)stateManager.GetATK() * Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("WOLF"))
        {
            if(!this.CompareTag("Arrow"))
                GetComponent<Collider>().enabled = false;
        }
    }
}
