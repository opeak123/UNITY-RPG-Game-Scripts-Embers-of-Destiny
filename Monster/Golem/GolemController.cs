#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

[RequireComponent (typeof(GolemPhaseState))]
[RequireComponent(typeof(GolemAnimation))]
[RequireComponent(typeof(GolemAttack))]
[RequireComponent(typeof(GolemHpBarUI))]
[RequireComponent(typeof(GolemMovement))]
public class GolemController : MonoBehaviour
{
    public GolemPhaseState golemPhase;
    private GolemAnimation golemAnimation;
    private GolemAttack golemAttack;
    private GolemHpBarUI golemHpBarUI;
    private GolemMovement golemMovement;

    public bool isWalk = false;
    public bool isAttack = false;
    public bool isThrow = false;
    public bool isIdle = false;

    public ParticleSystem phaseTwoParticle;
    public ParticleSystem phaseThreeParticle;

    private void Start()
    {
        golemAnimation = GetComponent<GolemAnimation>();
        golemAttack = GetComponent<GolemAttack>();
        golemHpBarUI = GetComponent<GolemHpBarUI>();
        golemMovement = GetComponent<GolemMovement>();
        golemPhase = GolemPhaseState.Phase1;

        isWalk = golemMovement.isWalk;
        isAttack = golemAttack.isAttack;
        isThrow = golemAttack.isThrow;

        StartCoroutine(OnceAttack());
    }

    private void Update()
    {
        isWalk = isAttack || isThrow ? false : isWalk;
        isAttack = isWalk || isThrow ? false : isAttack;
        isThrow = isWalk || isAttack ? false : isThrow;
        isIdle = isWalk || isThrow || isAttack ? false : true;

        golemHpBarUI.GolemHpUpdate();
        golemMovement.CalculateDistance();
        golemAttack.GolemAttackUpdate();
        //golemMovement.GolemMovementUpdate();

        if (golemPhase == GolemPhaseState.Phase1)
        {
            GolemState();
            golemAttack.isRandomAttacking = false;
            phaseTwoParticle.gameObject.SetActive(false);
            phaseThreeParticle.gameObject.SetActive(false);
        }
        else if (golemPhase == GolemPhaseState.Phase2)
        {
            GolemState();
            golemAttack.isRandomAttacking = false;
            golemMovement.navMesh.speed = 4f;
            golemHpBarUI.data.atk = 150;
            golemHpBarUI.data.def = 280;
            phaseTwoParticle.gameObject.SetActive(true);
        }
        else if (golemPhase == GolemPhaseState.Phase3)
        {
            GolemState();
            golemAttack.isRandomAttacking = true;
            golemAttack.RandomAttack();
            golemMovement.navMesh.speed = 5f;
            golemHpBarUI.data.atk = 200;
            golemHpBarUI.data.def = 180;
            phaseThreeParticle.gameObject.SetActive(true);
        }
    }

    private void GolemState()
    {
        //if (Input.anyKeyDown)
        //{
        //    isAttack = false;
        //    golemAnimation.Damage();
        //    golemHpBarUI.GolemDamaged(1000,transform);
        //}

        if (isAttack)
        {
            golemMovement.StopNavMesh();
            golemAnimation.Attack();
            golemAnimation.Walk(false);

            if (!golemMovement.AttackRange())
            {
                isWalk = true;
                isAttack = false;
                golemMovement.ResumeNavMesh();
            }
        }
        else if (isThrow)
        {
            golemAnimation.Throw();
            isThrow = false;
        }
        else
        {
            if (golemMovement.AttackRange())
            {
                isWalk = false;
                isAttack = true;
                golemMovement.StopNavMesh();
                golemAnimation.Walk(false);
                golemAnimation.Idle();
            }
            else if (isWalk)
            {
                golemMovement.ResumeNavMesh();
                golemAnimation.Walk(true);
            }
            else
            {
                isWalk = false;
                golemMovement.StopNavMesh();
                golemAnimation.Walk(false);
                golemAnimation.Idle();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDamage>() != null)
        {
            golemAnimation.Damage();
            golemHpBarUI.GolemDamaged(other.GetComponent<PlayerDamage>().DMG(), other.transform);
        }

        if (other.CompareTag("Player") && isAttack)
        {
            other.GetComponent<PlayerState>().HitPlayer(transform.GetComponent<GolemHpBarUI>().data.atk, true);
        }

    }

    IEnumerator OnceAttack()
    {
        yield return new WaitForSeconds(5f);
        isThrow = true;
        yield return new WaitForSeconds(5f);
        isWalk = true;
    }

    
}
