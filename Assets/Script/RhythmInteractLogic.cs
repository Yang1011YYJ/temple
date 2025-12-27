using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RhythmInteractLogic : MonoBehaviour
{
    [Header("變化組的回饋方式")]
    public Light2D TargetLight;
    public Renderer TargetRenderer;
    public int combo = 0;
    // Start is called before the first frame update
    void Reset()
    {
        TargetRenderer = TargetLight.GetComponent<Renderer>();
    }
    void Start()
    {
        TargetRenderer = TargetLight.GetComponent<Renderer>();
    }

    public void OnPress()
    {
        if (!RhythmManager.Instance) return;

        if (RhythmManager.Instance.IsOnBeat())
        {
            combo++;
            ApplyComboEffect();
        }
        else
        {
            combo = 0;
            ResetEffect();
        }
    }

    void ApplyComboEffect()
    {
        float intensity = Mathf.Clamp(combo * 0.5f, 0f, 5f);
        TargetRenderer.material.SetFloat("_Emission", intensity);
    }

    void ResetEffect()
    {
        TargetRenderer.material.SetFloat("_Emission", 0f);
    }
}
