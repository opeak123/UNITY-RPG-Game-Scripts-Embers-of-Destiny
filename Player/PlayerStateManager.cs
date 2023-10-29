using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    PlayerState P_State;

    private int CLASS { get; set; } // 클래스
    private int LV { get; set; } // 레벨
    private int EXP { get; set; } // 경험치
    private int HP { get; set; }    // 체력
    private int MaxMP { get; set; }    // 마력
    private int CurrentMp { get; set; }
    private int DEF { get; set; }  // 방어력
    private int ATK { get; set; }  // 공격력
    private int STATDMG { get; set; } // 스탯데미지
    private int TOTALDMG { get; set; } // 토탈데미지
    private float MOVESPEED { get; set; } // 이동속도
    private float ATKSPEED { get; set; }  // 공격속도

    private bool STUN { get; set; } // 스턴
    private bool BURN { get; set; } // 화상
    private bool POISION { get; set; } // 독
    private bool DIE { get; set; }  // 죽음
    private bool DASH { get; set; } // 대쉬

    // Getter 메서드
    public int GetCLASS() => CLASS;
    public int GetLV() => LV;
    public int GetEXP() => EXP;
    public int GetHP() => HP;
    public int GetMaxMP() => MaxMP;
    public int GetCurrentMp() => CurrentMp;
    public int GetDEF() => DEF;
    public int GetATK() => ATK;
    public int GetSTATDMG() => STATDMG;
    public int GetTOTALDMG() => TOTALDMG;
    public float GetMOVESPEED() => MOVESPEED;
    public float GetATKSPEED() => ATKSPEED;
    public bool GetSTUN() => STUN;
    public bool GetBURN() => BURN;
    public bool GetPOISION() => POISION;
    public bool GetDIE() => DIE;
    public bool GetDASH() => DASH;

    // Setter 메서드
    public void SetCLASS(int value) => CLASS = value;
    public void SetLV(int value) => LV = value;
    public void SetEXP(int value) => EXP = LevelManager(value);
    public void SetHP(int value) => HP = 900 + (100 * StatManager()) + value;
    public void SetMaxMP(int value) => MaxMP = 400 + (100 * StatManager()) + value;
    public void SetCurrentMp(int value) => CurrentMp = value;
    public void SetDEF(int value) => DEF = 9 + (1 * StatManager()) + value;
    public void SetATK(int value) => ATK = 9 + (1 * StatManager()) + value;
    public void SetMOVESPEED(float value) => MOVESPEED = (float)2.9 + (float)(0.1 * StatManager()) + value;
    public void SetATKSPEED(float value) => ATKSPEED = (float)0.9 + (float)(0.1 * StatManager()) + value;
    public void SetSTUN(bool value) => STUN = value;
    public void SetBURN(bool value) => BURN = value;
    public void SetPOISION(bool value) => POISION = value;
    public void SetDIE(bool value) => DIE = value;
    public void SetDASH(bool value) => DASH = value;

    private bool isAttack = false;
    private bool isBackStep = false;

    private void Awake()
    {
        SetLV(1);
    }

    private void Start()
    {
        SetHP(0);
        SetMaxMP(0);
        SetDEF(0);
        SetATK(0);
        SetATKSPEED(0);
        SetMOVESPEED(0);
        P_State.SettingHpMp();
    }

    private int LevelManager(int exp)
    {
        int maxExp = 1000 + (100 * GetLV());
        int curExp = GetEXP() + exp;

        if (maxExp <= curExp)
        {
            curExp = curExp - maxExp;
            SetLV(GetLV() + 1);
            SetMaxMP(0);
            P_State.currentHp = GetHP() + 100;
            P_State.currentMp = GetMaxMP();

            QuickSkillUI.Instance.AllQuickSlotCheck();
            SkillBook.Instance.AllSkillSlotCheck();
        }
        return curExp;
    }

    public int StatManager()
    {
        int statAdd = GetLV();
        return statAdd;
    }

    public void FindStateManager()
    {
        P_State = FindObjectOfType<PlayerState>();
    }

    private void Update()
    {
        if (!DIE && Input.GetKeyDown(KeyCode.Alpha5))
            HitPlayer(100);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            SetEXP(500);
            
        if (Input.GetKeyDown(KeyCode.F5))
            SetCLASS(0);
        if (Input.GetKeyDown(KeyCode.F6))
            SetCLASS(1);
        if (Input.GetKeyDown(KeyCode.F7))
            SetCLASS(2);
        
    }

    public void BackStep()
    {
        isBackStep = true;
    }

    public bool IsBackStep()
    {
        return isBackStep;
    }

    public void Attack()
    {
        isAttack = true;
    }

    public void AttackEnd()
    {
        isBackStep = false;
        isAttack = false;
    }

    public bool IsAttack()
    {
        return isAttack;
    }

    //데미지계산
    public void HitPlayer(int _damage)
    {
        P_State.HitPlayer(_damage, true);
    }

    public void MaxHp(int _Hp)
    {
        SetHP(_Hp);
    }

}
