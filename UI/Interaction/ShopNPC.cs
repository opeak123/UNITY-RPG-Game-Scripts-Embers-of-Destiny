using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public DialogueData dialogueData;
    private DialogueManager dialogueManager;
    private PlayerStateManager _playerStat;
    private PlayerMovement playerMove;
    private CamController camControl;


    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerMove = FindObjectOfType<PlayerMovement>();
        camControl = FindObjectOfType<CamController>();
 
    }

    private void Update()
    {
        if (firstCheck && Input.GetKeyDown(KeyCode.G))
            ContinueDialogue();
    }

    bool firstCheck = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            camControl.NPCTalk = true;
            Interact();
            firstCheck = true;
            //if (other.gameObject != null && Input.GetKeyDown(KeyCode.G))
            //{
            //    ContinueDialogue();

            //    Debug.Log("asd");
            //    //Interact();

            //}
            //else if (other.gameObject == null)
            //    dialogueManager.EndDialogue();


        }
      
    }
    public bool NPCCheck()
    {
        return firstCheck;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            firstCheck = false;
            camControl.NPCTalk = false;
            dialogueManager.EndDialogue();
        }

    }
    public void Interact()
    {
        dialogueManager.StartDialogue(dialogueData);
    }
    public bool ContinueDialogue()
    {
        return dialogueManager.ContinueDialogue();
    }
    public bool GetShopType()
    {
        if (dialogueData.isShop)
            return true;
        else
            return false;
    }
}