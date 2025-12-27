using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCore : MonoBehaviour
{
    [Header("目前狀態")]
    public bool interactable = true;

    [Header("腳本")]
    public FixedInteractLogic fixedLogic;
    public RhythmInteractLogic rhythmLogic;
    // Start is called before the first frame update

    private void Awake()
    {
        fixedLogic = FindAnyObjectByType<FixedInteractLogic>();
        rhythmLogic = FindAnyObjectByType<RhythmInteractLogic>();

        if (fixedLogic && rhythmLogic)
        {
            Debug.LogError($"{name} 同時掛了 Fixed 與 Rhythm，請只留一個");
        }

        if (!fixedLogic && !rhythmLogic)
        {
            Debug.LogWarning($"{name} 沒有掛任何互動邏輯");
        }
    }
    void OnMouseDown()
    {
        if (!interactable) return;

        if (fixedLogic)
            fixedLogic.OnPress();

        if (rhythmLogic)
            rhythmLogic.OnPress();
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }
}
