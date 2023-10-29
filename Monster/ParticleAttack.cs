using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{
    private MonsterManager monsterManager;
    private MonsterData monsterData;

    private void Start()
    {
        monsterManager = FindObjectOfType<MonsterManager>();
        if (monsterData == null)
        {
            if(transform.parent.name == "goblin")
                monsterData = monsterManager.GetMonsterData(MonsterType.Goblin);
            else
                monsterData = monsterManager.GetMonsterData(MonsterType.Wolf);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerState>().HitPlayer((float)monsterData.GetATK(), false);
        }
    }
}
