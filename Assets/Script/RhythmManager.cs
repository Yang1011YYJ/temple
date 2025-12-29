using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    [Header("節奏資料（由外部指定）")]
    public List<float> customBeats;   // 秒數列表
    public float tolerance = 0.15f;   // 容錯時間

    public int currentBeatIndex = 0;  // 節奏自己跑的索引
    public float startTime;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        startTime = Time.time;
    }
    void Update()
    {
        if (customBeats == null || currentBeatIndex >= customBeats.Count)
            return;

        float time = Time.time - startTime;

        // 節奏自己往前跑
        if (time >= customBeats[currentBeatIndex])
        {
            currentBeatIndex++;
        }
    }

    public void SetBeatData(List<float> beats)
    {
        customBeats = new List<float>(beats); // 複製一份，避免共用
        currentBeatIndex = 0;
        startTime = Time.time;
    }
    /// <summary>
    /// 判斷是否踩在節奏範圍內
    /// </summary>
    public bool IsOnBeat()
    {
        if (customBeats == null || currentBeatIndex >= customBeats.Count)
            return false;

        float time = Time.time - startTime;
        float nearestBeatTime = customBeats[currentBeatIndex];

        return Mathf.Abs(time - nearestBeatTime) <= tolerance;
    }

    /// <summary>
    /// 距離下一拍還有多少秒
    /// </summary>
    public float GetSecondsToNextBeat()
    {
        if (customBeats == null || currentBeatIndex >= customBeats.Count)
            return 0f;

        float time = Time.time - startTime;
        return Mathf.Max(0f, customBeats[currentBeatIndex] - time);

    }

    //距離節奏多遠
    public float GetBeatDistance()
    {
        if (customBeats == null || currentBeatIndex >= customBeats.Count)
            return float.MaxValue;

        float time = Time.time - startTime;
        return Mathf.Abs(time - customBeats[currentBeatIndex]);
    }
}
