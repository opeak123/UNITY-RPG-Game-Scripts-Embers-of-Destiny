#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemAttack : MonoBehaviour
{
    [HideInInspector]
    public bool isAttack = false;
    [HideInInspector]
    public bool isThrow = false;
    //랜덤공격 애니메이션 값
    private int attackValue;
    //공격할때 플레이어가 맞을 콜라이더
    public Collider[] hitColliders = new Collider[4];   // 0 = hit ground
                                                        // 1 = hit ground splash
                                                         // 2 = hit left hand
                                                         // 3 = hit right hand
    //throw 오브젝트
    public GameObject throwObject;
    //throw할 위치
    public Transform firePos;
    

    //파티클을 넣을 트랜스폼
    public Transform particlePrefabParent;
    //반지름
    public float radius = 15f;
    //시간 측정
    private float timer = 0f;
    //공격범위 표시할 오브젝트
    public GameObject obj;
    public bool isRandomAttacking = false;

    public void GolemAttackUpdate()
    {
        int value = FindObjectOfType<GolemAnimation>().attackValue();
        attackValue = value;
        //Debug.Log("Attack Value: " + attackValue);

        DisableAllColliders();
        

        switch (attackValue)
        {
            case 0: //idle
                isAttack = false;
                isThrow = false;
                break;

            case 1: //right hand
                EnableCollider(3);
                break;

            case 2: //left hand ok 
                EnableCollider(2);
                break;

            case 3: //hand both
                EnableCollider(2);
                EnableCollider(3);
                break;

            case 4: //all collider
                EnableAllColliders();
                break;

            default:
                break;
        }
    }

    private void EnableCollider(int value)
    {
        if (value >= 0 && value < hitColliders.Length)
        {
            hitColliders[value].enabled = true;
        }
    }

    private void EnableAllColliders()
    {
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].enabled = true;
        }
    }

    private void DisableAllColliders()
    {
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].enabled = false;
        }
    }

    public bool RandomThrow()
    {
        float rand = 0.005f;
        float randomValue = Random.value;
        return randomValue < rand;
    }

    public void ThrowRock()
     {
        Collider[] cols = Physics.OverlapSphere(transform.position,10f);
         Collider closestCol = null;

         float closestDist = Mathf.Infinity;

         foreach (Collider col in cols)
         {
             if (col.CompareTag("Rock"))
             {
                 float dist = Vector3.Distance(transform.position, col.transform.position);

                 if (dist < closestDist)
                 {
                     closestDist = dist;
                     closestCol = col;
                 }
             }
         }
        GameObject go = Instantiate(throwObject, firePos.position + 
            transform.forward * 5f, Quaternion.identity);

        Rigidbody rb = go.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Transform playerTransform = GameObject.FindWithTag("Player").transform;
            Vector3 throwDirection = (playerTransform.position - firePos.position).normalized;
            rb.AddForce(throwDirection * 40f, ForceMode.Impulse);
        }
     }

    public void RandomAttack()
    {
        timer += Time.deltaTime;
        if (timer > 2f && isRandomAttacking)
        {
            timer = 0f;
            StartCoroutine(SpawnParticle());
        }
    }

    IEnumerator SpawnParticle()
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir.y = 0f;

        GameObject go = Instantiate(obj, transform.position + randomDir, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);

        ParticleSystem particle = 
            go.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();

        particle.gameObject.SetActive(true);
        particle.Play();

        if (go.activeInHierarchy)
        {
            Destroy(go,1f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
