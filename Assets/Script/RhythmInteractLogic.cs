using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RhythmInteractLogic : MonoBehaviour
{
    [Header("變化組的回饋方式")]
    public GameObject ChildSet;
    public Light2D TargetLight;
    public Renderer TargetRenderer;


    [Header("亮度設定")]
    public float minIntensity = 0.5f;    // 失敗時亮度
    public float maxIntensity = 5f;      // combo 最大亮度
    public float intensityStep = 0.5f;   // 每次成功增加多少

    [Header("節奏管理")]
    public BeatDataS0 beatData;
    public RhythmManager rhythmManager;
    float perfectRange;
    float goodRange;


    public float combo = 0f;
    bool isLighting = false;

    [Header("漸變設定")]
    public float fadeTime = 0.3f;
    public float stayTime = 1f;

    void Reset()
    {
        TargetRenderer = ChildSet.GetComponent<Renderer>();
    }

    void Start()
    {
        TargetRenderer = ChildSet.GetComponent<Renderer>();
        if (TargetLight) TargetLight.intensity = 0f;

        if (rhythmManager && beatData)
        {
            rhythmManager.SetBeatData(beatData.beats);
        }

    }
    void OnMouseDown()
    {
        if (!rhythmManager || isLighting) return;

        float dist = rhythmManager.GetBeatDistance();
        float targetIntensity;

        perfectRange = rhythmManager.tolerance;
        goodRange = rhythmManager.tolerance * 2f;

        if (dist <= perfectRange)
        {
            // Perfect
            combo += 1f;
            targetIntensity = Mathf.Clamp(combo * intensityStep, 0f, maxIntensity);
        }
        else if (dist <= goodRange)
        {
            // Good（接近）
            combo += 0.5f;
            targetIntensity = Mathf.Clamp(combo * intensityStep, 0f, maxIntensity);
        }
        else
        {
            // Miss
            combo = 0f;
            targetIntensity = minIntensity;
        }

        StartCoroutine(LightRoutine(targetIntensity));
    }

    IEnumerator LightRoutine(float targetIntensity)
    {
        isLighting = true;

        // 淡入到計算亮度
        yield return StartCoroutine(FadeEmission(0f, targetIntensity, fadeTime));

        // 停留
        yield return new WaitForSeconds(stayTime);

        // 淡出回 0
        yield return StartCoroutine(FadeEmission(targetIntensity, 0f, fadeTime));

        isLighting = false;
    }

    IEnumerator FadeEmission(float from, float to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            TargetRenderer.material.SetFloat("_Emission", Mathf.Lerp(from, to, lerp));
            if (TargetLight)
                TargetLight.intensity = Mathf.Lerp(from, to, lerp);
            yield return null;
        }

        TargetRenderer.material.SetFloat("_Emission", to);
        if (TargetLight) TargetLight.intensity = to;
    }
}
