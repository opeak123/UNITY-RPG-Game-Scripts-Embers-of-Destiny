using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerState : MonoBehaviour
{
    Animator anim;
    PlayerStateManager stateManager;
    PlayerUI _playerUI;
    Rigidbody rig;
    ParticleSystem burnParticle;
    CameraShake cameraShake;
    public GameObject StunEffect;
    public GameObject PoisionEffect;
    public GameObject DMGText;
    private DeadScreen dead;

    [Header("NAME")]
    public string CharacterName = "Nameless";

    private enum State
    {
        none,
        hit_f,
        hit_b,
        large_hit,
        die,
        stun,
        stunEnd_Equiped,
        stunEnd_Unarmed
    }

    private float burnTime = 0;
    private float stunTime = 0;
    private float poisionTime = 0;

    private float MaxHp = 0;
    private float MaxMp = 0;
    public float currentHp = 0;
    public float currentMp = 0;
    private bool isWarrior = false;

    void Awake()
    {
        burnParticle = GameObject.Find("FireFlame").GetComponent<ParticleSystem>();
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        stateManager = FindObjectOfType<PlayerStateManager>();
        stateManager.FindStateManager();
        _playerUI = FindObjectOfType<PlayerUI>();
        if (gameObject.GetComponent<WarriorAttack>() != null)
            isWarrior = true;
    }

    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        cameraShake = FindObjectOfType<CameraShake>();
        dead = FindObjectOfType<DeadScreen>();
        SettingHpMp();
    }

    public void SettingHpMp()
    {
        MaxHp = stateManager.GetHP();
        MaxMp = stateManager.GetMaxMP();
        currentHp = MaxHp;
        currentMp = MaxMp;
    }

    private void Update()
    {
        stateManager.SetCurrentMp((int)currentMp);
        if (!GetComponent<PlayerMovement>().enabled)
        {
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", 0);
        }

        //화상 쿨타임
        if (burnTime >= 0)
            burnTime -= Time.deltaTime;
        if (burnTime <= 0 && stateManager.GetBURN())
        {
            stateManager.SetBURN(false);
            burnParticle.Stop();
            if (!stateManager.GetBURN() && !stateManager.GetPOISION())
                StopAllCoroutines();
        }
        //스턴 쿨타임
        if (stunTime >= 0)
            stunTime -= Time.deltaTime;
        if (stunTime <= 0 && stateManager.GetSTUN())
        {
            StunEffect.SetActive(false);
            stateManager.SetSTUN(false);
            if (anim.GetInteger("BayonetState") == 3)
                anim.SetInteger("DMGState", (int)State.stunEnd_Equiped);
            else if(anim.GetInteger("BayonetState") == 4 || anim.GetInteger("BayonetState") == 0)
                anim.SetInteger("DMGState", (int)State.stunEnd_Unarmed);
        }
        //중독 쿨타임
        if (poisionTime >= 0)
            poisionTime -= Time.deltaTime;
        if (poisionTime <= 0 && stateManager.GetPOISION())
        {
            stateManager.SetPOISION(false);
            PoisionEffect.SetActive(false);
            if (!stateManager.GetBURN() && !stateManager.GetPOISION())
                StopAllCoroutines();
        }

    
    }

    IEnumerator StatusEffect()
    {
        while (true)
        {
            if (stateManager.GetBURN())
            {
                currentHp -= stateManager.GetHP() * 0.01f;
                GameObject dmgtext = Instantiate(DMGText, transform.position + transform.up * 1.8f, transform.rotation,transform);
                dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 165f / 255f, 0);
                dmgtext.GetComponent<DamageText>().damage = MaxHp * 0.01f;
            }

            if (stateManager.GetPOISION())
            {
                currentHp -= stateManager.GetHP() * 0.01f;
                GameObject dmgtext = Instantiate(DMGText, transform.position + transform.up * 1.8f, transform.rotation,transform);
                dmgtext.GetComponent<TextMeshPro>().color = new Color(148f / 255f, 0, 211f / 255f);
                dmgtext.GetComponent<DamageText>().damage = MaxHp * 0.01f;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void Burn()
    {
        if(!stateManager.GetBURN())
            StartCoroutine(StatusEffect());
        stateManager.SetBURN(true);
        burnTime = 10.0f;
        burnParticle.Play();
    }

    public void Stun()
    {
        StunEffect.SetActive(true);
        stateManager.SetSTUN(true);
        stunTime = 3.0f;
        anim.SetTrigger("Stun");
        anim.SetInteger("DMGState", (int)State.stun);
    }

    public void Poision()
    {
        if (!stateManager.GetPOISION())
            StartCoroutine(StatusEffect());
        stateManager.SetPOISION(true);
        poisionTime = 10.0f;
        PoisionEffect.SetActive(true);
        PoisionEffect.GetComponent<ParticleSystem>().Play();
    }

    void Die()
    {
        anim.SetInteger("DIE", (int)State.die);
        StunEffect.SetActive(false);
        burnParticle.Stop();
        PoisionEffect.SetActive(false);
        stateManager.SetDIE(true);
        PoisionEffect.SetActive(false);
        anim.SetTrigger("Die");
        rig.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(dead.FadeOutStart());
        dead.Invoke("RecallActive", 3f);
    }

    //포션 사용
    public void UsePortion(int value, int type)
    {
        switch (type)
        {
            case -1: //포션이 아닌 아이템 값입력
                Debug.Log("잘못된 아이템 사용");
                break;
            case 0: //HP포션
                currentHp += value;
                //최대 HP이상이면 최대 HP로 변경

                if (currentHp > stateManager.GetHP())
                    currentHp = stateManager.GetHP();
                break;
            case 1: //MP포션
                currentMp += value;
                //최대 MP이상이면 최대 MP로 변경
                if (currentMp > stateManager.GetMaxMP())
                    currentMp = stateManager.GetMaxMP();
                break;
        }
    }

    private bool isHit = false;

    public void HitPlayer(float _damage, bool _nuckback)
    {
        if (!stateManager.GetDASH() && !isHit)
        {
            isHit = true;
            currentHp -= (_damage - stateManager.GetDEF());
            GameObject dmgtext = Instantiate(DMGText, transform.position + transform.up * 1.8f, transform.rotation, transform);
            dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
            dmgtext.GetComponent<TextMeshPro>().fontSize = 5;
            dmgtext.GetComponent<DamageText>().damage = (_damage - stateManager.GetDEF());
            if (currentHp > 0)
            {
                cameraShake.HitShake();
                if (_nuckback)
                {
                    stateManager.Attack();
                    anim.applyRootMotion = false;
                    rig.AddForce(-transform.forward * 10, ForceMode.Impulse);
                    anim.SetInteger("DMGState", (int)State.hit_f);
                }
            }
        }
        Invoke("DMG_FIN", 1.0f);
        if (currentHp <= 0)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "NovaFire")
        {
            anim.applyRootMotion = false;
            rig.AddForce(-transform.forward * 50, ForceMode.Impulse);
            anim.SetInteger("DMGState", (int)State.hit_f);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            if (isWarrior)
            {
                if (Vector3.Dot(transform.forward, (col.transform.position - transform.position)) >= 0)
                {
                    stateManager.Attack();
                    anim.applyRootMotion = false;
                    rig.AddForce(-transform.forward * 10, ForceMode.Impulse);
                    anim.SetInteger("DMGState", (int)State.hit_f);
                    Invoke("DMG_FIN", 1.0f);
                }
                else
                {
                    stateManager.Attack();
                    anim.applyRootMotion = false;
                    rig.AddForce(transform.forward * 10, ForceMode.Impulse);
                    anim.SetInteger("DMGState", (int)State.hit_b);
                    Invoke("DMG_FIN", 1.0f);
                }

            }
            
        }
    }

    public void HITPLAYER()
    {
        anim.SetInteger("DMGState", (int)State.none);
    }

    private void DMG_FIN()
    {
        isHit = false;
        stateManager.AttackEnd();
        anim.applyRootMotion = true;
    }

}
