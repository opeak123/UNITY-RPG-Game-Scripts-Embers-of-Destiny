using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttack : MonoBehaviour
{
    Animator anim;
    PlayerStateManager stateManager;

    float atkSpeed;

    void Awake()
    {
        
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        atkSpeed = stateManager.GetATKSPEED();
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("isAttack");
        }

    }

    public void AttackStart()
    {
        anim.speed = atkSpeed;
        stateManager.Attack();
    }

    public void AttackEnd()
    {
        anim.speed = 1.0f;
        stateManager.AttackEnd();
    }
}
