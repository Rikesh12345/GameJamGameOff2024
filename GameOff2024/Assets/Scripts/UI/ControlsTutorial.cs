using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorial : MonoBehaviour
{
    private bool _controlsShown;

    private void OnTriggerStay2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("Player") && !_controlsShown)
            {
                GameObjectManager.Instance.SetRowingText(true);
                _controlsShown = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("Player"))
            {
                GameObjectManager.Instance.SetRowingText(false);
                GameObjectManager.Instance.SetGuideArrow(true);
                Destroy(gameObject);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerExit2D: " + ex);
        }
    }
}
