using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField]
    private Light2D _light;
    [SerializeField]
    private float _maxWait = 1;
    [SerializeField]
    private float _maxFlicker = 0.2f;

    float _timer;
    float _interval;

    private void Update()
    {
        try
        {
            _timer += Time.deltaTime;
            if (_timer > _interval)
            {
                ToggleLight();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void ToggleLight()
    {
        try
        {
            _light.enabled = !_light.enabled;
            if (_light.enabled)
            {
                _interval = UnityEngine.Random.Range(0, _maxWait);
            }
            else
            {
                _interval = UnityEngine.Random.Range(0, _maxFlicker);
            }

            _timer = 0;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ToggleLight: " + ex);
        }
    }
}
