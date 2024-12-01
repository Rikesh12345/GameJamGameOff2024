using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Unit _unit;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject _healthUI;
    [SerializeField]
    private Image _healthBarFill;

    public void Start()
    {
        try
        {
            _unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
            SetHealthUI(_unit);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealth: " + ex);
        }
    }

    public void SetHealthUI(Unit unit)
    {
        try
        {
            SetHealth(true);

            //if (_unit == null)
            //{
            //    _unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
            //}

            if (_healthBarFill != null)
            {
                _healthBarFill.fillAmount = (float)unit.Health / (float)unit.MaxHealth;
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
                _healthUI.SetActive(true);
            }
            else
            {
                _healthUI.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealth: " + ex);
        }
    }
}
