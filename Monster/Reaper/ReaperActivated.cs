using System.Collections;
using UnityEngine;

public class ReaperActivated : MonoBehaviour
{
    public GameObject Reaper;
    //private BoxCollider collider;
    private Vector3 cameraOriginPos;
    public GameObject virtualCamera;
    public GameObject subCamera;
    public GameObject startBGM;
    private void Start()
    {
        //collider = GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider col)
    {
        cameraOriginPos = Camera.main.transform.position;
        
        if (col.gameObject.CompareTag("Player"))
        {
            startBGM.SetActive(false);
            GetComponent<AudioSource>().Play();
            Camera.main.targetDisplay = 1;
            subCamera.GetComponent<Camera>().targetDisplay = 0;
            FindObjectOfType<ReaperCinamacine>().AutoDollyEnabled();
            FindObjectOfType<PlayerMovement>().enabled = false;
            Reaper.SetActive(true);
            StartCoroutine(CameraPosition());


            if (Reaper == null)
            {
                Destroy(gameObject);
            }
        }
    }
    IEnumerator CameraPosition()
    {
        yield return new WaitForSeconds(8f);
        //collider.isTrigger = false;
        virtualCamera.SetActive(false);
        Camera.main.targetDisplay = 0;
        subCamera.GetComponent<Camera>().targetDisplay = 1;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraOriginPos, 2f * Time.deltaTime);
        Camera.main.transform.position = cameraOriginPos;
        FindObjectOfType<PlayerMovement>().enabled = true;
    }
}