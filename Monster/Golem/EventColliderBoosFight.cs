using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EventColliderBoosFight : MonoBehaviour
{
    public Camera cinemachineCamera;
    [SerializeField]
    private CinemachineTrackedDolly trackedDolly; 
    protected bool onColliderHit = false;
    private Vector3 originPos;
    int dirty = 0;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    public GameObject startBGM;

    private void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    public bool Dead = false;
    private void LateUpdate()
    {
        if (Dead)
        {
            Dead = false;
            StartCoroutine(GoHome());
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        originPos = Camera.main.transform.position;

        if (col.gameObject.CompareTag("Player") && dirty ==0)
        {
            dirty++;
            startBGM.SetActive(false);
            GetComponent<AudioSource>().Play();
            onColliderHit = true;
            GolemIntro go = FindObjectOfType<GolemIntro>();

            Camera.main.targetDisplay = 1;
            cinemachineCamera.GetComponent<Camera>().targetDisplay = 0;
            trackedDolly.m_AutoDolly.m_Enabled = true;

            if (go != null)
            {
                go.onColliderhit(onColliderHit);
            }

            StartCoroutine(CameraPosition());
        }
    }

    IEnumerator CameraPosition()
    {
        yield return new WaitForSeconds(13f);
        cinemachineCamera.gameObject.SetActive(false);
        Camera.main.targetDisplay = 0;
        cinemachineCamera.targetDisplay = 1;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, originPos, 2f * Time.deltaTime);
        Camera.main.transform.position = originPos;
    }

    public Text timertext;
    public GameObject obj;
    public IEnumerator GoHome()
    {
        yield return new WaitForSeconds(20f);
        FadeInOut _fadeinOut = FindObjectOfType<FadeInOut>();
        obj.SetActive(true);
        timertext.text = "5";
        yield return new WaitForSeconds(1f);
        timertext.text = "4";
        yield return new WaitForSeconds(1f);
        timertext.text = "3";
        yield return new WaitForSeconds(1f);
        timertext.text = "2";
        yield return new WaitForSeconds(1f);
        timertext.text = "1";
        yield return new WaitForSeconds(1f);
        StartCoroutine(_fadeinOut.FadeOutStart());
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        saveManager.Save();
        yield return new WaitForSeconds(3.0f);
        obj.SetActive(false);
        SceneManager.LoadScene("Map");
    }
}
