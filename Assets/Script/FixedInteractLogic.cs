using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FixedInteractLogic : MonoBehaviour
{
    [Header("固定組的回饋方式")]
    public Light2D TargetLight;
    public Renderer TargetRenderer;
    public float lightDuration = 1f;
    public bool isLighting = false;

    void Reset()
    {
        TargetRenderer = TargetLight.GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TargetRenderer = TargetLight.GetComponent<Renderer>();
    }

    public void OnPress()
    {
        if (isLighting) return;
        StartCoroutine(LightRoutine());
    }

    public IEnumerator LightRoutine()
    {
        isLighting = true;

        SetLight(true);
        yield return new WaitForSeconds(lightDuration);
        SetLight(false);

        isLighting = false;
    }

    void SetLight(bool on)
    {
        if (!TargetLight) return;
        TargetRenderer.material.SetFloat("_Emission",on? 1 : 0);
    }
}
