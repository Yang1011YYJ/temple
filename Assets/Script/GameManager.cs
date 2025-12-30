using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("腳本")]
    public static GameManager Instance;
    public other currentOther; // 場景內的 other
    public MenuScript menuScript;
    public GoogleFormUploader_Full googleUploader;


    private float stageStartTime;
    private int currentStage = 0; // 1 = 固定, 2 = 節奏
    private int stageCount = 0;//第幾關
    private List<string> logLines = new List<string>();

    [Header("表單上傳")]
    private string MenuStartTime;
    private string stage1StartTime, stage1EndTime, stage1Duration;//流程上第一個先出現的關卡
    private string stage2StartTime, stage2EndTime, stage2Duration;//流程上第二個先出現的關卡
    private int firstStageF, secondStageF;



    public GameObject currentLight2D;
    public GameObject currentBlackPanel;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //// 設定檔案路徑
            //logFilePath = Application.persistentDataPath + "/GameLog.txt";
        }
        else
        {
            Destroy(gameObject);
        }
        currentOther = FindAnyObjectByType<other>();
        menuScript = FindAnyObjectByType<MenuScript>();
        googleUploader = FindObjectOfType<GoogleFormUploader_Full>();

    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 每次進新場景都找一次
        currentOther = FindObjectOfType<other>();
        if (currentOther != null)
        {
            Debug.Log($"找到 other：{currentOther.name}（場景 {scene.name}）");
        }
        else
        {
            Debug.Log($"場景 {scene.name} 沒有 other（可能是 menu）");
        }

        currentLight2D = GameObject.Find("Global Light 2D");
        if (currentLight2D != null)
        {
            Debug.Log($"找到 currentLight2D：{currentLight2D.name}（場景 {scene.name}）");
        }
        else
        {
            Debug.Log($"場景 {scene.name} 沒有 currentLight2D");
        }
    }

    // 你之後所有要對 other 的操作，都透過這裡
    public other GetOther()
    {
        return currentOther;
    }

    //=================================
    //遊戲相關功能
    //=================================

    // Menu按下開始呼叫
    public void StartGame()
    {
        // 用真實世界時間作為開始時間
        string menuStartTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        logLines.Add($"MenuStartTime: {menuStartTime}");
        MenuStartTime = menuStartTime;

        // 隨機選第一關：1 = 固定, 2 = 節奏
        // 隨機決定第一關
        currentStage = UnityEngine.Random.Range(1, 3);
        logLines.Add($"FirstStageSelected: {currentStage}");
        firstStageF = currentStage;

        LoadStageScene(currentStage);
    }

    void LoadStageScene(int stage)
    {
        string sceneName = stage == 1 ? "FixedStage" : "RhythmStage";
        Debug.Log(sceneName);
        if (currentBlackPanel == null)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }
        if (currentBlackPanel != null)
        {
            currentBlackPanel.SetActive(true);
            Fade(currentBlackPanel, 0, 1, 2, () => SceneChange(sceneName));
        }
        else Debug.Log("沒有BlackPanel無法淡出");
    }

    // 關卡開始時呼叫
    public void StageStart()
    {
        stageStartTime = Time.time;// 遊戲內秒數
        // 用真實世界時間作為開始時間
        string stageStartTimeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        logLines.Add($"Stage{currentStage}_StartTime: {stageStartTimeStr}");

        if (stageCount == 0)
        {
            stage1StartTime = stageStartTimeStr;
        }
        else
        {
            stage2StartTime = stageStartTimeStr;
        }
    }

    // 關卡結束時呼叫
    public void StageEnd()
    {
        float stageEndTime = Time.time;// 遊戲內秒數
        float duration = stageEndTime - stageStartTime;
        string stageEndTimeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // 現實時間

        

        // 第一關剛結束，開始第二關
        if (stageCount == 0)
        {
            logLines.Add($"Stage{currentStage}_EndTime: {stageEndTimeStr}");
            logLines.Add($"Duration: {duration:F2}");
            stageCount++;

            stage1EndTime = stageEndTimeStr;
            stage1Duration = duration.ToString("F2");

            // 開始第二關
            int secondStageT = currentStage == 1 ? 2 : 1;
            currentStage = secondStageT;
            logLines.Add($"SecondStageSelected: {currentStage}");
            secondStageF = currentStage;

            LoadStageScene(currentStage);
        }
        else
        {
            // 第二關結束，寫入檔案
            logLines.Add($"Stage{currentStage}_EndTime: {stageEndTimeStr}");
            logLines.Add($"Duration2: {duration:F2}");

            stage2EndTime = stageEndTimeStr;
            stage2Duration = duration.ToString("F2");

            SaveLogToFile();

            // 同時上傳 Google Form
            if (googleUploader != null)
            {
                googleUploader.UploadPlayData(
                    menuTime: MenuStartTime,
                    firstLevelID: firstStageF.ToString(),
                    firstStartTime: stage1StartTime,
                    firstEndTime: stage1EndTime,
                    firstDuration: stage1Duration, // 第一關 duration
                    secondLevelID: secondStageF.ToString(),
                    secondStartTime: stage2StartTime,
                    secondEndTime: stage2EndTime,
                    secondDuration: stage2Duration // 第二關 duration
                );
            }
        }
    }
    string GetLogValue(string key)
    {
        foreach (var line in logLines)
        {
            if (line.StartsWith(key))
            {
                return line.Substring(key.Length + 2); // 去掉 "key: "
            }
        }
        return "";
    }


    void SaveLogToFile()
    {
        // 取得時間戳作檔名
        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"PlayTime_{timeStamp}.txt";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllLines(path, logLines);
        Debug.Log("遊玩紀錄已儲存：" + path);
        logLines.Clear();

        currentOther.BackToMenu();
    }


    //=================================
    //通用功能
    //=================================
    public void SceneChange(string SceneName)
    {
        StopAllCoroutines(); // 或 StopCoroutine(已知Fade Coroutine)

        SceneManager.LoadScene(SceneName);
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
    public IEnumerator FadeOutGlobalLightandSceneChange(float duration, string sceneName)
    {

        float time = 0f;
        float startIntensity = currentLight2D.GetComponent<Light2D>().intensity;

        while (time < duration)
        {
            time += Time.deltaTime;
            currentLight2D.GetComponent<Light2D>().intensity = Mathf.Lerp(startIntensity, 0f, time / duration);
            yield return null;
        }

        currentLight2D.GetComponent<Light2D>().intensity = 0f;

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeI(GameObject gameObject, float start, float end, float duration, Action onComplete)
    {
        if (gameObject == null) yield break;

        CanvasGroup canvasG = gameObject.GetComponent<CanvasGroup>();
        if (canvasG == null) yield break;

        float time = 0f;
        canvasG.alpha = start;

        while (time < duration)
        {
            if (canvasG == null) yield break; // <- 這行保護
            if (gameObject == null) yield break; // <- 這行保護

            time += Time.deltaTime;
            canvasG.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        if (canvasG != null)
            canvasG.alpha = end;

        onComplete?.Invoke();
    }
}
