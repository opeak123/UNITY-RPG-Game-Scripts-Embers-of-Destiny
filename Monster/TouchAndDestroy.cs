using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(this.gameObject, 0.1f);
        }
    }
}
