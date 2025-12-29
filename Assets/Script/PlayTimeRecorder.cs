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

        string path = Path.Combine(
            Application.persistentDataPath,
            "PlayTime.txt"
        );

        File.WriteAllText(path, content);

        Debug.Log("遊玩時間已儲存：" + path);
    }
}
