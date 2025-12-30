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
    public GameObject illustrateText;
    public GameObject illustratePanel;
    public GameObject StartButton;
    public GameObject UIAll;

    [Header("UI")]
    public GameObject SettingPanel;
    public GameObject EndGameButton;
    public GameObject EndGameCheckPanel;

    public Light2D Globalight2D;
    // Start is called before the first frame update
    void Start()
    {
        textRevealFromCenter = FindAnyObjectByType<TextRevealFromCenter>();
        if (illustratePanel != null) illustratePanel.SetActive(false);
        if (illustrateButton != null) illustrateButton.SetActive(false);
        if (illustrateText != null) illustrateText.SetActive(false);

        CloseUI();

        StartCoroutine(MenuStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseUI()
    {
        EndGameCheckPanel.SetActive(false);
        SettingPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit(); //關閉應用程式
    }

    public IEnumerator MenuStart()
    {
        GameManager.Instance.currentBlackPanel.SetActive(true);
        GameManager.Instance.Fade(GameManager.Instance.currentBlackPanel, 1, 0, 1, () =>
        {
            GameManager.Instance.currentBlackPanel.SetActive(false);
        });
        StartCoroutine(textRevealFromCenter.Reveal());
        yield return new WaitUntil(() => illustratePanel.activeSelf);
        yield return new WaitForSeconds(1);

        StopCoroutine(GameManager.Instance.Blink(StartButton));
        StartCoroutine(GameManager.Instance.Blink(StartButton));
    }
}
