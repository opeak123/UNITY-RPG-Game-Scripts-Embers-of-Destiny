using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusMovement : MonoBehaviour
{
    Transform playerTr;
    Rigidbody rig;
    Animator anim;
    BoxCollider jumpAtkCol;
    SetSecondPaze setPaze;

    private int dist = 0;
    private float moveSpeed = 5.0f;
    private float lookSpeed = 5.0f;
    private int randomPatern;
    private bool isAttack = false;

    private enum State
    {
        Idle,
        Walk,
        Run,
        Bite,
        JumpAttack,
        Breathe,
        BackWalk,
        Die
    }

    private void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        setPaze = transform.GetChild(3).GetComponent<SetSecondPaze>();
        jumpAtkCol = GetComponent<BoxCollider>();
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine(Ran_num());
    }

    Vector3 looklerp;
    private void FixedUpdate()
    {
        if (!setPaze.SecondPazing() && !GetComponent<CerberusHp>().isGetHit())
        {
            Patern();
            Dist();
            if (!isAttack)
            {
                Vector3 dir = new Vector3(playerTr.position.x, 0, playerTr.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
                dir.y = 0;
                dir.Normalize();

                looklerp = Vector3.Lerp(looklerp, dir, Time.fixedDeltaTime * lookSpeed);

                transform.LookAt(transform.position + looklerp);
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * lookSpeed);
            }
        }
        else
            return;
    }

    public void StopMovement()
    {
        StopAllCoroutines();
        anim.SetInteger("State", (int)State.Idle);
        anim.SetTrigger("SecondPaze");
        GetComponent<CerberusAnimationEvent>().BreatheEnd();
        GetComponent<CerberusAnimationEvent>().ChargeEnd();
    }

    public void StartMovement()
    {
        StartCoroutine(Ran_num());
    }

    private void Dist()
    {
        Vector3 dir = playerTr.position - transform.position;
        if (dir.magnitude >= 25.0f)
        {
            dist = 1;
        }
        else if (dir.magnitude >= 15.0f && dir.magnitude < 25.0f)
        {
            dist = 2;
        }
        else if (dir.magnitude < 15.0f && dir.magnitude >= 10.0f)
        {
            dist = 3;
        }
        else if (dir.magnitude < 10.0f && dir.magnitude >= 9.0f)
            dist = 4;
        else
            dist = 5;
    }

    private void ToPlayer(int num)
    {
        Vector3 dir = playerTr.position - transform.position;

        if (num == (int)State.Walk)
            moveSpeed = 5.0f;
        else if (num == (int)State.Run)
            moveSpeed = 10.0f;
        else if (num == (int)State.BackWalk)
            moveSpeed = -5.0f;

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * lookSpeed);
        rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        anim.SetInteger("State", num);
    }

    IEnumerator Ran_num()
    {
        while (true)
        {
            if (dist == 1)
            {
                randomPatern = 0;
            }
            else if (dist == 2)
            {
                randomPatern = Random.Range(1, 3);
            }
            else if (dist == 3)
            {
                randomPatern = 3;
            }
            else if (dist == 4)
            {
                int _random = Random.Range(0, 3);
                if (_random == 0)
                    randomPatern = 4;
                else if (_random == 1)
                    randomPatern = 1;
                else
                    randomPatern = 2;
            }
            else
            {
                randomPatern = 5;
            }

            if (randomPatern == 1)
            {
                yield return new WaitForSeconds(0.6f); //1.2
                randomPatern = 6;
                jumpAtkCol.enabled = false;
                yield return new WaitForSeconds(2.0f);
            }
            else if (randomPatern == 2)
            {
                yield return new WaitForSeconds(2.8f); //2.8
                randomPatern = 6;
                yield return new WaitForSeconds(2.0f);
            }
            else if (randomPatern == 4)
            {
                yield return new WaitForSeconds(1.5f); //1.5
                randomPatern = 6;
                yield return new WaitForSeconds(2.0f);
            }
            else
                yield return new WaitForSeconds(0.1f);

        }
        
    }

    void Patern()
    {
        switch (randomPatern)
        {
            case 0:
                isAttack = false;
                ToPlayer((int)State.Run);
                break;
            case 1:
                isAttack = true;
                jumpAtkCol.enabled = true;
                anim.SetInteger("State", (int)State.JumpAttack);
                break;
            case 2:
                isAttack = true;
                anim.SetInteger("State", (int)State.Breathe);
                break;
            case 3:
                isAttack = false;
                anim.SetInteger("State", (int)State.Walk);
                ToPlayer((int)State.Walk);
                break;
            case 4:
                isAttack = true;
                anim.SetInteger("State", (int)State.Bite);
                break;
            case 5:
                isAttack = false;
                ToPlayer((int)State.BackWalk);
                break;
            case 6:
                isAttack = false;
                anim.SetInteger("State", (int)State.Idle);
                break;
        }
    }
}
