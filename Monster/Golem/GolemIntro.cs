#pragma warning disable 0414
using System.Collections;
using UnityEngine;

public class GolemIntro : MonoBehaviour
{
    private GameObject splitGolem;
    private GameObject rockGolem;
    private Animator ani;
    //public AudioSource audioSource;
    private bool isAnimating = false;
    private bool OnColliderHit = false;
    public void onColliderhit(bool _onColHit)
    {
        OnColliderHit = _onColHit;
    }

    void Start()
    {
        ani = transform.GetChild(0).GetComponent<Animator>();
        rockGolem = transform.GetChild(0).gameObject;
        splitGolem = transform.GetChild(1).gameObject;
    }


    void Update()
    {
        PlayerReachToPoint();
    }

    private void PlayerReachToPoint()
    {
        if (OnColliderHit)
        {
            //audioSource.Play();

            Animator splitAni = splitGolem.GetComponent<Animator>();
            splitAni.speed = 0.9f; //0.5
            splitAni.SetTrigger("construct");

            StartCoroutine(WaitForAnimation(splitAni));
        }
    }

    IEnumerator WaitForAnimation(Animator animator)
    {
        isAnimating = true;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("construct"))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        splitGolem.gameObject.SetActive(false);
        rockGolem.gameObject.SetActive(true);

        if (rockGolem.activeInHierarchy)
        {
            ani.SetTrigger("roar");
            yield return new WaitForSeconds(0.5f);
            GetComponent<AudioSource>().Play();
            while (!ani.GetCurrentAnimatorStateInfo(0).IsName("roar"))
            {
                yield return new WaitForEndOfFrame();
            }

        }
        isAnimating = false;
        yield return new WaitForSeconds(2f);
        DestroyImmediate(this);
    }
}
