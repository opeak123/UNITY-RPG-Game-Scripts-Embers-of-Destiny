using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GolemAnimation : MonoBehaviour
{

    private Animator ani;
    private int attackAnimationCount = 4;
    private int previousAttackAnimation = 0;
    private int currentAttackAnimation = 0;
    private float previousAttackTime = 0f;
    private int AttackValue = 0;
    private bool isDamaged = false;
    public AudioSource audioSource;
    public AudioSource bgmSource;
    private MonsterAudioClip audioClip;
    
    //public AudioSource[] audioSources = new AudioSource[2];
    //private MonsterAudioClip audioClip;

    public int attackValue()
    {
        //return AttackValue = currentAttackAnimation;
        return ani.GetInteger("attack");
    }

    void Start()
    {
        audioClip = GetComponent <MonsterAudioClip>();
        bgmSource.Play();
        if (ani == null)
        {
            ani = GetComponent<Animator>();
        }
    }
    public void Idle()
    {
        ani.speed = 0.5f;
        ani.SetTrigger("idle");
    }
    public void Walk(bool value)
    {
        ani.speed = 0.5f;
        ani.SetBool("walk", value);
    }
    public void Throw()
    {
        ani.speed = 1f;
        StartCoroutine(WaitForThrowAnimation());
    }
    IEnumerator WaitForThrowAnimation()
    {
        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        ani.SetTrigger("throw");
        while (!stateInfo.IsName("throw"))
        {
            yield return null;
            stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        }
    }
    public void Damage()
    {
        if (!isDamaged)
        {
            audioSource.clip = audioClip.clip[4];
            audioSource.Play();
            GetComponent<GolemMovement>().StopNavMesh();
            ani.speed = 1f;
            StartCoroutine(GolemDamaged());
            StartCoroutine(WaitForDamageAnimation());
        }
    }

    private IEnumerator WaitForDamageAnimation()
    {
        isDamaged = true;
        GetComponent<GolemController>().isWalk = false;
        ani.SetTrigger("damaged");

        yield return new WaitForSeconds(0.1f);

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        float time = stateInfo.length;
        yield return new WaitForSeconds(time + 3);
        GetComponent<GolemController>().isWalk = true;
        //피격시 damage 애니메이션을 실행 후 6초간 대기
        yield return new WaitForSeconds(6f);

        isDamaged = false;
    }

    IEnumerator GolemDamaged()
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        Material mat = skinnedMesh.material;
        Color originColor = mat.color;
        mat.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        mat.color = originColor;

        float backForce = 5f;
        Vector3 targetPosition = transform.parent.position - transform.parent.forward * backForce;

        float tiemr = 0f;

        while (tiemr < 1f)
        {
            tiemr += Time.deltaTime;

            float t = tiemr / 1f;
            transform.parent.position = Vector3.Lerp(transform.parent.position, targetPosition, t);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }

    public void EventRotate()
    {
        StartCoroutine(LookRotate());
    }
    IEnumerator LookRotate()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 5f);

        foreach(Collider cols in col)
        {
            if(cols.gameObject.tag != "Player")
                yield return null;
        }
        
        Vector3 tr = GameObject.FindWithTag("Player").transform.position;
        Vector3 lookDir = tr - transform.position;
        Vector3 rightVec = transform.right;
        float rightDot = Vector3.Dot(rightVec, lookDir);

        lookDir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20f * Time.deltaTime);
            yield return null;
        }
    }
    public void Attack()
    {
        ani.speed = 1f;
        float currentTime = Time.time;
        AttackValue = currentAttackAnimation;

        if (currentAttackAnimation == 0 || currentTime >= previousAttackTime + 6f)
        {
            int newAttackAnimation = Random.Range(1, attackAnimationCount + 1);
            AttackValue = newAttackAnimation;
            while (newAttackAnimation == currentAttackAnimation)
            {
                newAttackAnimation = Random.Range(1, attackAnimationCount + 1);
            }

            currentAttackAnimation = newAttackAnimation;
            previousAttackAnimation = currentAttackAnimation;
            previousAttackTime = currentTime;
            ani.SetInteger("attack", currentAttackAnimation);
        }
        else
        {
            ani.SetInteger("attack", 0);
            ani.SetTrigger("idle");
        }
    }

    public void Roar()
    {
        ani.speed = 0.5f;
        ani.SetTrigger("roar");
        StartCoroutine(WaitForRoarAnimation());
    }
    IEnumerator WaitForRoarAnimation()
    {
        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("roar"))
        {
            yield return null;
            stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        }
    }
}