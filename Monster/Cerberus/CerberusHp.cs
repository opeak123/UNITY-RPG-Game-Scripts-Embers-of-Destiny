using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CerberusHp : MonoBehaviour
{
    Animator anim;
    Slider HpSlider;
    Image HpImage;
    Transform playerTr;
    [SerializeField]
    ParticleSystem[] FireRings;
    GameObject SecondPazeObject;
    public CerberusData data;
    public GameObject DMGText;

    private float currentHp = 0;
    private float nuckBack = 6.0f;
    public bool isDie = false;
    private bool secondPaze = false;
    private bool getHit = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        HpSlider = transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>();
        HpSlider.maxValue = data.hp;
        currentHp = data.hp;
        SecondPazeObject = transform.GetChild(3).gameObject;
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            currentHp = 3000f;
        if (nuckBack <= 6.0f)
            nuckBack += Time.deltaTime;
        if (currentHp <= 0 && !isDie)
        {
            Die();
            //Destroy(gameObject, 13.0f);
        }
        else
        {
            HpSlider.value = currentHp;
        }

        if (currentHp / HpSlider.maxValue <= 0.3f && !secondPaze)
        {
            secondPaze = true;
            SecondPazeObject.SetActive(true);
            data.atk += 40;
            anim.SetTrigger("SecondPaze");
            for (int i = 0; i < FireRings.Length; i++)
            {
                FireRings[i].Play();
            }
        }
    }
    public void dead()
    {
        anim.SetInteger("State", 0);
    }

    private void Die()
    {
        isDie = true;
        DisableChangeChildTag(transform);
        GetComponent<CerberusAnimationEvent>().BreatheEnd();
        GetComponent<CerberusAnimationEvent>().ChargeEnd();
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(GetComponent<BoxCollider>());
        DisableChildColliders(transform);
        Destroy(GetComponent<CerberusMovement>());
        anim.SetInteger("State", 10);
        anim.SetTrigger("Die");
        for (int i = 0; i < FireRings.Length; i++)
        {
            FireRings[i].Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDamage>() != null)
        {
            if (!isDie)
            {
                HitCerberus(other.GetComponent<PlayerDamage>().DMG(), other.transform);
                StartCoroutine(GetHit());
                if (currentHp <= 0)
                    currentHp = 0;
            }
        }
    }

    private void HitCerberus(float _damage, Transform _pos)
    {
        currentHp -= (_damage - ((float)data.def * 0.1f));
        GameObject dmgtext = Instantiate(DMGText, _pos.position, playerTr.rotation, transform);
        dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
        dmgtext.GetComponent<TextMeshPro>().fontSize = 15;
        dmgtext.GetComponent<DamageText>().damage = _damage - ((float)data.def * 0.1f);
        if (nuckBack >= 6.0f)
        {
            nuckBack = 0;
            anim.SetTrigger("NuckBack");
        }
    }

    IEnumerator GetHit()
    {
        getHit = true;
        Color origincolor = new Color(1, 1, 1);
        transform.GetComponent<SkinnedMeshRenderer>().material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<SkinnedMeshRenderer>().material.color = origincolor;
        yield return new WaitForSeconds(1.0f);
        getHit = false;
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.transform.GetComponent<PlayerDamage>() != null)
    //    {
    //        GetComponent<CerberusMovement>().checkPlayer = true;
    //        currentHp -= col.transform.GetComponent<PlayerDamage>().DMG();
    //        StartCoroutine(GetHit());
    //        if (currentHp <= 0)
    //            currentHp = 0;
    //    }    
    //}

    public bool isSecondPaze()
    {
        return secondPaze;
    }

    public bool isGetHit()
    {
        return getHit;
    }

    void DisableChildColliders(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }

            // 자식 오브젝트에 대해서도 재귀적으로 호출
            DisableChildColliders(child);
        }
    }

    void DisableChangeChildTag(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.tag = "Untagged";

            // 자식 오브젝트에 대해서도 재귀적으로 호출
            DisableChildColliders(child);
        }
    }
}
