public class MonsterData
{
    private int HP { get; set; }    // 체력
    private int MaxHP { get; set; } //최대체력
    private int DEF { get; set; }  // 방어력
    private int ATK { get; set; }  // 공격력
    private float SPEED { get; set; } // 속도
    private int EXP { get; set; }    // 획득 경험치

    private bool STUN { get; set; } // 스턴
    private bool BURN { get; set; } // 화상
    private bool PROVOKE { get; set; } // 도발

    // Getter 메서드
    public int GetHP() => HP;
    public int GetMaxHP() => MaxHP;
    public int GetDEF() => DEF;
    public int GetATK() => ATK;
    public float GetSPEED() => SPEED;
    public bool GetSTUN() => STUN;
    public bool GetBURN() => BURN;
    public bool GetPROVOKE() => PROVOKE;
    public int GetEXP() => EXP;


    // Setter 메서드
    public void SetHP(int value) => HP = value;
    public void SetMaxHP(int value) => MaxHP = value;
    public void SetDEF(int value) => DEF = value;
    public void SetATK(int value) => ATK = value;
    public void SetSPEED(float value) => SPEED = value;
    public void SetSTUN(bool value) => STUN = value;
    public void SetBURN(bool value) => BURN = value;
    public void SetPROVOKE(bool value) => PROVOKE = value;
    public void SetEXP(int value) => EXP = value;
}