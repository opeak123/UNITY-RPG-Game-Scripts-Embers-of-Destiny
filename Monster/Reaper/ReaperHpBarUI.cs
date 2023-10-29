#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class ReaperHpBarUI : MonoBehaviour
{
    public ReaperData reaperData;
    public ReaperPhase reaperPhase;
    private Canvas hpCanvas;
    private Slider hpSlider;
    private Image fillRect;
    public ParticleSystem phaseParticle;
    private Transform playerTr;
    public GameObject DMGText;
    private bool reaperDead = false;
    private int dirty = 0;
    private int activatedParticleIndex = 0;
    public AudioSource[] audioSource = new AudioSource[2];
    public AudioSource bgmSource;
    private MonsterAudioClip audioClip;
    public PlayableDirector playableDirector;
    
    public void AudioLerp()
    {
        float timer = Mathf.Clamp(Time.time, 0f, 15f);
        bgmSource.volume = 1f - (timer / 15f);
    }
    private void Awake()
    {
        reaperData = new ReaperData();
        reaperPhase = ReaperPhase.Phase1;
    }
    private void Start()
    {
        InitReaperData();
        hpCanvas = transform.GetChild(0).GetComponent<Canvas>();
        hpSlider = hpCanvas.GetComponentInChildren<Slider>();
        fillRect = hpSlider.fillRect.GetComponent<Image>();
        audioClip = GetComponent<MonsterAudioClip>();

        hpSlider.maxValue = reaperData.maxHp;
        hpSlider.value = reaperData.hp;
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ReaperDamaged(2000f, transform);
            reaperData.hp -= 2000;
        }
        HpBarUpdate();
    }

    private void InitReaperData()
    {
        reaperData.name = "reaper";
        reaperData.maxHp = 8000;
        reaperData.hp = reaperData.maxHp;
        reaperData.atk = 75;
        reaperData.def = 250;
    }

    public void HpBarUpdate()
    {
        if (GetComponent<ReaperMovement>().Activated(true))
        {
            StartCoroutine(SetUpHpBar());
        }
        HpManage();
    }

    private void HpManage()
    {
        reaperDead = hpSlider.value <= 0;

        if (reaperDead)
        {
            ReaperDead();
            return;
        }

        hpSlider.value = Mathf.Lerp(hpSlider.value, reaperData.hp, Time.deltaTime * 10f);

        if (hpSlider.value <= 8000f)
        {
            reaperPhase = ReaperPhase.Phase1;
            fillRect.color = Color.green;
        }
        if (hpSlider.value <= 4000)
        {
            reaperPhase = ReaperPhase.Phase2;
            if(activatedParticleIndex == 0)
            {
                activatedParticleIndex++;
                audioSource[1].clip = audioClip.clip[7];
                audioSource[1].Play();
            }
            phaseParticle.gameObject.SetActive(true);
            fillRect.color = Color.yellow;
        }
    }
    IEnumerator SetUpHpBar()
    {
        yield return new WaitForSeconds(2f);
        hpCanvas.enabled = true;
    }

    public void ReaperDamaged(float damage, Transform _pos)
    {
        if (reaperDead)
            return;

        if (!audioSource[1].isPlaying)
        {
            audioSource[1].clip = audioClip.clip[10];
            audioSource[1].Play();
        }
        //damage = /*데미지 계산 공식 */
        if (GetComponent<ReaperAnimation>() != null)
        {
            GetComponent<ReaperAnimation>().ReaperDamage();
        }
        reaperData.hp -= (int)(damage - (float)reaperData.def * 0.1f);
        GameObject dmgtext = Instantiate(DMGText, _pos.position, playerTr.rotation, transform);
        dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
        dmgtext.GetComponent<TextMeshPro>().fontSize = 15;
        dmgtext.GetComponent<DamageText>().damage = (int)(damage - (float)reaperData.def * 0.1f);
    }

    private void ReaperDead()
    {
        if(dirty == 0)
        {
            dirty++;
            playableDirector.Play();
            audioSource[0].clip = audioClip.clip[9];
            audioSource[0].Play();
            GetComponent<ReaperMovement>().ReaperDead(true);
            GetComponent<ReaperAnimation>().ReaperDead();
        }
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(12f);
         this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDamage>() != null)
        {
            ReaperDamaged(other.GetComponent<PlayerDamage>().DMG(), other.transform);
        }
    }
}
