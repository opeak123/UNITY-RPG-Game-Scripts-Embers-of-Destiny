using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private ShopNPC currentShopNPC;
    private DialogueManager dialogueManager; 


    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //if (canInteract && Input.GetKeyDown(KeyCode.G))
        //{
        //    if (currentShopNPC != null)
        //    {
        //        dialogueManager.ContinueDialogue(); // 대화 계속 진행
        //    }
        //}

        if(Input.GetKeyDown(KeyCode.G))
        {
            if (currentShopNPC != null)
            {
                //대화 끝
                if (currentShopNPC.ContinueDialogue())
                {
                    //currentShopNPC = null;
                    currentShopNPC.Interact();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("ShopNPC"))
        {
             if (currentShopNPC == null)
             {
                 currentShopNPC = other.gameObject.GetComponent<ShopNPC>();
                 currentShopNPC.Interact();
             }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShopNPC"))
        {
            currentShopNPC = null;
            dialogueManager.EndDialogue();
        }
    
    }

    float speed = 10f;

    Rigidbody rigidbody;
    Vector3 movement;
    float h, v;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
 
    void FixedUpdate()
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

}