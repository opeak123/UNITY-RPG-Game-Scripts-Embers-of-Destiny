using UnityEngine;
using System.Collections;
public class SpawnSpotTrigger : MonoBehaviour
{
    public ParticleSystem particle;
    void Start()
    {
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();   
        StartCoroutine(ParticleOn());
        Destroy(gameObject,10f);
    }

    IEnumerator ParticleOn()
    {
        yield return new WaitForSeconds(5f);
        particle.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
    }
}
