using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatData", menuName = "Rhythm/BeatData")]
public class BeatDataS0 : ScriptableObject
{
    public string songName;         // 可以對應歌曲
    public AudioClip audioClip;     // 對應的音樂
    public List<float> beats;       // 節拍秒數

    [Header("TXT 匯入（拖入自動解析）")]
    public TextAsset txtBeats;

    // 每次 Inspector 變動時自動處理
    private void OnValidate()
    {
        if (txtBeats == null) return;

        beats = new List<float>();

        string[] lines = txtBeats.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            if (float.TryParse(line, out float t))
            {
                beats.Add(t);
            }
            else
            {
                Debug.LogWarning("無法解析: " + line);
            }
        }
    }
}
