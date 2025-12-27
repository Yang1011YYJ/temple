using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    [Header("節奏")]
    public float beatInterval = 1f;   // 節奏間隔（秒）
    public float tolerance = 0.15f;   // 容錯時間

    float startTime;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        startTime = Time.time;
    }

    public bool IsOnBeat()
    {
        float time = Time.time - startTime;
        float mod = time % beatInterval;

        return mod <= tolerance || mod >= beatInterval - tolerance;
    }
}
