using System;
using System.IO;
using UnityEngine;

public class PlayTimeRecorder : MonoBehaviour
{
    private float startTime;
    private bool isTiming = false;

    void Start()
    {
        StartTiming();
    }

    // 開始計時
    public void StartTiming()
    {
        Debug.Log("1");
        startTime = Time.time;
        isTiming = true;
    }

    // 結束計時（綁在「遊戲結束」按鈕）
    public void EndTimingAndSave()
    {
        if (!isTiming) return;

        float playTime = Time.time - startTime;
        int seconds = Mathf.FloorToInt(playTime);

        SaveToFile(seconds);
        isTiming = false;
    }

    void SaveToFile(int seconds)
    {
        string content = $"遊玩時間:{seconds}秒";

        // 取得目前時間（年月日_時分秒）
        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        string fileName = $"PlayTime_{timeStamp}.txt";

        string path = Path.Combine(
            Application.persistentDataPath,
            fileName
        );

        File.WriteAllText(path, content);

        Debug.Log("遊玩時間已儲存：" + path);
    }
}
