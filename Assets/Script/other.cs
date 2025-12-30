using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class other : MonoBehaviour
{
    [Header("UI")]
    public GameObject SettingPanel;
    public GameObject EndGameButton;
    public GameObject EndGameCheckPanel;
    public GameObject BlackPanel;

    public void CloseUI()
    {
        EndGameCheckPanel.SetActive(false);
        SettingPanel.SetActive(false);
        BlackPanel.SetActive(false);
    }

    private void Start()
    {
        CloseUI();
        BlackPanel.SetActive(true);
        Fade(BlackPanel,1,0,1,()=> CloseUI());
    }

    public void BackToMenu()
    {
        BlackPanel.SetActive(true);
        Fade(BlackPanel, 0, 1, 2, ()=>SceneChange("Menu"));
    }

    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void Fade(GameObject gameObject, float start, float end, float duration, Action onComplete)
    {
        StartCoroutine(FadeI(gameObject, start, end, duration, onComplete));
    }

    public IEnumerator FadeI(GameObject gameObject, float start, float end, float duration, Action onComplete)
    {
        float time = 0f;
        CanvasGroup canvasG = gameObject.GetComponent<CanvasGroup>();

        canvasG.alpha = start;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasG.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        canvasG.alpha = end;

        onComplete?.Invoke();

    }


}
