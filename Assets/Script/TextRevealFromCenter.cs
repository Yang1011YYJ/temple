using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextRevealFromCenter : MonoBehaviour
{
    public RectTransform maskRect;
    public float revealTime = 0.5f;
    public float targetWidth = 600f; // 最終顯示寬度
    public float blinkDuration = 1f; // 閃爍一次的時間
    public float waitAfterReveal = 1f; // 顯示完成後等待時間

    public MenuScript menuScript;
    void Awake()
    {
        menuScript = FindAnyObjectByType<MenuScript>();
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public IEnumerator Reveal()
    {
        menuScript.illustrateButton.SetActive(false);
        menuScript.illustrateText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        float t = 0f;
        maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);

        while (t < revealTime)
        {
            t += Time.deltaTime;
            float width = Mathf.Lerp(0f, targetWidth, t / revealTime);
            maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            yield return null;
        }

        maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        // 2. 等待一秒
        yield return new WaitForSeconds(waitAfterReveal);

        //顯示說明按紐
        menuScript.illustrateButton.SetActive(true);
        menuScript.illustrateText.SetActive(true);

        // 3. 開始無限閃爍
        StopCoroutine(GameManager.Instance.Blink(menuScript.illustrateText));
        StartCoroutine(GameManager.Instance.Blink(menuScript.illustrateText));
    }
}
