using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtShipyardTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.IsPlayerAtShipyard = true;
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
                GameManager.Instance.IsPlayerAtShipyard = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerExit2D: " + ex);
        }
    }
}
