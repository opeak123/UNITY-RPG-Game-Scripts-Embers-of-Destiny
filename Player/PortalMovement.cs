using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMovement : MonoBehaviour
{
    public GameObject cerberusIntro;
    public GameObject cerberusOrigin;

    private bool openning = false;
    private void Update()
    {
        //    if (transform.position.y <= 0 && !openning)
        //        transform.Translate(new Vector3(0, 10, 0) * Time.deltaTime);
        //    else if (transform.position.y >= 0)
        //    {
        //        openning = true;
        //        cerberusIntro.SetActive(true);
        //    }

        //    if(cerberusOrigin.gameObject.activeInHierarchy)
        //    {
        //        if (transform.position.y >= -10)
        //            transform.Translate(new Vector3(0, -10, 0) * Time.deltaTime);
        //        else
        //            Destroy(gameObject);
        //    }
        //}

        if(!openning)
        {
            if (transform.position.y < -0.5)
            {
                transform.Translate(new Vector3(0, 9.5f, 0) * Time.deltaTime);
            }
            else
            {
                openning = true;
                cerberusIntro.SetActive(true);
            }
        }


        if(cerberusOrigin.gameObject.activeInHierarchy)
        {
            if (transform.position.y >= -20)
                transform.Translate(new Vector3(0, -10, 0) * Time.deltaTime);
            else
                Destroy(gameObject);
        }

    }
}
