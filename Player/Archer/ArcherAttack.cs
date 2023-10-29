using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    Animator anim;
    WeaponSheath weaponBayonet;
    GameObject energyarrow;
    GameObject multiarrow;
    Canvas arrowcanvas;
    [SerializeField]
    ParticleSystem arrowstormParticle;

    [SerializeField]
    Animator[] bowAnim; //장착된 활의 애님
    [SerializeField]
    Transform firePos; //화살 발사위치
    [SerializeField]
    GameObject arrowPrefab; //화살 프리팹
    PlayerStateManager stateManager;
    [SerializeField]
    GameObject energyarrowshotPrefab;
    [SerializeField]
    GameObject multishot;
    [SerializeField]
    GameObject arrowrainPrefab;
    [SerializeField]
    GameObject arrowstormPrefab;

    public float atkSpeed; //공격속도

    private float energyshotCoolTime = 0f;
    private float multiarrowshotCoolTime = 0f;
    private float arrowrainCoolTime = 0f;
    private float arrowstorming = 5.0f;
    private float arrowstormCoolTime = 0f;

    private int bowNum;

    private bool isFire = false;
    private bool isNextAtk = false;
    private bool isEnergyShot = false;
    private bool isMultiArrowShot = false;
    private bool isArrowRain = false;
    private bool isArrowStorming = false;

    private enum Skill
    {
        stop,
        energyarrowshot,
        multishot,
        arrowrain,
        arrowstorm,
        attack,
        arrowstormend
    }

    void Awake()
    {
        arrowcanvas = transform.GetChild(10).gameObject.GetComponent<Canvas>();
        anim = GetComponent<Animator>();
        weaponBayonet = GetComponent<WeaponSheath>();
        firePos = GameObject.FindWithTag("Arrow").transform;
        energyarrow = firePos.GetChild(0).gameObject;
        multiarrow = firePos.GetChild(2).gameObject;
        arrowstormParticle = transform.GetChild(11).GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        atkSpeed = stateManager.GetATKSPEED();
        for (int i = 0; i < bowAnim.Length; i++)
        {
            bowAnim[i] = GameObject.FindWithTag("Weapon_Hand").transform.GetChild(i).GetComponent<Animator>();
        }

        bowNum = GetComponent<WeaponSheath>().item_num;

    }

    void Update()
    {
        
        if (GetComponent<PlayerState>().currentMp < stateManager.GetMaxMP())
        {
            GetComponent<PlayerState>().currentMp += 5 * Time.deltaTime;
        }
        else
        {
            GetComponent<PlayerState>().currentMp = stateManager.GetMaxMP();
        }

        if (GetComponent<PlayerState>().currentHp < stateManager.GetHP())
        {
            GetComponent<PlayerState>().currentHp += 5 * Time.deltaTime;
        }
        else
        {
            GetComponent<PlayerState>().currentHp = stateManager.GetHP();
        }


        if (!isEnergyShot && energyshotCoolTime <= 2.0f)
            energyshotCoolTime -= Time.deltaTime;
        if (!isMultiArrowShot && multiarrowshotCoolTime <= 3.0f)
            multiarrowshotCoolTime -= Time.deltaTime;
        if (!isArrowRain && arrowrainCoolTime <= 10.0f)
            arrowrainCoolTime -= Time.deltaTime;
        if (!isArrowStorming && arrowstormCoolTime <= 10.0f)
            arrowstormCoolTime -= Time.deltaTime;

        bowNum = GetComponent<WeaponSheath>().item_num;
        if (!weaponBayonet.Bayonet())
        {
            Attack();
            EnergyArrowShot();
            MultiShotArrow();
            ArrowRain();
            ArrowStorm();
            arrowcanvas.enabled = true;
        }
        else
            arrowcanvas.enabled = false;
    }

    #region Skill&&Attack
    //공격
    void Attack()
    {
        if (!isFire)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                stateManager.Attack();
                isFire = true;
                isNextAtk = true;
                anim.SetInteger("Skill", (int)Skill.attack);
                //GetComponent<PlayerSoundPlay>().playersound_female_attack();
            }
        }
    }

    private void EnergyArrowShot()
    {
        if (Input.GetKeyDown(KeyCode.Q) && energyshotCoolTime <= 0 && stateManager.GetCurrentMp() >= 50)
        {
            GetComponent<PlayerState>().currentMp -= 50;
            Debug.LogError(GetComponent<PlayerState>().currentMp);
            energyshotCoolTime = 2.0f;
            Finish();
            isEnergyShot = true;
            anim.SetInteger("Skill", (int)Skill.energyarrowshot);
        }
        else if(Input.GetKeyDown(KeyCode.Q) && energyshotCoolTime > 0 && !isEnergyShot)
            GetComponent<PlayerSoundPlay>().playersound_female_skillcooltime();
    }

    public void EnergyArrow_Shot()
    {
        isEnergyShot = false;
        energyarrow.SetActive(false);
        Instantiate(energyarrowshotPrefab, firePos.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
    }

    public void ShowEnergyArrow()
    {
        energyarrow.SetActive(true);
    }

    private void MultiShotArrow()
    {

        if (Input.GetKeyDown(KeyCode.E) && multiarrowshotCoolTime <= 0f && stateManager.GetCurrentMp() >= 75)
        {
            GetComponent<PlayerState>().currentMp -= 75;
            isMultiArrowShot = true;
            multiarrowshotCoolTime = 3.0f;
            anim.SetInteger("Skill", (int)Skill.multishot);
        }
        else if(Input.GetKeyDown(KeyCode.E) && multiarrowshotCoolTime > 0f)
            GetComponent<PlayerSoundPlay>().playersound_female_skillcooltime();
    }

    public void ShowMultiArrow()
    {
        multiarrow.SetActive(true);
    }

    public void MultiShot()
    {
        GetComponent<PlayerSoundPlay>().ArcherMultiShot();
        isMultiArrowShot = false;
        multiarrow.SetActive(false);
        Instantiate(multishot, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
        Instantiate(multishot, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y - 15, 0));
        Instantiate(multishot, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y - 30, 0));
        Instantiate(multishot, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 15, 0));
        Instantiate(multishot, transform.position + transform.up + transform.forward * 2.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 30, 0));
    }

    private void ArrowRain()
    {
        if (Input.GetKeyDown(KeyCode.R) && arrowrainCoolTime <= 0 && stateManager.GetCurrentMp() >= 120)
        {
            GetComponent<PlayerState>().currentMp -= 120;
            arrowrainCoolTime = 10.0f;
            Finish();
            isArrowRain = true;
            anim.SetInteger("Skill", (int)Skill.arrowrain);
            Instantiate(arrowrainPrefab, transform.position + transform.forward * 3.0f, Quaternion.Euler(-90, transform.rotation.eulerAngles.y, 0));
        }
        else if(Input.GetKeyDown(KeyCode.R) && arrowrainCoolTime > 0)
            GetComponent<PlayerSoundPlay>().playersound_female_skillcooltime();

        if (Input.GetKeyUp(KeyCode.R) && isArrowRain)
        {
            isArrowRain = false;
            GetComponent<PlayerSoundPlay>().ArrowVolleyFire();
            anim.SetInteger("Skill", (int)Skill.stop);
        }
    }

    public bool IsArrowRain()
    {
        return isArrowRain;
    }
    private void ArrowStorm()
    {
        if (isArrowStorming && arrowstorming <= 5.0f)
            arrowstorming += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.F) && arrowstormCoolTime <= 0f && stateManager.GetCurrentMp() >= 180)
        {
            GetComponent<PlayerState>().currentMp -= 180;
            arrowstormCoolTime = 10.0f;
            Finish();
            GetComponent<PlayerSoundPlay>().ArcherCharge();
            arrowstorming = 0f;
            isArrowStorming = true;
            anim.SetInteger("Skill", (int)Skill.arrowstorm);
            arrowstormParticle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.F) && arrowstormCoolTime > 0f)
        {
            GetComponent<PlayerSoundPlay>().playersound_female_skillcooltime();
        }

        if (arrowstorming >= 5.0f)
        {
            arrowstorming = 0;
            GetComponent<PlayerSoundPlay>().ArcherChargeEnd();
            CancelInvoke("ArrowShot");
            isArrowStorming = false;
            anim.SetInteger("Skill", (int)Skill.stop);
            firePos.GetChild(1).gameObject.SetActive(false);
        }
        
        

        if (Input.GetKeyUp(KeyCode.F) && isArrowStorming)
        {
            isArrowStorming = false;
            arrowstormCoolTime = 10.0f;
            GetComponent<PlayerSoundPlay>().ArcherChargeEnd();
            CancelInvoke("ArrowShot");
            arrowstorming = 0;
            anim.SetInteger("Skill", (int)Skill.stop);
            firePos.GetChild(1).gameObject.SetActive(false);
            arrowstormParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public bool IsArrowStorm()
    {
        return isArrowStorming;
    }

    public void ArrowStormShot()
    {
        firePos.GetChild(1).gameObject.SetActive(true);
        InvokeRepeating("ArrowShot", 0.2f, 0.2f);
    }

    private void ArrowShot()
    {
        GetComponent<PlayerSoundPlay>().ArrowOneShot();
        Instantiate(arrowstormPrefab, transform.position + transform.right * 0.1f + transform.up + transform.forward * 5.0f, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
    }
    #endregion

    //활의 애님(Animation Event)
    public void Bow()
    {
        bowAnim[bowNum].speed = 2.7f * atkSpeed;
        bowAnim[bowNum].SetTrigger("ShotReady");
    }

    //발사(Animation Event)
    public void Fire()
    {
        Instantiate(arrowPrefab, firePos.position, transform.rotation, firePos);
    }

    //발사 종료(Animation Event)
    public void Finish()
    {
        isFire = false;
        anim.speed = 1.0f;
    }

    //공격 시작(Animation Event)
    public void AttackStart()
    {
        isNextAtk = false;
        anim.speed = atkSpeed;
        stateManager.Attack();
    }

    //공격 종료(Animation Event)
    public void AttackEnd()
    {
        stateManager.SetDASH(false);
        anim.speed = 1.0f;
        stateManager.AttackEnd();
        if (isNextAtk)
            anim.SetInteger("Skill", (int)Skill.attack);
        else
            anim.SetInteger("Skill", (int)Skill.stop);
    }
}
