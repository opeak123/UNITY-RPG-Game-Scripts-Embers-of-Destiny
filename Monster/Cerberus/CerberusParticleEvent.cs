using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusParticleEvent : MonoBehaviour
{
    PlayerStateManager playerstateManager;
    public CerberusData data;

    private void Awake()
    {
        playerstateManager = FindObjectOfType<PlayerStateManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerState>().Burn();
            other.GetComponent<PlayerState>().HitPlayer(data.atk * 0.1f, false);
        }
    }
}
