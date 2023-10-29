using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public GameObject dialoguePanel;
    public RawImage npcImage;
    public Texture shopRendererTexture;  // NPC1에 해당하는 Renderer Texture
    public Texture blacksmithRendererTexture;  // NPC2에 해당하는 Renderer Texture
    public Texture QuestRenedererTexture;


    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    bool isEndDialogue = false;


    private void Start()
    {
        dialoguePanel.SetActive(false);

    }

    public void StartDialogue(DialogueData dialogue)
    {
        isEndDialogue = false;
        //dialoguePanel.SetActive(true);
        currentDialogue = dialogue;
        currentLineIndex = 0;
   
    }


    public bool ContinueDialogue()
    {
        UpdateDialogue();

        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return true;
        }

        dialogueText.text = currentDialogue.lines[currentLineIndex++];

        return false;
    }

    private void UpdateDialogue()
    {
        dialoguePanel.SetActive(true);

        if (currentDialogue.isShop)
        {
            button1.gameObject.SetActive(true);
            button1.GetComponentInChildren<Text>().text = "물품 구매";
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);
            button4.gameObject.SetActive(false);
            npcImage.texture = shopRendererTexture;
            UISoundPlay.Instance.PurchaseSound();
        }
        else if (currentDialogue.isBlacksmith)
        {
            button2.gameObject.SetActive(true);
            button2.GetComponentInChildren<Text>().text = "강화";
            button1.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);
            button4.gameObject.SetActive(false);
            npcImage.texture = blacksmithRendererTexture;
            UISoundPlay.Instance.BlackSmithSound();

        }
        else if (currentDialogue.isQuest)
        {
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(true);
            button4.gameObject.SetActive(true);
            button3.GetComponentInChildren<Text>().text = "의뢰 수락";
            button4.GetComponentInChildren<Text>().text = "의뢰 완료";
            npcImage.texture = QuestRenedererTexture;
        }
    }

    public void EndDialogue()
    {
        isEndDialogue = true;
        dialoguePanel.SetActive(false);
        currentDialogue = null;
        currentLineIndex = 0;
    }

    public bool IsEndDialogue()
    {
        return isEndDialogue;
    }

}