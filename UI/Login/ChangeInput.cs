using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInput : MonoBehaviour // 로그인창 Input 이동용 스크립트
{
    EventSystem system;

    [SerializeField] Selectable firstInput;
    [SerializeField] Button LoginButton;

    private void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if(next != null)
            {
                next.Select();
            }       
        }
        else if (Input.GetKeyDown(KeyCode.Tab) )
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginButton.onClick.Invoke();

            Debug.Log("Button Pressed");
        }
    }
}
