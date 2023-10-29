using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    int dirty;
    [SerializeField]
    GolemHpBarUI golemHpBarUI;
    [SerializeField]
    ReaperHpBarUI reaperHpBarUI;

    private void Start()
    {
        if (FindObjectOfType<GolemHpBarUI>() != null)
            golemHpBarUI = FindObjectOfType<GolemHpBarUI>();
        if (FindObjectOfType<ReaperHpBarUI>() != null)
            reaperHpBarUI = FindObjectOfType<ReaperHpBarUI>();
    }

    private void OnParticleCollision(GameObject col)
    {
        if(col.gameObject.CompareTag("Player") && dirty == 0)
        {
            dirty++;
            print("hit");
            if(this.gameObject.name == "PoisonParticle")
            {
                print("Áßµ¶");
                col.GetComponent<PlayerState>().Poision();
            }
            else if(this.gameObject.CompareTag("Nova"))
            {
                print("hit nova");
                col.GetComponent<PlayerState>().HitPlayer(reaperHpBarUI.reaperData.atk * 1.5f, true);
            }
            else if(this.gameObject.name == "ShadowSlash")
            {
                //col.GetComponent<PlayerState>().HitPlayer(reaperHpBarUI.reaperData.atk);
            }
            else
            {
                col.GetComponent<PlayerState>().Stun();
                col.GetComponent<PlayerState>().HitPlayer(golemHpBarUI.data.atk, true);
            }

        }
        dirty = 0;
    }
}
