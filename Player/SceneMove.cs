using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneMove : MonoBehaviour
{
    public string SceneName;
    public GameObject loadingBox;
    public Text loadingText;
    public Slider lodingSlider;
    private float loadingTime;
    public CanvasGroup exitGroup;
    public int dirty;
    public void OnClickExitButton()
    {
        exitGroup.alpha = 0;
        exitGroup.blocksRaycasts = false;
        exitGroup.interactable = false;
    }
    public void NextScene()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        saveManager.Save();
        StartCoroutine(LoadAsync(SceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        dirty++;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;
        loadingBox.SetActive(true);

        while (!asyncOp.isDone)
        {
            loadingTime += Time.deltaTime * 0.08f;
            lodingSlider.value = loadingTime;
            loadingText.text = loadingTime.ToString("P0");

            if (lodingSlider.value < 0.9f)
            {
                lodingSlider.value = loadingTime;
            }
            else
            {
                lodingSlider.value = Mathf.Clamp01(loadingTime);
                if (lodingSlider.value >= 1f)
                {
                    asyncOp.allowSceneActivation = true;
                    dirty = 0;
                }
            }

            yield return null;
        }
    }
}