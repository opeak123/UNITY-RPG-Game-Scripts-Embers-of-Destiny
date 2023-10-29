using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordJudgementWait : MonoBehaviour
{
    [SerializeField]
    GameObject SwordJudgement;
    PlayerStateManager stateManager;
    WarriorAttack warriorAtk;

    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 25; // Y축 누적 이동량 

    void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        warriorAtk = FindObjectOfType<WarriorAttack>();
    }

    void Update()
    {
        if (stateManager.IsAttack())
        {
            xmove = Input.GetAxis("Mouse X");
            ymove = Input.GetAxis("Mouse Y");

            //transform.position += new Vector3(xmove, 0, ymove);
            transform.position += (-transform.up * ymove) + (transform.right * xmove);
        }
        
        if (Input.GetKeyUp(KeyCode.F) && stateManager.IsAttack())
        {
            stateManager.AttackEnd();
            Invoke("CreateSword", 0.78f);
        }
    }

    public void CreateSword()
    {
        Instantiate(SwordJudgement, transform.position, Quaternion.Euler(-90, warriorAtk.transform.rotation.eulerAngles.y, 0));
        Destroy(this.gameObject);
    }
}
