using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineStart : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    [SerializeField]
    private CinemachineTrackedDolly dolly;
    public Camera subCam;
    public GameObject Cerberus;
    public GameObject startBGM;

    private Vector3 cameraOriginPos;
    private void Start()
    {
        dolly = cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>();
    }


    private void OnTriggerEnter(Collider col)
    {
        cameraOriginPos = Camera.main.transform.position;

        if (col.gameObject.CompareTag("Player"))
        {
            startBGM.SetActive(false);
            Cerberus.SetActive(true);
            Cerberus.GetComponent<AudioSource>().Play();
            Camera.main.GetComponent<AudioListener>().enabled = false;
            subCam.GetComponent<AudioListener>().enabled = true;
            Camera.main.targetDisplay = 1;
            subCam.GetComponent<Camera>().targetDisplay = 0;
            dolly.m_AutoDolly.m_Enabled = true;
            FindObjectOfType<PlayerMovement>().enabled = false;
            StartCoroutine(CameraPosition());

            if (Cerberus == null)
            {
                Destroy(gameObject);
            }
        }
    }
    IEnumerator CameraPosition()
    {
        yield return new WaitForSeconds(8f);
        GetComponent<Collider>().enabled = false;
        cinemachine.gameObject.SetActive(false);
        Camera.main.GetComponent<AudioListener>().enabled = true;
        subCam.GetComponent<AudioListener>().enabled = false;
        Camera.main.targetDisplay = 0;
        subCam.GetComponent<Camera>().targetDisplay = 1;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraOriginPos, 2f * Time.deltaTime);
        Camera.main.transform.position = cameraOriginPos;
        FindObjectOfType<PlayerMovement>().enabled = true;
    }

}
