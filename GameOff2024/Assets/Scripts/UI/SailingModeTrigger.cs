using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingModeTrigger : MonoBehaviour
{
    [SerializeField]
    private SailingModeUI _sailingModeUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("Player"))
            {
                if (!_sailingModeUI.FirstTimeActivated)
                {
                    GameObjectManager.Instance.SetSailingModeText(true);
                    _sailingModeUI.FirstTimeActivated = true;
                }

                
                GameObjectManager.Instance.CheckAndSpawnEnemies();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }
}
