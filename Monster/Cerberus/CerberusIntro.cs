using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusIntro : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rig;
    public GameObject cerberusOrigin;

    private bool introing = false;

    void Update()
    {
        Intro();
    }

    private void Intro()
    {
        Vector3 dir = cerberusOrigin.transform.position - transform.position;
        
        if (dir.magnitude >= 0.1f && !introing)
        {
            dir.Normalize();
            transform.position += dir * 5 * Time.deltaTime;
        }

        if(dir.magnitude <= 0.1f)
        {
            introing = true;
            anim.SetTrigger("Idle");
            cerberusOrigin.SetActive(true);
            transform.gameObject.SetActive(false);
        }
            
    }
}
