using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffectController : MonoBehaviour {

    [SerializeField] ParticleSystem[] starEffects;
    [SerializeField] ParticleSystem[] warpEffects;

    [SerializeField] float maxSimulateSpeed;
    [SerializeField] float deltaSimulateSpeed;
    private float nowSimulateSpeed;

    // Use this for initialization
    void Start () {
        
    }

    public IEnumerator AccelWarpEffect()
    {
        while (maxSimulateSpeed >= nowSimulateSpeed)
        {
            nowSimulateSpeed += deltaSimulateSpeed;
            for (int i = 0; i < warpEffects.Length; i++)
            {
                var _main = warpEffects[i].main;
                _main.simulationSpeed = nowSimulateSpeed;
            }
            yield return new WaitForSeconds(0.1f);
        }
        StopWarpEffect();
    }

    public void WarpSequence(TeleportType teleportType)
    {
        StartCoroutine(WarpRoutine(teleportType));
    }

    private IEnumerator WarpRoutine(TeleportType teleportType)
    {
        StartWarpEffect();

        yield return StartCoroutine(AccelWarpEffect());

        if(teleportType == TeleportType.City)
        {
            GameManager.Instance.SettingCity();
        }
        else if(teleportType == TeleportType.Catle)
        {
            GameManager.Instance.SettingCatle();
        }
    }

    private void StartWarpEffect()
    {
        InitWarpEffectSpeed();
        for (int i = 0; i < starEffects.Length; i++)
        {
            starEffects[i].Play();
        }
        for (int i = 0; i < warpEffects.Length; i++)
        {
            warpEffects[i].Play();
        }
    }

    private void StopWarpEffect()
    {
        for (int i = 0; i < starEffects.Length; i++)
        {
            starEffects[i].Stop();
        }
        for (int i = 0; i < warpEffects.Length; i++)
        {
            warpEffects[i].Stop();
        }
    }

    private void InitWarpEffectSpeed()
    {
        for (int i = 0; i < warpEffects.Length; i++)
        {
            var _main = warpEffects[i].main;
            _main.simulationSpeed = 1;
        }
        nowSimulateSpeed = 1;
    }
}
