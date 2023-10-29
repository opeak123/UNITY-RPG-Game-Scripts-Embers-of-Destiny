using UnityEngine;
public class MonsterAnimation : MonoBehaviour
{
    //몬스터 애니메이션
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    
    //idle
    public void Idle()
    {
        ani.SetTrigger("idle");
    }
    //걷기
    public void Walk()
    {
        ani.SetTrigger("walk");
    }
    //몬스터가 추적
    public void Run()
    {
        ani.SetTrigger("run");
    }
    //공격
    public void Attack()
    {
        ani.SetTrigger("attack");
    }
    //몬스터가 공격받았을 때
    public void Damaged()
    {
        ani.SetTrigger("damaged");
    }
    public void Dead()
    {
        ani.SetTrigger("dead");
    }
}