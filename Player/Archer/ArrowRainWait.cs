using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainWait : MonoBehaviour
{
    [SerializeField]
    GameObject ArrowRain;
    PlayerStateManager stateManager;
    ArcherAttack archerAtk;

    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 25; // Y축 누적 이동량 

    void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        archerAtk = FindObjectOfType<ArcherAttack>();
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

        if (Input.GetKeyUp(KeyCode.R) && stateManager.IsAttack())
        {
            stateManager.AttackEnd();
            CallArrowRain();
        }
    }

    public void CallArrowRain()
    {
        Instantiate(ArrowRain, transform.position, Quaternion.Euler(-90, archerAtk.transform.rotation.eulerAngles.y, 0));
        Destroy(this.gameObject);
    }
}
