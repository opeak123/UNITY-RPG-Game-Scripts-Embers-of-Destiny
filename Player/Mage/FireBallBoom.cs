using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBoom : MonoBehaviour
{
    [SerializeField]
    GameObject Blast;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy" || other.transform.tag == "WOLF")
        {
            Instantiate(Blast, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 1.0f);
        }
    }
}
