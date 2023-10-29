using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSecondPaze : MonoBehaviour
{
    GameObject Nova;
    GameObject[] Arrows;

    private bool secondPazing = false;
    void Awake()
    {
        Nova = transform.parent.GetChild(4).gameObject;
        StartCoroutine(NovaBumb());
    }

    void Update()
    {
        if (transform.localScale.x <= 2)
            transform.localScale += new Vector3(2, 2, 2) * 0.5f * Time.deltaTime;
    }

    IEnumerator NovaBumb()
    {
        transform.parent.GetComponent<CerberusMovement>().StopMovement();
        secondPazing = true;
        transform.parent.GetComponent<CerberusHp>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        GetComponent<ParticleSystem>().Stop();
        Nova.SetActive(true);
        if (GameObject.FindGameObjectsWithTag("playerAttack") != null)
            Arrows = GameObject.FindGameObjectsWithTag("playerAttack");
        foreach (GameObject _arrow in Arrows)
        {
            Destroy(_arrow);
        }
        yield return new WaitForSeconds(1.0f);
        secondPazing = false;
        transform.parent.GetComponent<CerberusMovement>().StartMovement();
        transform.parent.GetComponent<CerberusHp>().enabled = true;
        Destroy(Nova);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    public bool SecondPazing()
    {
        return secondPazing;
    }
}
