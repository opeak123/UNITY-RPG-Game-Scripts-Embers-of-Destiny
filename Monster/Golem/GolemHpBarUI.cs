using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class GolemHpBarUI : MonoBehaviour
{
    public Canvas canvas;
    public Slider hpSlider;
    public Image fillRect;
    public GolemData data;
    public GameObject DMGText;
    private Transform playerTr;

    private float golemMaxHp = 10000f;
    private float golemCurHp;
    private bool isGolemDead = false;
    public PlayableDirector playableDirector;
    PlayerStateManager playerStat;
    GolemController golemController;
    private void Start()
    {
        golemController = GetComponent<GolemController>();
        playerStat = new PlayerStateManager();
        StartCoroutine(SetUpHpBar());
        golemCurHp = golemMaxHp;
        hpSlider.maxValue = golemMaxHp;
        hpSlider.value = golemCurHp;
        //playerStat.SetATK(1000);
        playerTr = GameObject.FindWithTag("Player").transform;
    }
    public void GolemHpUpdate()
    {
        isGolemDead = golemCurHp <= 0;

        if (isGolemDead)
        {
            playableDirector.Play();
            GolemDead();
        }

        hpSlider.value = Mathf.Lerp(hpSlider.value, golemCurHp, Time.deltaTime * 10f);

        if (hpSlider.value <= 8000f)
        {
            golemController.golemPhase = GolemPhaseState.Phase1;
        }
        if (hpSlider.value <= 6000f)
        {
            golemController.golemPhase =  GolemPhaseState.Phase2;
            fillRect.color = Color.green;
        }
        if(hpSlider.value <= 3000f)
        {
            golemController.golemPhase = GolemPhaseState.Phase3;
            fillRect.color = Color.yellow;
        }
    }

    private IEnumerator SetUpHpBar()
    {
        yield return new WaitForSeconds(2f);
        canvas.enabled = true;
    }

    private bool GolemDead()
    {
        hpSlider.value = 0;
        FindObjectOfType<EventColliderBoosFight>().Dead = true;
        GameObject parentObj = transform.parent.gameObject;
        parentObj.AddComponent<GolemOutro>();
        gameObject.SetActive(false);
        return isGolemDead;
    }

    public void GolemDamaged(float damage, Transform _pos)
    {
        if (isGolemDead)
            return;

        golemCurHp -= (int)(damage - (float)data.def * 0.1f);
        GameObject dmgtext = Instantiate(DMGText, _pos.position, playerTr.rotation, transform);
        dmgtext.GetComponent<TextMeshPro>().color = new Color(1, 0, 0);
        dmgtext.GetComponent<TextMeshPro>().fontSize = 7;
        dmgtext.GetComponent<DamageText>().damage = (int)(damage - (float)data.def * 0.1f);
    }

    
}

