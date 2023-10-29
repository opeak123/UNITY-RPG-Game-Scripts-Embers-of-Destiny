using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    Rigidbody rig;
    PlayerStateManager stateManager;

    private enum Movement
    {
        Walk,
        Run,
        Jump
    }

    Vector3 dir;

    float h;
    float v;

    float playerSpeed; //플레이어 속도
    float tempPlayerSpeed;

    float jumpPower = 6.0f; //점프 파워
    [SerializeField]
    private float dumblingCoolTime = 1.0f;
    private float dashTimer = 1.0f;
    private float dashPower = 15.0f;
    private float dumblingTimer = 1.0f;

    private bool isJump = false;
    private bool isJumping = false;
    private bool isRun = false;
    private bool isWarrior = false;
    private bool isArcher = false;
    void Awake()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        if (GetComponent<WarriorAttack>() != null)
            isWarrior = true;
        if (GetComponent<ArcherAttack>() != null)
            isArcher = true;
    }

    private void Start()
    {
        tempPlayerSpeed = stateManager.GetMOVESPEED();
        playerSpeed = tempPlayerSpeed;
        anim.speed = 1;
    }

    void Update()
    {
        if (!stateManager.GetDIE())
        {
            tempPlayerSpeed = stateManager.GetMOVESPEED();
            if (anim.GetInteger("Skill") == 0)
            {
                Jump();
                Run();
            }

            if(dashTimer <= dumblingCoolTime)
                dashTimer += Time.deltaTime;
            if (dumblingTimer <= 1.0f)
                dumblingTimer += Time.deltaTime;

            if (!isJump)
            {
                if (isWarrior )
                    Dash();
                if (isArcher)
                    Dumbling();
                if (!isWarrior && !isArcher)
                    Teleport();
            }
        }
    }

    private void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        anim.SetFloat("X", v);
        anim.SetFloat("Y", h);

        if (!stateManager.GetDIE() && anim.GetInteger("DMGState") != 5)
        {
            if (!stateManager.IsAttack() && anim.GetInteger("BayonetState") != 1)
                PlayerInput();
        }
    }


    #region Player_Input/Motion(Run, Jump, Dash/Dumbling/Teleport)
    //플레이어 인풋
    void PlayerInput()
    {
        if (!stateManager.GetSTUN())
        {
            dir = transform.right * h;
            dir += transform.forward * v;
            dir.Normalize();
            rig.MovePosition(transform.position + dir * playerSpeed * Time.fixedDeltaTime);
        }
        
    }

    void Jumping()
    {
        isJumping = !isJumping;
    }

    //점프
    void Jump()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP])) //점프키를 누르면
        {
            if (!isJump)
            {
                isJump = true;
                Invoke("Jumping", 0.5f);
                anim.SetTrigger("Jump");
                anim.SetInteger("Movement", (int)Movement.Jump);
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                if (this.name == "Warrior" || this.name == "Mage")
                    GetComponent<PlayerSoundPlay>().PlayerShortSound_Male();
                else
                    GetComponent<PlayerSoundPlay>().playerJumpSound_Female();
            }
            
        }

        RaycastHit hit;
        float raycastDistance = 1.3f;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, raycastDistance))
        {
            if (isJumping) //착지 순간
            {
                Debug.Log("JumpEnd");
                isJump = false;
                isJumping = false;
                if (isRun)
                    anim.SetInteger("Movement", (int)Movement.Run);
                else
                    anim.SetInteger("Movement", (int)Movement.Walk);
            }
        }
        else //낭떨어지에서 떨어졌을 때
        {
            anim.SetInteger("Movement", (int)Movement.Jump);
            isJumping = true;
        }

    }

    //달리기
    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) //왼쪽 쉬프트키를 눌렀다면
        {
            isRun = true;
            playerSpeed = 2.0f * tempPlayerSpeed;
            if(!isJump)
                anim.SetInteger("Movement", (int)Movement.Run);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || (h == 0 && v == 0)) //왼쪽 쉬프트키를 때거나 플레이어가 멈췄다면
        {
            isRun = false;
            playerSpeed = tempPlayerSpeed;
            if(!isJump)
                anim.SetInteger("Movement", (int)Movement.Walk);
        }
    }

    //덤블링(궁수)
    void Dumbling()
    {

        if (Input.GetMouseButtonDown(1) && dumblingTimer >= 1.0f) //마우스 우클릭한다면
        {
            stateManager.SetDASH(true);
            anim.SetTrigger("isDumbling");
            dumblingTimer = 0;
        }  
    }

    //대쉬(전사)
    void Dash()
    {
        if (dashTimer >= dumblingCoolTime) //쿨타임이 찼을때
        {
            float _v = Input.GetAxisRaw("Vertical");
            float _h = Input.GetAxisRaw("Horizontal");
            if (Input.GetMouseButtonDown(1)) //마우스 우클릭한다면
            {
                dashTimer = 0;
                stateManager.SetDASH(true);
                anim.SetTrigger("isDumbling");
                anim.applyRootMotion = false;
                GetComponent<WarriorAttack>().SkillFinish();
                if (_v > 0 && _h == 0) //전진
                    rig.AddForce(transform.forward * dashPower, ForceMode.Impulse);
                else if(_v > 0 && _h > 0) //오른쪽 대각선
                    rig.AddForce((transform.forward + transform.right).normalized * dashPower, ForceMode.Impulse);
                else if (_v > 0 && _h < 0) //왼쪽 대각선
                    rig.AddForce((transform.forward - transform.right).normalized * dashPower, ForceMode.Impulse);
                else if (_v < 0 && _h == 0) //후진
                    rig.AddForce(-transform.forward * dashPower, ForceMode.Impulse);
                else if (_v < 0 && _h > 0) //오른쪽 뒤 대각선
                    rig.AddForce((-transform.forward + transform.right).normalized * dashPower, ForceMode.Impulse);
                else if (_v < 0 && _h < 0) // 왼쪽 뒤 대각선
                    rig.AddForce((-transform.forward - transform.right).normalized * dashPower, ForceMode.Impulse);
                else if (_v == 0 && _h > 0) //우측
                    rig.AddForce(transform.right * dashPower, ForceMode.Impulse);
                else if (_v == 0 && _h < 0) //좌측
                    rig.AddForce(-transform.right * dashPower, ForceMode.Impulse);
                else //정지
                    rig.AddForce(transform.forward * dashPower, ForceMode.Impulse);
                AudioManager.Instance.PlaySFX(3, 1f);
            }
        }
    }

    RaycastHit ShootHit;
    Transform rayTr; //레이 발사 위치

    private float range = 1000.0f; //레이 길이
    private float teleportdist = 10.0f;

    //텔레포트
    void Teleport()
    {
        float _v = Input.GetAxisRaw("Vertical");
        float _h = Input.GetAxisRaw("Horizontal");
        if (Input.GetMouseButtonDown(1) && dashTimer >= dumblingCoolTime) //마우스 우클릭한다면
        {
            dashTimer = 0;
            GetComponent<PlayerSoundPlay>().Teleport();
            if (_v > 0 && _h == 0) //전진
            {
                if (Physics.Raycast(transform.position + transform.forward * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v > 0 && _h > 0) //오른쪽 대각선
            {
                if (Physics.Raycast(transform.position + (transform.forward + transform.right).normalized * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v > 0 && _h < 0) //왼쪽 대각선
            {
                if (Physics.Raycast(transform.position + (transform.forward - transform.right).normalized * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v < 0 && _h == 0) //후진
            {
                if (Physics.Raycast(transform.position - transform.forward * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v < 0 && _h > 0) //오른쪽 뒤 대각선
            {
                if (Physics.Raycast(transform.position + (-transform.forward + transform.right).normalized * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v < 0 && _h < 0) // 왼쪽 뒤 대각선
            {
                if (Physics.Raycast(transform.position + (-transform.forward - transform.right).normalized * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v == 0 && _h > 0) //우측
            {
                if (Physics.Raycast(transform.position + transform.right * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else if (_v == 0 && _h < 0) //좌측
            {
                if (Physics.Raycast(transform.position - transform.right * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }
            else //정지
            {
                if (Physics.Raycast(transform.position + transform.forward * teleportdist + transform.up * 500.0f, Vector3.down, out ShootHit, range))
                {
                    transform.position = ShootHit.point;
                }
            }

            
            
        }
    }

    #endregion

    public void ResetRootMotion()
    {
        stateManager.SetDASH(false);
        GetComponent<WeaponSheath>().BayonetState();
        stateManager.AttackEnd();
        anim.applyRootMotion = true;
    }

    public float PlayerSpeed()
    {
        return playerSpeed;
    }

    public bool IsJump()
    {
        return isJump;
    }
}
