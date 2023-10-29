using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private PlayerStateManager stateManager;
    public GameObject player; // 바라볼 플레이어 오브젝트입니다. 
    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 25; // Y축 누적 이동량 
    public float distance = 1;
    private Vector3 velocity = Vector3.zero;
    private int toggleView = 3; // 1=1인칭, 3=3인칭 
    private float wheelspeed = 10.0f;
    private Vector3 Player_Height;
    private Vector3 Player_Side;
    private bool stopScroll = false;
    public bool NPCTalk = false;
    void Start()
    {
        stateManager = FindObjectOfType<PlayerStateManager>();
        player = GameObject.FindWithTag("Player").gameObject;
        Player_Height = new Vector3(0, 2.0f, 0f);
        Player_Side = new Vector3(0f, 0f, -2.0f);
    }

    // Update is called once per frame 
    void Update()
    {
        if(player.GetComponent<WarriorAttack>() != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<WarriorAttack>().IsSwordJudge())
            {
                stopScroll = true;
                distance = 1;
                Player_Height = new Vector3(0, 5.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -10.0f);
            }
            else if (Input.GetKeyUp(KeyCode.F) && !player.GetComponent<WarriorAttack>().IsSwordJudge())
            {
                stopScroll = false;
                Player_Height = new Vector3(0, 2.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -2.0f);
            }
        }
        else if(player.GetComponent<MagicianAttack>() != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<MagicianAttack>().IsMeteo())
            {
                stopScroll = true;
                distance = 1;
                Player_Height = new Vector3(0, 10.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -20.0f);
            }
            else if (Input.GetKeyUp(KeyCode.F) && !player.GetComponent<MagicianAttack>().IsMeteo())
            {
                stopScroll = false;
                Player_Height = new Vector3(0, 2.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -2.0f);
            }
        }
        else if(player.GetComponent<ArcherAttack>() != null)
        {
            if (Input.GetKeyDown(KeyCode.R) && player.GetComponent<ArcherAttack>().IsArrowRain())
            {
                stopScroll = true;
                distance = 1;
                Player_Height = new Vector3(0, 10.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -20.0f);
            }
            else if (Input.GetKeyUp(KeyCode.R) && !player.GetComponent<ArcherAttack>().IsArrowRain())
            {
                stopScroll = false;
                Player_Height = new Vector3(0, 2.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -2.0f);
            }
            else if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<ArcherAttack>().IsArrowStorm())
            {
                stopScroll = true;
                ymove = 0;
                distance = 1;
                Player_Height = new Vector3(0, 2.0f, 0f);
                Player_Side = new Vector3(0f, 0f, 1.0f);
            }
            else if(!player.GetComponent<ArcherAttack>().IsArrowStorm() && !player.GetComponent<ArcherAttack>().IsArrowRain())
            {
                stopScroll = false;
                Player_Height = new Vector3(0, 2.0f, 0f);
                Player_Side = new Vector3(0f, 0f, -2.0f);
            }
        }
        

        if (!stateManager.GetDIE())
        {
            xmove += Input.GetAxis("Mouse X");
            // 마우스의 좌우 이동량을 xmove 에 누적합니다.
            #region 궁수일때
            if (player.GetComponent<ArcherAttack>() != null)
            {
                if (!player.GetComponent<ArcherAttack>().IsArrowStorm())
                {
                    if (ymove >= -35 && ymove <= 60)
                    {
                        ymove -= Input.GetAxis("Mouse Y");
                        // 마우스의 상하 이동량을 ymove 에 누적합니다. 
                    }
                    else if (ymove > 60)
                    {
                        if (Input.GetAxis("Mouse Y") > 0)
                            ymove -= Input.GetAxis("Mouse Y");
                    }
                    else
                    {
                        if (Input.GetAxis("Mouse Y") < 0)
                            ymove -= Input.GetAxis("Mouse Y");
                    }
                }
            }
            #endregion
            else
            {
                if (ymove >= -35 && ymove <= 60)
                {
                    ymove -= Input.GetAxis("Mouse Y");
                    // 마우스의 상하 이동량을 ymove 에 누적합니다. 
                }
                else if (ymove > 60)
                {
                    if (Input.GetAxis("Mouse Y") > 0)
                        ymove -= Input.GetAxis("Mouse Y");
                }
                else
                {
                    if (Input.GetAxis("Mouse Y") < 0)
                        ymove -= Input.GetAxis("Mouse Y");
                }
            }

            if (!NPCTalk)
            {
                transform.rotation = Quaternion.Euler(ymove, xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다. 
                if (!stateManager.IsAttack() && !stateManager.GetSTUN())
                    player.transform.rotation = Quaternion.Euler(0, xmove, 0);
            }
            

            if (toggleView == 3 && !stopScroll)
            {
                distance -= Input.GetAxis("Mouse ScrollWheel") * wheelspeed;
                if (distance < 1f) distance = 1f;
                if (distance > 10.0f) distance = 10.0f;
            }

            if (toggleView == 3)
            {
                Vector3 Eye = player.transform.position
                    + transform.rotation * Player_Side + Player_Height;
                Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance);
                // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다. 
                transform.position = Eye - transform.rotation * reverseDistance;
            }
        }
    }
}
