using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingModeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _sailingModeUI;
    [SerializeField]
    private GameObject _sailingModeParticleEffect;

    private bool _firstTimeActivated = false;

    public bool FirstTimeActivated { get => _firstTimeActivated; set => _firstTimeActivated = value; }

    private bool _particleEffectShown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (GameManager.Instance.PlayerController.IsSailingMode)
            {
                if (FirstTimeActivated)
                {
                    if (!_particleEffectShown)
                    {
                        _sailingModeParticleEffect.SetActive(true);
                        _particleEffectShown = true;
                    }
                    GameObjectManager.Instance.SetSailingModeText(false);
                }
                _sailingModeUI.SetActive(true);
            }
            else
            {
                _sailingModeParticleEffect.SetActive(false);
                _sailingModeUI.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }
}
