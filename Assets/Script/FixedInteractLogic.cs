using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FixedInteractLogic : MonoBehaviour
{
    [Header("固定組的回饋方式")]
    public GameObject ChildSet;
    public Light2D TargetLight;

    [Header("亮度設定")]
    [Tooltip("亮度停留時間")]public float lightDuration = 1f;
    [Tooltip("亮度淡入時間")] public float fadeTime = 0.3f;
    [Tooltip("亮?")] bool isLighting = false;
    [Tooltip("亮度目標")] public float targetIntensity;

    void Start()
    {
        if (!TargetLight)
        {
            Debug.LogError("[FixedInteractLogic] TargetLight not assigned.");
            return;
        }

        // 記住你在 Inspector 調好的亮度
        targetIntensity = TargetLight.intensity;

        // 一開始關燈
        TargetLight.intensity = 0f;
    }

    void OnMouseDown()
    {
        OnPress();
    }

    public void OnPress()
    {
        if (isLighting) return;
        StartCoroutine(LightRoutine());
    }

    IEnumerator LightRoutine()
    {
        isLighting = true;

        // 漸亮
        yield return StartCoroutine(FadeLight(0f, targetIntensity));

        // 停留
        yield return new WaitForSeconds(lightDuration);

        // 漸暗
        yield return StartCoroutine(FadeLight(targetIntensity, 0f));

        isLighting = false;
    }

    IEnumerator FadeLight(float from, float to)
    {
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float lerp = t / fadeTime;
            TargetLight.intensity = Mathf.Lerp(from, to, lerp);
            yield return null;
        }

        TargetLight.intensity = to;
    }
}
