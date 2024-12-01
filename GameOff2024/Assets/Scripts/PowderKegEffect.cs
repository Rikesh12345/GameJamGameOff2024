using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKegEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionEffectLight;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            StartCoroutine(DeactivateAfterDuration(0.5f));
            Destroy(gameObject, 3f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private IEnumerator DeactivateAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _explosionEffectLight.SetActive(false);
    }
}
