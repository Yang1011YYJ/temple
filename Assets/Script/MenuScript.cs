using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public TextRevealFromCenter textRevealFromCenter;
    public GameObject illustrateButton;
    public GameObject illustratePanel;
    public GameObject StartButton;
    public GameObject UIAll;

    public Light2D Globalight2D;
    // Start is called before the first frame update
    void Start()
    {
        textRevealFromCenter = FindAnyObjectByType<TextRevealFromCenter>();
        illustratePanel.SetActive(false);
        illustrateButton.SetActive(false);

        StartCoroutine(MenuStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        StartCoroutine(FadeOutGlobalLightandSceneChange(1));
        Fade(UIAll, 1, 0, 1, null);
    }

    public IEnumerator MenuStart()
    {
        yield return new WaitUntil(() => illustratePanel.activeSelf);
        yield return new WaitForSeconds(1);

        StartCoroutine(Blink(StartButton));
    }

    public IEnumerator Blink(GameObject gameObject)
    {
        while (true)
        {
            Fade(gameObject, 0, 1, 0.8f, null);
            yield return new WaitForSeconds(1.5f);
            Fade(gameObject, 1, 0, 0.8f, null);
            yield return new WaitForSeconds(1.5f);

        }
    }
    public void Fade(GameObject gameObject, float start, float end, float duration, Action onComplete)
    {
        StartCoroutine(FadeI(gameObject, start, end, duration, onComplete));
    }
    IEnumerator FadeOutGlobalLightandSceneChange(float duration)
    {
        float time = 0f;
        float startIntensity = Globalight2D.intensity;

        while (time < duration)
        {
            time += Time.deltaTime;
            Globalight2D.intensity = Mathf.Lerp(startIntensity, 0f, time / duration);
            yield return null;
        }

        Globalight2D.intensity = 0f;

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("01");
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
    }
}
