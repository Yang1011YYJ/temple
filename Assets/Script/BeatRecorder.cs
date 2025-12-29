using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
#endif
public class BeatRecorder : MonoBehaviour
{
    [Header("歌曲播放")]
    public AudioSource music;             // 拖入歌曲
    public KeyCode recordKey = KeyCode.Space;

    private List<float> beatTimes = new List<float>();
    private float startTime;
    private bool isPlaying = false;

    void Update()
    {
        if (!isPlaying && Input.GetKeyDown(KeyCode.Return)) // 按 Enter 開始
        {
            StartRecording();
        }

        if (isPlaying)
        {
            if (Input.GetKeyDown(recordKey))
            {
                float time = Time.time - startTime;
                beatTimes.Add(time);
                Debug.Log("Beat recorded: " + time);
            }

            // 歌曲播完自動停止
            if (!music.isPlaying)
            {
                StopRecording();
            }
        }
    }

    void StartRecording()
    {
        beatTimes.Clear();
        startTime = Time.time;
        music.Play();
        isPlaying = true;
        Debug.Log("Recording started...");
    }

    void StopRecording()
    {
        isPlaying = false;
        Debug.Log("Recording stopped. Total beats: " + beatTimes.Count);

        // 存成 txt
        string path = Application.dataPath + "/RecordedBeats.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (float t in beatTimes)
            {
                writer.WriteLine(t);
            }
        }
        Debug.Log("Saved beats to: " + path);
    }
}
