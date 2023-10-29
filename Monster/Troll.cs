using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(MonsterAnimation))]
public class Troll : MonsterMovement
{
    private MonsterManager monsterManager;
    //몬스터 데이터 값 
    private MonsterData trollData;
    //몬스터애니메이션
    private MonsterAnimation monsterAni;
    //NaveMeshAgent
    private NavMeshAgent navMesh;

    private PlayerStateManager stateManager;
    private Transform playerTr;
    public GameObject DMGText;
    private Animator trollanim;

    protected override float moveSpeed { get; set; }
    protected override bool isMoving { get; set; }
    protected override bool isTrace { get; set; }
    protected override bool isAttack { get; set; }
    protected override bool isDamaged { get; set; }
    protected override bool isDead { get; set; }
    protected override Transform playerTransform { get; set; }
    private Vector3 originPos;

    //몬스터 체력 
    private Slider slider;
    private float trollCurrentHP = 0f;

    //몬스터 공격 쿨타임
    private float trollAttackTimer = 0f;
    //몬스터 공격 범위
    private float attackRange = 1.5f;
    //몬스터 추적 범위
    private float traceRange = 20f;

    //콜라이더에 닿으면 플레이어의 공격력과 방어력을 깎음
    private SphereCollider sphereCollider;
    //private bool debuffapplied = false;
    private int debuffActiveNum = 0;
    private int debuffDEFAmount = 0;
    private int debuffATKAmount = 0;

    private AudioSource audioSource;
    private MonsterAudioClip audioClip;
    int dirty = 0;
    private bool animDone = false;

    private void Awake()
    {
        //상속
        trollanim = GetComponent<Animator>();
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        monsterAni = GetComponent<MonsterAnimation>();
        audioSource = GetComponent<AudioSource>();
        audioClip = GetComponent<MonsterAudioClip>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        sphereCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        
    }

    private void Start()
    {
        //데이터 Null 검사
        if (trollData == null)
        {
            trollData = monsterManager.GetMonsterData(MonsterType.Troll);
        }
        else if (trollData != null)
        {
            Debug.Log("trollData가 이미 존재합니다.");
        }
        trollCurrentHP = trollData.GetMaxHP();
        moveSpeed = monsterManager.GetMonsterData(MonsterType.Troll).GetSPEED();
        navMesh.speed += moveSpeed;
        //플레이어 위치
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        //플레이어 정보 
        stateManager = FindObjectOfType<PlayerStateManager>();
    }

    void Update()
    {
        if (!isDead)
            TrollState();
    }

    public void TrollState()
    {
        float dir = Vector3.Distance(this.transform.position, playerTransform.position);
        isTrace = dir <= traceRange && !isAttack;
        isAttack = dir < attackRange;
        isMoving = navMesh.velocity.magnitude > 0.1f;
        trollAttackTimer += Time.deltaTime;

        //애니메이션 재생 및 상황에 따른 몬스터 속도제어
        if (isTrace)
        {
            MoveToward();
            monsterAni.Run();
            moveSpeed = 20f;    
            navMesh.acceleration = 100f;
            navMesh.angularSpeed = 300f;
            this.transform.rotation.SetLookRotation(playerTransform.transform.position);
        }
        else if (isAttack)
        {
            navMesh.destination = this.transform.position;
            if (trollAttackTimer > 2f && !isTrace)
            {
                //audioSource.Play();
                monsterAni.Attack();
                trollAttackTimer = 0f;
            }
            else
            {
                monsterAni.Idle();
            }
        }
        else if (isMoving && !isTrace)
        {
            moveSpeed = 1f;
            navMesh.acceleration = 1f;
            navMesh.angularSpeed = 100f;
            monsterAni.Walk();
        }
        else
        {
            MoveToOrigin();
            monsterAni.Idle();
        }
    }

    //플레이어를 찾아서 추적
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    
    //제자리로 되돌아감
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    //몬스터가 죽었을 때
    protected override void MonsterDead()
    {
        if (dirty == 0)
        {
            dirty++;
            audioSource.volume = 1f;
            audioSource.clip = audioClip.clip[3];
            audioSource.Play();
        }
        GetComponent<Collider>().enabled = false;
        DisableChildColliders(transform);
        isAttack = false;
        isDead = true;
        trollanim.SetBool("Die", true);
        //Debug.Log
        gameObject.tag = "Untagged";
        navMesh.isStopped = true;
        navMesh.height = 0;
        navMesh.radius = 0;
        monsterAni.Dead();
        stateManager.SetEXP(trollData.GetEXP());
        Destroy(this.gameObject, 20f);
        StartCoroutine(WaitForDeadAnimation());
        
    }


    //몬스터의 sphereCollider에 머물러있으면 플레이어 20%만큼 디버프(중첩)
    private void OnTriggerStay(Collider col)
    {
        if(debuffActiveNum == 0)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (!audioSource.isPlaying && dirty == 0)
                {
                    audioSource.volume = 0.5f;
                    audioSource.clip = audioClip.clip[2];
                    audioSource.Play();
                }
                debuffActiveNum++;
                //debuffapplied = true;
                PlayerDebuff();
            }
        }
    }
    //sphereCollider에 나갔을 때 디버프 제거 
    private void OnTriggerExit(Collider col)
    {
        if(debuffActiveNum > 0)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                //debuffapplied = false;
                PlayerRemoveDebuff();
                debuffActiveNum--;
            }
        }
    }
    //플레이어 디버프 
    private void PlayerDebuff()
    {
        int debuffATK = Mathf.RoundToInt(stateManager.GetATK() * 0.2f);
        int debuffDEF = Mathf.RoundToInt(stateManager.GetDEF() * 0.2f);
        debuffDEFAmount = debuffDEF;
        debuffATKAmount = debuffATK;

        int reducedDEF = stateManager.GetDEF() - debuffDEFAmount;
        int reduceATK = stateManager.GetATK() - debuffATKAmount;

        stateManager.SetDEF(reducedDEF - (9 + (1 * stateManager.StatManager())));
        stateManager.SetATK(reduceATK - (9 + (1 * stateManager.StatManager())));
    }
    //플레이어 디버프 제거
    private void PlayerRemoveDebuff()
    {
        int originalDEF = stateManager.GetDEF() + debuffDEFAmount;
        int originalATK = stateManager.GetATK() + debuffATKAmount;

        stateManager.SetDEF(originalDEF - (9 + (1 * stateManager.StatManager()))); //현재 공격력 값 대입해야됨
        stateManager.SetATK(originalATK - (9 + (1 * stateManager.StatManager()))); //현재 공격력 값 대입해야됨

        debuffDEFAmount = 0;
        debuffATKAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDamage>())
        {
            HitTroll((int)other.transform.GetComponent<PlayerDamage>().DMG(), other.transform);
        }

        if (other.CompareTag("Player") && isAttack)
            other.GetComponent<PlayerState>().HitPlayer(trollData.GetATK(), false);
    }

    private void TrollHpSlider()
    {
        slider.maxValue = (float)trollData.GetMaxHP();
        slider.value = trollCurrentHP;
    }

    private void HitTroll(float damage, Transform _pos)
    {
        if (!audioSource.isPlaying && dirty == 0)
        {
            audioSource.volume = 1f;
            audioSource.clip = audioClip.clip[1];
            audioSource.Play();
        }
        isDamaged = true;
        trollCurrentHP -= (int)(damage * 0.8f);
        GameObject dmgtext = Instantiate(DMGText, _pos.position, playerTransform.rotation);
        dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
        dmgtext.GetComponent<TextMeshPro>().fontSize = 15;
        dmgtext.GetComponent<DamageText>().damage = (int)(damage * 0.8f);
        monsterAni.Damaged();

        TrollHpSlider();
        if (trollCurrentHP <= 0)
        {
            isDead = true;
            MonsterDead();
        }
    }

    public void Attack()
    {
        audioSource.volume = 0.5f;
        audioSource.clip = audioClip.clip[0];
        audioSource.Play();
    }

    IEnumerator WaitForDeadAnimation()
    {
        yield return new WaitForSeconds(2.933f);
        animDone = true;
        if (animDone)
        {
            GameObject meshObject = transform.GetChild(3).gameObject;
            GameObject thisObject = this.gameObject;
            if (meshObject.name == "Mesh_Body")
            {
                meshObject.transform.position = transform.position;
                meshObject.transform.rotation = transform.rotation;
                meshObject.transform.parent = default;
            }
            for (int i = 0; i < thisObject.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void DisableChildColliders(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // 자식 오브젝트에 대해서도 재귀적으로 호출
            DisableChildColliders(child);
        }
    }

}
