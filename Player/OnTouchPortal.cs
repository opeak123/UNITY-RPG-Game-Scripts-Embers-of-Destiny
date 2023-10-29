using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnTouchPortal : MonoBehaviour
{
    public SceneMove sceneMoveScript;
    //public GameObject loadingBox;
    //public Text loadingText;
    //public Slider slider;
    public Image image;
    public Sprite[] sprites = new Sprite[3];
    //private float loadingTime;

    //private bool portal1;
    //private bool portal2;
    //private bool portal3;
    //private string SceneName;
    private void Start()
    {
        sceneMoveScript = FindObjectOfType<SceneMove>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player") && sceneMoveScript.dirty == 0)
        {
            FindObjectOfType<CamController>().NPCTalk = true;
            Debug.Log("on hit");
            PortalNameCheck();
            GetComponent<AudioSource>().Play(); //버튼 클릭할때 틀어줘야함
            sceneMoveScript.exitGroup.alpha = 1;
            sceneMoveScript.exitGroup.blocksRaycasts = true;
            sceneMoveScript.exitGroup.interactable = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<CamController>().NPCTalk = false;
            sceneMoveScript.dirty = 0;
            sceneMoveScript.exitGroup.alpha = 0;
            sceneMoveScript.exitGroup.blocksRaycasts = false;
            sceneMoveScript.exitGroup.interactable = false;
        }
    }

    public void PortalNameCheck()
    {
        if(this.name == "Portal1")
        {
            image.sprite = sprites[0];
            print("this is portal 1");
            sceneMoveScript.SceneName = "CerberusScene";
        }
        else if(this.name == "Portal2")
        {
            image.sprite = sprites[1];
            print("this is portal 2");
            sceneMoveScript.SceneName = "GolemScene";
        }
        else
        {
            image.sprite = sprites[2];
            print("this is portal 3");
            sceneMoveScript.SceneName = "ReaperScene";
        }
    }
}
