using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CerberusDMG : MonoBehaviour
{
    [SerializeField]
    GameObject particle;
    public CerberusData data;

    bool firsCheck = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !transform.CompareTag("Enemy"))
        {
            if (firsCheck == false)
            {
                // 충돌 지점에 파티클 시스템을 생성하고 재생합니다.
                SpawnParticleEffect(other.transform.position + other.transform.up);
                other.GetComponent<PlayerState>().HitPlayer(data.atk, true);
                GetComponent<CerberusAnimationEvent>().BiteEnd();
                GetComponent<BoxCollider>().enabled = false;
                firsCheck = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !transform.CompareTag("Enemy"))
        {
            firsCheck = false;
        }
    }
    void SpawnParticleEffect(Vector3 _position)
    {
        // 파티클 시스템 재생
        Instantiate(particle, _position, Quaternion.identity);
    }
}
