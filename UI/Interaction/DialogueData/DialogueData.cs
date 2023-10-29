using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    public bool isShop;
    public bool isBlacksmith;
    public bool isQuest;
    public string[] lines;
}