using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    private Unit _unit;

    [Header("UI Elements")]
    [SerializeField]
    private Transform _healthCanvasTransform;
    [SerializeField]
    private Transform _healthBarTransform;
    [SerializeField]
    private Image _healthBarFill;

    private Transform _mainCameraTransform;

    private Vector3 _positionOffset;


    public void Start()
    {
        try
        {
            _unit = GetComponent<Unit>();
            _mainCameraTransform = Camera.main.transform;
            _positionOffset = _healthCanvasTransform.localPosition;
            SetHealthUI();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealth: " + ex);
        }
    }

    public void SetHealthUI()
    {
        try
        {
            SetHealth(true);

            if (_healthBarFill != null)
            {
                _healthBarFill.fillAmount = (float)_unit.Health / (float)_unit.MaxHealth;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealth: " + ex);
        }
    }

    public void SetHealth(bool active)
    {
        try
        {
            if (active)
            {
                _healthBarTransform.gameObject.SetActive(true);
            }
            else
            {
                _healthBarTransform.gameObject.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealth: " + ex);
        }
    }

    private void LateUpdate()
    {
        try
        {
            _healthCanvasTransform.position = _unit.transform.position + _positionOffset;
            _healthBarTransform.LookAt(_healthBarTransform.transform.position + _mainCameraTransform.forward);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }
}