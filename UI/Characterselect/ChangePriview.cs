using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangePriview : MonoBehaviour
{
    //캐릭터 RawImage
    public RawImage characterPreview;
    //캐릭터 선택 시 설명 Text
    public GameObject infoTxtPrefab;
    public Transform infoTxtParent;
    //각 직업별 Texture
    public Texture[] warriorTextures;
    public Texture[] magicianTextures;
    public Texture[] priestTextures;
    public Texture[] archerTextures;

    //Class 버튼 판넬
    public GameObject classPanel;
    //캐릭터 선택 시 화면 Fade
    public FadeInOut fadeInOut;
    //캐릭터 고유 값을 불러올 스크립트
    private PlayerStateManager playerStateManager;
    //현재 캐릭터 텍스트
    private GameObject currentInfoTxt;
    

    private void Start()
    {
        //캐릭터를 선택하지 않았을 시 초기 텍스처를 전사 텍스처로 설정
        characterPreview.texture = warriorTextures[0];
        //Class버튼 비활성화
        classPanel.gameObject.SetActive(false);
        //PlayerStateManager에서 캐릭터 직업별 고유 값 로드
        playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
    }

    public void ChangeCharacterTexture(int jobIndex)
    {
        Texture[] textures = null;

        switch (jobIndex)
        {
            case 0:
                textures = warriorTextures;
                CreateInfoTxt("전사: 강하고 강력한 전투사입니다.");
                playerStateManager.SetCLASS(0); // 직업 설정
                break;
            case 1:
                textures = archerTextures;
                CreateInfoTxt("궁수: 정확하고 민첩한 활잡이입니다.");
                playerStateManager.SetCLASS(1); // 직업 설정
                break;
            case 2:
                textures = magicianTextures;
                CreateInfoTxt("마법사: 비전과 주문의 달인입니다.");
                playerStateManager.SetCLASS(2); // 직업 설정
                break;
            case 3:
                textures = priestTextures;
                CreateInfoTxt("사제: 신성한 치유사이자 수호자입니다.");
                playerStateManager.SetCLASS(3); // 직업 설정
                break;
            default:
                Debug.LogError("유효하지 않은 직업 인덱스입니다!");
                return;
        }

        // RawImage 텍스처 변경
        characterPreview.texture = textures[0];

        // 직업 인덱스 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedJobIndex", jobIndex);
        PlayerPrefs.Save();

    }

    private void CreateInfoTxt(string description)
    {
        // info_txt 객체를 비활성화
        DeactivateInfoTxt();

        // 객체를 생성
        GameObject infoTxtObject = Instantiate(infoTxtPrefab, infoTxtParent);
        infoTxtObject.GetComponent<Text>().text = description;

        // 현재 info_txt 객체로 설정합니다.
        currentInfoTxt = infoTxtObject;
    }

    private void DeactivateInfoTxt()
    {
        if (currentInfoTxt != null)
        {
            // 현재의 info_txt 객체를 비활성화하고 삭제합니다.
            currentInfoTxt.SetActive(false);
            Destroy(currentInfoTxt);
        }
    }

    public void ChangeScene()
    {
        fadeInOut.StartCoroutine("FadeOutStart");
        //StartCoroutine(fadeInOut.FadeOutStart());
        Invoke("LoadScene", 3f);
    }
    public void LoadScene()
    {
        AudioManager.Instance.bgmSources[0].Stop();
        AudioManager.Instance.bgmSources[0].clip = AudioManager.Instance.bgmClips[Random.Range(1, 4)];
        AudioManager.Instance.bgmSources[0].Play();
        SceneManager.LoadScene("Map");
    }
    public void ClassButton()
    {
        classPanel.gameObject.SetActive(true);
    }

}