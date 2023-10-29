using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GolemMovement : MonoBehaviour
{
    public NavMeshAgent navMesh;
    private Transform playerTransform;
    private bool IsWalk;

    public bool isWalk => IsWalk;

    //private bool isIdle = false;
    //private bool isWalk;
    //private bool isAttack;
    //private bool isThrow;


    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(GolemStartWalk());
    }

    //public void GolemMovementUpdate()
    //{
    //    float dir = Vector3.Distance(transform.position, playerTransform.position);
    //    isWalk = dir >= 5f && !isIdle && !isAttack && !isThrow;
    //    isAttack = dir < 5f && !isIdle && !isWalk && !isThrow;
    //    isThrow  = dir >= 5f && isIdle && !IsWalk && !isAttack;

    //    if(isWalk)
    //    {
    //        navMesh.SetDestination(playerTransform.position);
    //    }
    //    else if(isAttack)
    //    {
    //        navMesh.SetDestination(transform.position);
    //    }
    //    else if(isThrow)
    //    {
    //        navMesh.SetDestination(transform.position);
    //    }
    //    else
    //    {
    //        navMesh.SetDestination(transform.position);
    //    }

    //}



    public void CalculateDistance()
    {
        float traceDir = Vector3.Distance(transform.position, playerTransform.position);
        IsWalk = traceDir >= 5f;
        MoveToPlayer();
    }
    public bool AttackRange()
    {
        float attackDir = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 checkVector = (transform.position - playerTransform.position);
        float angle = Vector3.Angle(Vector3.forward, checkVector);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-90f / 2f, transform.up) * transform.forward * 10f, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(90f / 2f, transform.up) * transform.forward * 10f, Color.yellow);

        if (attackDir < 5f)
        {
            //navMesh.speed = 0;
            navMesh.velocity = Vector3.zero;
            navMesh.acceleration = 0;
            navMesh.angularSpeed = 0;
            return true;
        }
        else
        {
            //navMesh.speed = 2;
            navMesh.acceleration = 8;
            navMesh.angularSpeed = 120;
            return false;
        }
    }

    public void MoveToPlayer()
    {
        if (!IsWalk && AttackRange())
        {
            IsWalk = false;
            navMesh.SetDestination(transform.position);
            StopNavMesh();
        }
        else if (IsWalk)
        {
            ResumeNavMesh();
            navMesh.SetDestination(playerTransform.position);
        }
        else
        {
            StopNavMesh();
            if (!AttackRange())
            {
                navMesh.SetDestination(transform.position);
            }
        }
    }
    public void StopNavMesh()
    {
        navMesh.isStopped = true;
    }

    public void ResumeNavMesh()
    {
        navMesh.isStopped = false;
    }

    IEnumerator GolemStartWalk()
    {
        yield return new WaitForSeconds(4.5f);

        GolemController gm = GetComponent<GolemController>();
        gm.isWalk = true;
    }
}
