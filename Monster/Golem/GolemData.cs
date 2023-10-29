using UnityEngine;

[CreateAssetMenu(fileName = "GolemData", menuName = "Golem")]
public class GolemData : ScriptableObject
{
    public new string name;
    public int hp;
    public int atk;
    public int def;
}
