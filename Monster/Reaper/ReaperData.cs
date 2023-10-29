using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReaperData", menuName = "Reaper")]
public class ReaperData : ScriptableObject
{
    public new string name;
    public int maxHp;
    public int hp;
    public int atk;
    public int def;
}
