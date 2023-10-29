using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    GolemHpBarUI golemHpBarUI;

    private void Start()
    {
        if (FindObjectOfType<GolemHpBarUI>() != null)
            golemHpBarUI = FindObjectOfType<GolemHpBarUI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit by rock");
            GetComponent<AudioSource>().Play();
            collision.transform.GetComponent<PlayerState>().Stun();
            collision.transform.GetComponent<PlayerState>().HitPlayer(golemHpBarUI.data.atk, true);
            Destroy(this.gameObject, 3f);
        }
    }
}
