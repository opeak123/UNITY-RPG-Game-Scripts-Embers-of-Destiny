#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ReaperAnimation : MonoBehaviour
{
    private Animator ani;
    private bool isDamaged = false;
    
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(IntroAlive());
    }
    public void ReaperIdle(bool value)
    {
        ani.SetBool("idle", value);
    }
    public void ReaperSkillA()
    {
        ani.SetTrigger("skillA");
        StartCoroutine(SkilWaitA());
    }
    public void ReaperSkillB()
    {
        ani.SetTrigger("skillB");
    }

    public void ReaperForward()
    {
        ani.SetTrigger("forward");
    }
    public void ReaperResetForward()
    {
        ani.ResetTrigger("forward");
    }
    public void ReaperAttack()
    {
        ani.SetTrigger("attack");
    }
    public void ReaperResetAttack()
    {
        ani.ResetTrigger("attack");
    }
    public void ReaperDead()
    {
        ani.SetTrigger("dead");
    }
    public void ReaperDamage()
    {
        if(!isDamaged)
        {
            StartCoroutine(Damgaed());
        }
    }

    IEnumerator Damgaed()
    {
        isDamaged = true;
        ani.SetTrigger("hit");

        yield return new WaitForSeconds(0.1f);

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        float time = stateInfo.length;
        yield return new WaitForSeconds(6f);
        isDamaged = false;
    }

    IEnumerator SkilWaitA()
    {
        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("skillA"))
        {
            yield return null;
            stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        }
        ReaperIdle(true);
    }


    private IEnumerator IntroAlive()
    {
        ani.SetTrigger("alive");
        GetComponent<AudioSource>().Play();
        Vector3 originPos = transform.position;
        Quaternion originRot = transform.rotation;
        float timer = 0f;

        while (transform.position.y < 1f)
        {
            float aniTime =  ani.GetCurrentAnimatorStateInfo(0).length;
            timer += Time.deltaTime;
            float t = timer / aniTime;
            float newPosY = Mathf.Lerp(originPos.y, 1f, t);
            transform.position = new Vector3(originPos.x, newPosY, originPos.z);
            transform.rotation = originRot;
            yield return null;
            //yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        transform.rotation = originRot;
        GetComponent<ReaperMovement>().Activated(true);
    }


}
