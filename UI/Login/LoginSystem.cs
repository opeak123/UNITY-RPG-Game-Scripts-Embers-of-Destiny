using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoginSystem : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public Text outputText;
    public GameObject loginState;


    void Start()
    {
        FirebaseAuthManager.Instance.LoginState += OnChangedState;
        FirebaseAuthManager.Instance.Init();

        UISoundPlay.Instance.LoginBGM();
        loginState.SetActive(false);
    }

    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        outputText.text += FirebaseAuthManager.Instance.UserId;

        if (sign)
        {
            // 게임 로그인 확인창 띄우기, 다음씬으로 이동하는 버튼 구현
            loginState.SetActive(true);
        }
    }

    public void Create()
    {
        string e = email.text;
        string p = password.text;

        FirebaseAuthManager.Instance.Create(e, p);
    }
    public void Login()
    {
        FirebaseAuthManager.Instance.Login(email.text, password.text);
    }
    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }

    public void Acess()
    {
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("CharacterSelect");
    }
}
