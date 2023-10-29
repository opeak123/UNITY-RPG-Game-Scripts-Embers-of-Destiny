using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySwordSlashEffect : MonoBehaviour
{
    WarriorAttack warriorAtk;

    private void Start()
    {
        warriorAtk = FindObjectOfType<WarriorAttack>();
        Destroy(this.gameObject, 1.0f);
    }

    private void Update()
    {
        if (!warriorAtk.IsMultiSlash())
            Destroy(this.gameObject, 0.4f);
    }
}
