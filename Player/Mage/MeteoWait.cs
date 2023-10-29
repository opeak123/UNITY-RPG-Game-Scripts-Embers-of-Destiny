using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoWait : MonoBehaviour
{
    [SerializeField]
    GameObject MeteoStrikeFire;
    PlayerStateManager stateManager;
    MagicianAttack mageAtk;

    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 25; // Y축 누적 이동량 

    void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        mageAtk = FindObjectOfType<MagicianAttack>();
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
            CallMeteo();
        }
    }

    public void CallMeteo()
    {
        Instantiate(MeteoStrikeFire, transform.position, Quaternion.Euler(-90, mageAtk.transform.rotation.eulerAngles.y, 0));
        Destroy(this.gameObject);
    }
}
