using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestNpc : MonoBehaviour
{

    Quest _quest;
    public QuestData _qData;
    private DialogueManager _dialogue;


    void Start()
    {
        _quest = FindObjectOfType<Quest>();
        _dialogue = FindObjectOfType<DialogueManager>();
    }

    public void GolemQuest()
    {
        _quest.Add(_qData);
        _dialogue.EndDialogue();
    }
    public void GolemQuestDone()
    {
        _quest.SearchQuest(_qData.QuestID);
        UISoundPlay.Instance.QuestDoneSound();
        _dialogue.EndDialogue();
    }

}
