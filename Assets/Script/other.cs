using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class other : MonoBehaviour
{
    [Header("UI")]
    public GameObject SettingPanel;
    public GameObject EndGameButton;
    public GameObject EndGameCheckPanel;
    public Button endButton;



    public void CloseUI()
    {
        EndGameCheckPanel.SetActive(false);
        SettingPanel.SetActive(false);
        GameManager.Instance.currentBlackPanel.SetActive(false);
    }

    private void Start()
    {
        if (EndGameCheckPanel != null) EndGameCheckPanel.SetActive(false);
        if (SettingPanel != null) SettingPanel.SetActive(false);

        if (GameManager.Instance.currentBlackPanel != null)
        {
            GameManager.Instance.Fade(GameManager.Instance.currentBlackPanel, 1, 0, 1, () =>
            {
                CloseUI();
                GameManager.Instance.currentBlackPanel.SetActive(false);
                GameManager.Instance.StageStart();
            });
        }

        if (endButton != null)
        endButton.onClick.AddListener(() => GameManager.Instance.StageEnd());

        
    }

    public void BackToMenu()
    {
        GameManager.Instance.currentBlackPanel.SetActive(true);
        GameManager.Instance.Fade(GameManager.Instance.currentBlackPanel, 0, 1, 2, ()=>GameManager.Instance.SceneChange("Menu"));
    }

    
}
