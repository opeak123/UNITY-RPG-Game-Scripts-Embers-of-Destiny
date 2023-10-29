using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemOutro : MonoBehaviour
{
    public GameObject splitGolem;
    public GameObject rockGolem;
    private Animator splitGolemAni;
    private GameObject hpSlider;

    private void Awake()
    {
        rockGolem = transform.GetChild(0).gameObject;
        splitGolem = transform.GetChild(1).gameObject;
        hpSlider = transform.GetChild(2).gameObject;
        splitGolemAni = splitGolem.GetComponent<Animator>();
    }

    private void Start()
    {
        splitGolemAni.speed = 0.7f;
        splitGolem.transform.position = rockGolem.transform.position;
        rockGolem.SetActive(false);
        splitGolem.SetActive(true);
        splitGolem.GetComponent<AudioSource>().Play();
        splitGolemAni.SetTrigger("explode");
        StartCoroutine(GolemDie());
    }

    IEnumerator GolemDie()
    {
        yield return new WaitForSeconds(3f);
        hpSlider.SetActive(false);
        yield return new WaitForSeconds(10f);
        //Destroy(this.gameObject);
    }

}
