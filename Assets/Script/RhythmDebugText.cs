using UnityEngine;
using TMPro;

public class RhythmVisualizer : MonoBehaviour
{
    [Header("節奏來源")]
    public RhythmManager rhythmManager;
    public RhythmInteractLogic interactLogic;

    [Header("顯示文字")]
    public TextMeshProUGUI beatText;
    public TextMeshProUGUI comboText;

    void Update()
    {
        if (!rhythmManager || !interactLogic) return;

        // 倒數只算距離下一拍
        float secondsToNextBeat = rhythmManager.GetSecondsToNextBeat();
        beatText.text = $"Next Beat in: {secondsToNextBeat:F2}s";

        // 顯示 combo
        comboText.text = $"Combo : {interactLogic.combo:0.0}";

    }

    //// 給互動物件呼叫，成功踩到節奏
    //public void OnHitSuccess()
    //{
    //    currentCombo++;
    //    comboText.text = $"Combo : {currentCombo}";
    //}

    //// 給互動物件呼叫，踩錯節奏
    //public void OnHitFail()
    //{
    //    currentCombo = 0;
    //    comboText.text = $"Combo : 0";
    //}
}
