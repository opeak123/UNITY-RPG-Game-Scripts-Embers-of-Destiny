using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianAttack : MonoBehaviour
{
    Animator anim;
    PlayerStateManager stateManager;
    PlayerSoundPlay playerSoundPlay;
    WeaponSheath weaponBayonet;
    [SerializeField]
    GameObject fireballPrefab;
    [SerializeField]
    GameObject magicmissilePrefab;
    [SerializeField]
    GameObject meteostrikewaitPrefab;
    [SerializeField]
    GameObject chainlightingPrefab;

    private float fireballCoolTime = 2.0f;
    private float magicmissileCoolTime = 3.0f;
    private float chainlightingCoolTime = 5.0f;
    private float meteostrikeCoolTime = 10.0f;

    private bool isFireBall = false;
    private bool isMagicMissile = false;
    private bool isChainLighting = false;
    private bool setChainLighting = false;
    private bool isMeteo = false;
    private bool isNextAtk = false;

    private enum Skill
    {
        stop,
        fireBall,
        magicMissile,
        chainLighting,
        meteoStrikeWait,
        meteoStrikeFire,
        attack
    }

    void Awake()
    {
        playerSoundPlay = GetComponent<PlayerSoundPlay>();
        stateManager = FindObjectOfType<PlayerStateManager>();
        anim = GetComponent<Animator>();
        weaponBayonet = GetComponent<WeaponSheath>();
    }

    void Update()
    {
        if (GetComponent<PlayerState>().currentMp < stateManager.GetMaxMP())
        {
            GetComponent<PlayerState>().currentMp += 10 * Time.deltaTime;
        }
        else
        {
            GetComponent<PlayerState>().currentMp = stateManager.GetMaxMP();
        }

        if (GetComponent<PlayerState>().currentHp < stateManager.GetHP())
        {
            GetComponent<PlayerState>().currentHp += 3 * Time.deltaTime;
        }
        else
        {
            GetComponent<PlayerState>().currentHp = stateManager.GetHP();
        }

        if (!isFireBall && fireballCoolTime <= 2.0f)
            fireballCoolTime += Time.deltaTime;
        if (!isMagicMissile && magicmissileCoolTime <= 3.0f)
            magicmissileCoolTime += Time.deltaTime;
        if (!isChainLighting && chainlightingCoolTime <= 5.0f)
            chainlightingCoolTime += Time.deltaTime;
        if (!isMeteo && meteostrikeCoolTime <= 10.0f)
            meteostrikeCoolTime += Time.deltaTime;

        if (!weaponBayonet.Bayonet() && anim.GetInteger("Movement") != 2)
        {
            Vector3 center = transform.position;
            float radius = 10.0f;

            Attack();
            FireBall();
            MagicMissile();
            MeteoStrike();

            if (CheckObjectsInRadius(center, radius))
            {
                Debug.Log(setChainLighting);
                ChainLighting();
            }
            else
                Debug.Log("You cannot spell ChainLighting");
        }
        
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isNextAtk)
        {
            isNextAtk = true;
            anim.SetInteger("Skill", (int)Skill.attack);
        }

    }

    private void FireBall()
    {
        if (Input.GetKeyDown(KeyCode.Q) && fireballCoolTime >= 2.0f && stateManager.GetCurrentMp() >= 30)
        {
            GetComponent<PlayerState>().currentMp -= 30;
            stateManager.Attack();
            isFireBall = true;
            fireballCoolTime = 0;
            anim.SetInteger("Skill", (int)Skill.fireBall);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && fireballCoolTime < 2.0f)
            playerSoundPlay.SkillCoolTime();

    }

    public void ShootFireBall()
    {
        isFireBall = false;
        stateManager.AttackEnd();
        Instantiate(fireballPrefab, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
        anim.SetInteger("Skill", (int)Skill.stop);
    }

    private void MagicMissile()
    {
        if (Input.GetKeyDown(KeyCode.E) && magicmissileCoolTime >= 3.0f && stateManager.GetCurrentMp() >= 150)
        {
            GetComponent<PlayerState>().currentMp -= 150;
            stateManager.Attack();
            isMagicMissile = true;
            magicmissileCoolTime = 0;
            anim.SetInteger("Skill", (int)Skill.magicMissile);
        }
        else if (Input.GetKeyDown(KeyCode.E) && magicmissileCoolTime < 3.0f)
            playerSoundPlay.SkillCoolTime();
    }

    public void ShooMagicMissile()
    {
        isMagicMissile = false;
        stateManager.AttackEnd();
        Instantiate(magicmissilePrefab, transform.position + transform.up + transform.forward * 12.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
        anim.SetInteger("Skill", (int)Skill.stop);
    }

    private void ChainLighting()
    {
        if (Input.GetKeyDown(KeyCode.R) && chainlightingCoolTime >= 5.0f && stateManager.GetCurrentMp() >= 300)
        {
            GetComponent<PlayerState>().currentMp -= 300;
            isChainLighting = true;
            chainlightingCoolTime = 0;
            anim.SetInteger("Skill", (int)Skill.chainLighting);
            Instantiate(chainlightingPrefab, transform.position, Quaternion.Euler(-90, transform.rotation.eulerAngles.y, 0));
            playerSoundPlay.PlayerBigSound1_Male();
            Invoke("ChainLightingEnd", 0.6f);
        }
        else if(Input.GetKeyDown(KeyCode.R) && chainlightingCoolTime < 5.0f)
            playerSoundPlay.SkillCoolTime();
    }

    private void ChainLightingEnd()
    {
        isChainLighting = false;
        anim.SetInteger("Skill", (int)Skill.stop);
    }

    private void MeteoStrike()
    {
        if (Input.GetKeyDown(KeyCode.F) && meteostrikeCoolTime >= 10.0f && stateManager.GetCurrentMp() >= 500)
        {
            GetComponent<PlayerState>().currentMp -= 500;
            isMeteo = true;
            stateManager.Attack();
            anim.SetInteger("Skill", (int)Skill.meteoStrikeWait);
            Instantiate(meteostrikewaitPrefab, transform.position + transform.forward * 10.0f, Quaternion.Euler(-90, transform.rotation.eulerAngles.y, 0), transform);
            playerSoundPlay.PlayerBigSound_Male();
        }
        else if(Input.GetKeyDown(KeyCode.F) && meteostrikeCoolTime < 10.0f)
            playerSoundPlay.SkillCoolTime();

        if (Input.GetKeyUp(KeyCode.F) && isMeteo)
        {
            anim.SetInteger("Skill", (int)Skill.meteoStrikeFire);
            isMeteo = false;
        }
    }

    public void ResetOrigin() //초기 상태로(Animation Event)
    {
        stateManager.AttackEnd();
        anim.SetInteger("Skill", (int)Skill.stop);
    }

    public bool IsMeteo()
    {
        return isMeteo;
    }

    public void AttackStart()
    {
        isNextAtk= false;
        stateManager.Attack();
    }

    public void AttackEnd()
    {
        if(isNextAtk)
            anim.SetInteger("Skill", (int)Skill.attack);
        else
            anim.SetInteger("Skill", (int)Skill.stop);
    }

    bool CheckObjectsInRadius(Vector3 center, float radius)
    {
        int countmonster = 0;
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("WOLF") || colliders[i].CompareTag("Enemy"))
                countmonster++;
        }
        if (countmonster > 0)
            setChainLighting = true;
        else
            setChainLighting = false;

        return setChainLighting;
    }
}
