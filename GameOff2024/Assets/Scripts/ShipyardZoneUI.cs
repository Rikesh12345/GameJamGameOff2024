using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipyardZoneUI : MonoBehaviour
{

    [SerializeField]
    private GameObject _shipyardIcon;
    [SerializeField]
    private GameObject _shipyardUIGO;

    private ShipyardUI _shipyardUI;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _shipyardUI = _shipyardUIGO.GetComponent<ShipyardUI>();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                Unit playerUnit = collision.gameObject.GetComponent<Unit>();
                _shipyardUI.SetUnitVisuals(playerUnit.UnitVisuals.sprite);
                _shipyardIcon.SetActive(false);
                GameObjectManager.Instance.SetUiElements(false);
                _shipyardUIGO.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerStay2D: " + ex);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                _shipyardIcon.SetActive(true);
                _shipyardUIGO.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerExit2D: " + ex);
        }
    }

}
