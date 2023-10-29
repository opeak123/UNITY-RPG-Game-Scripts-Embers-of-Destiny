using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(MonsterAnimation))]
public class Goblin : MonsterMovement
{
    //몬스터매니저 
    private MonsterManager monsterManager;
    //몬스터 데이터 값 
    private MonsterData goblinData;
    //몬스터애니메이션
    private MonsterAnimation monsterAni;
    //NaveMeshAgent
    private NavMeshAgent navMesh;

    private AudioSource audioSource;
    private MonsterAudioClip audioClip;

    private PlayerStateManager stateManager;
    private Transform playerTr;
    public GameObject DMGText;
    //추상 멤버
    protected override bool isAttack { get; set; } = false;
    protected override bool isMoving { get; set; } = true; 
    protected override bool isTrace { get; set; } = false;
    protected override bool isDamaged { get; set; } = false;
    protected override bool isDead { get; set; } = false;
    protected override float moveSpeed { get; set; }
    protected override Transform playerTransform { get; set; }

    //플레이어와의 거리 측정
    private Vector3 originPos;
    private float dirXZ;
    private float dirY;

    //몬스터 공격쿨타임
    private float goblinAttackTimer;

    //몬스터 체력
    private Slider slider;
    private float goblinCurrentHP;

    //몬스터 체력 자동회복
    private float recoveryHpTimer;
    private void Awake()
    {
        //할당
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        audioSource = GetComponent<AudioSource>();
        audioClip = GetComponent<MonsterAudioClip>();
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        
    }

    private void Start()
    {
        //데이터 확인
        if (goblinData == null)
        {
            goblinData = monsterManager.GetMonsterData(MonsterType.Goblin);
        }
        else if(goblinData != null)
        {
            Debug.Log("GolbinData가 이미 존재합니다.");
        }
        //데이터 값 참조 
        goblinCurrentHP = goblinData.GetMaxHP();
        moveSpeed = monsterManager.GetMonsterData(MonsterType.Goblin).GetSPEED();
        navMesh.speed += moveSpeed;
        //플레이어 위치
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        //플레이어 정보
        stateManager = FindObjectOfType<PlayerStateManager>();
        playerTr = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        //print("moving" + isMoving);
        //print("trace" + isTrace);
        //print("attack" + isAttack);
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    slider.value -= 0.2f;
        //    Debug.Log("깎임");
        //}

        if (this.isDead)
            return;
        //몬스터가 플레이어의 로테이션을 바라보게 함
        this.transform.rotation.SetLookRotation(playerTransform.transform.position);
        //플레이어와의 거리계산
        GoblinState();
    }
    //몬스터와 플레이어의 거리 측정해서 길찾기
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    //몬스터가 원래자리로 되돌아감
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }
    
    private void GoblinState()
    {
        //플레이어와 몬스터의 포지션의 X,Z값 계산
        dirXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
            new Vector2(playerTransform.position.x, playerTransform.position.z));

        //플레이어와 몬스터의 포지션의 Y값 계산
        dirY = Mathf.Abs(transform.position.y - playerTransform.position.y);
        //몬스터 공격 쿨타임
        goblinAttackTimer += Time.deltaTime;
        //Bool 변수 : 추적/공격/이동중인지
        isTrace = dirXZ <= 20f && dirY <= 2f && !isAttack;
        isAttack = dirXZ < 1.5f;
        isMoving = navMesh.velocity.magnitude > 0.1f;

        //애니메이션 재생
        switch (isTrace)
        {
            case true:
                MoveToward();
                monsterAni.Run();
                break;

            case false:
                if(isMoving)
                {
                    monsterAni.Walk();
                }
                break;
        }

        //애니메이션 재생
        switch (isAttack)
        {
            case true:
                if (isAttack)
                {
                    navMesh.destination = this.transform.position;
                    if (goblinAttackTimer >= 3 && !isTrace && !isMoving)
                    {
                        monsterAni.Attack();
                        audioSource.volume = 0.5f;
                        audioSource.clip = audioClip.clip[0];
                        audioSource.Play();
                        goblinAttackTimer = 0f;
                    }
                    else
                    {
                        monsterAni.Idle();
                    }
                }
                break;
        }
        //애니메이션 재생
        if (isMoving)
        {
            monsterAni.Walk();
        }
        else if(!isMoving && !isTrace && !isAttack)
        {
            MoveToOrigin();
            monsterAni.Idle();
            //RecoveryHP();
        }
    }

    //게임을 시작했을때 슬라이더에 최대체력 대입
    private void GoblinHpSlider()
    {
        slider.maxValue = (float)goblinData.GetMaxHP();
        slider.value = goblinCurrentHP;
    }

    //고블린에게 데미지를 입혔을 때
    private void HitGoblin(float damage, Transform _pos)
    {
        if(!audioSource.isPlaying && dirty == 0)
        {
            audioSource.volume = 1f;
            audioSource.clip = audioClip.clip[1];
            audioSource.Play();
        }
        goblinCurrentHP -= damage;
        goblinCurrentHP -= (int)(damage * 0.8f);
        monsterAni.Damaged();
        GameObject dmgtext = Instantiate(DMGText, _pos.position, playerTr.rotation);
        dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
        dmgtext.GetComponent<TextMeshPro>().fontSize = 15;
        dmgtext.GetComponent<DamageText>().damage = (int)(damage * 0.8f);

        GoblinHpSlider();
        if (goblinCurrentHP <= 0)
        {
            isDead = true;
            MonsterDead();
        }
    }
    int dirty = 0;
    //고블린이 죽었을때 State
    protected override void MonsterDead()
    {
        if(dirty == 0)
        {
            dirty++;
            audioSource.volume = 1f;
            audioSource.clip = audioClip.clip[2];
            audioSource.Play();
        }
        DisableChildColliders(transform);
        transform.GetComponent<Collider>().enabled = false;
        gameObject.tag = "Untagged";
        navMesh.isStopped = true;
        navMesh.height = 0;
        navMesh.radius = 0;
        monsterAni.Dead();
        stateManager.SetEXP(goblinData.GetEXP());
        Destroy(this.gameObject, 20f);

        GameObject thisObject = this.gameObject;
        for (int i = 0; i < thisObject.transform.childCount; i++)
        {
            GameObject meshObject = transform.GetChild(3).gameObject;
            if (meshObject.name == "Mesh_Body")
            {
                meshObject.transform.parent = default;
            }
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //전투상태가 아니라면 체력 자동회복
    private void RecoveryHP()
    {
        recoveryHpTimer += Time.deltaTime;
        //float amount = goblinData.GetMaxHP() / 99f;
        float amount = goblinData.GetMaxHP() * 0.0002f;
        if (recoveryHpTimer > 0.5f)
        {
            //slider.value += amount;
            slider.value = Mathf.Min(slider.value + amount,goblinData.GetMaxHP());
            recoveryHpTimer = 0f;
        }
    }


    //private void RecoveryHP()
    //{
    //    recoveryHpTimer += Time.deltaTime;
    //    //float amount = goblinData.GetMaxHP() / 99f;
    //    float amount = goblinData.GetMaxHP() * 0.0002f;
    //    if (recoveryHpTimer > 0.5f)
    //    {
    //        //slider.value += amount;
    //        slider.value = Mathf.Min(slider.value + amount, goblinData.GetMaxHP());
    //        recoveryHpTimer = 0f;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDamage>())
        {
            HitGoblin(other.transform.GetComponent<PlayerDamage>().DMG(), other.transform);
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